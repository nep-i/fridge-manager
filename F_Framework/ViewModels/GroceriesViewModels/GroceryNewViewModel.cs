namespace F_Framework.ViewModels.GroceriesViewModels
{
    [QueryProperty(nameof(AddedProducts), "bag")]
    public partial class GroceryNewViewModel : BaseViewModel
    {
        public IRelayCommand Create { get; set; }
        public IRelayCommand AddProductCommand { get; set; }
        public IRelayCommand OnAppearanceCommand { get; set; }
        //private Result<Product> _result;

        [ObservableProperty]
        private ObservableCollection<Product> addedProducts;

        [ObservableProperty]
        private Grocery grocery;

        [ObservableProperty]
        private string newProductName;

        [ObservableProperty]
        private string groceryNote;

        private IToast _messageService;
        private ICustomNavigations _navigations;
        private IUnitOfWork _unitOfWork;

        public GroceryNewViewModel(IUnitOfWork unitOfWork, IToast messageService, ICustomNavigations navigations)
        {
            Title = $"New Grocery List";
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _navigations = navigations;
            AsigningLocals();
            Grocery = new Grocery();
        }

        private void AsigningLocals()
        {
            grocery = new Grocery();
            Create = new AsyncRelayCommand(CreateGrocery);
            AddProductCommand = new AsyncRelayCommand(AddProduct);
            OnAppearanceCommand = new AsyncRelayCommand(OnAppearance);
        }

        public async Task OnAppearance()
        {
            var result = new Result<Product>();
            result = await _unitOfWork.Products.Value.GetAll();
            StaticData.ProductNames = result.elements.Select(x => x.Name).ToList();
            AddedProducts = null;
            AddedProducts = new ObservableCollection<Product>();
            Grocery = null;
            Grocery = new Grocery();
            NewProductName = "";
        }

        private async Task AddProduct()
        {
            var result = new Result<Product>();
            result = await _unitOfWork.Products.Value.GetFirstOrDefautAsync(x => x.Name == newProductName);
            var bridge = new ObservableCollection<Product>();
            if (result.element == null)
            {
                var newProduct = new Product()
                {
                    Name = newProductName,
                    Icon = "unknown.svg",
                    DaysForLife = 1,
                    AmountToBuy = 1,
                    DateAdded = DateTime.Now
                };

                bridge = new ObservableCollection<Product>(AddedProducts.ToList());
                AddedProducts.Clear();
                foreach (var item in bridge)
                {
                    AddedProducts.Add(item);
                }
                AddedProducts.Add(newProduct);
                await _unitOfWork.Products.Value.Add(newProduct);
            }
            else
            {
                AddedProducts.Add(result.element);
                bridge = new ObservableCollection<Product>(AddedProducts.ToList());
                AddedProducts.Clear();
                foreach (var item in bridge)
                {
                    AddedProducts.Add(item);
                }
                var product = result.element;
                product.UsedAmount += 1;
                await _unitOfWork.Products.Value.Update(product);
            }
        }

        public async Task CreateGrocery()
        {
            var result = new Result<Grocery>();

            var isOnlyActive = await _unitOfWork.Groceries.Value.GetAllByFilterAsync(x => x.IsDone == false);
            if (isOnlyActive.elements != null && isOnlyActive.elements.Count() > 0)
            {
                foreach (var item in isOnlyActive.elements)
                {
                    item.IsDone = true;
                    await _unitOfWork.Groceries.Value.Update(item);
                }
                var isOnlyActiveBridges = await _unitOfWork.GroceryShopProducts.Value.GetAllByFilterAsync(x => x.IsActive == true);
                if (isOnlyActiveBridges != null && isOnlyActive.elements.Count() > 0)
                {
                    foreach (var item in isOnlyActiveBridges.elements)
                    {
                        if (item.Key != "")
                        {
                            item.IsActive = false;
                            await _unitOfWork.GroceryShopProducts.Value.Update(item);
                        }
                    }
                }

                var isOnlyActiveProducts = await _unitOfWork.Products.Value.GetAllByFilterAsync(x => x.IsDone == false);
                if (isOnlyActiveProducts != null && isOnlyActiveProducts.elements.Count() > 0)
                {
                    foreach (var item in isOnlyActiveProducts.elements)
                    {
                        item.IsDone = false;
                        await _unitOfWork.Products.Value.Update(item);
                    }
                }
            }


            grocery.IsDone = false;
            grocery.UsedAmount = 1;
            grocery.DateAdded = DateTime.Now;

            result = await _unitOfWork.Groceries.Value.Add(grocery);
            grocery.Id = result.element.Id;

            var lst = AddedProducts.ToList();
            foreach (var item in lst)
            {
                var connection = new GroceryShopProduct() { GroceryId = grocery.Id, ProductId = item.Id, IsActive = true, ShopId = 0 };
                _ = await _unitOfWork.GroceryShopProducts.Value.Add(connection);
                item.IsDone = false;
                _ = await _unitOfWork.Products.Value.Update(item);
            }

            var groceryFull = new GroceryFull(result.element);
            groceryFull.Products = AddedProducts.ToList();
            Grocery = new Grocery();
            await Task.Delay(2200);
            _messageService.MakeToast("Grocery Created!");

            await _navigations.GoToGroceryDetails(groceryFull);
        }
    }
}