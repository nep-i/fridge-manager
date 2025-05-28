namespace F_Framework.ViewModels.GroceriesViewModels
{
    [QueryProperty("GrocFull", "bag")]
    public partial class GroceryDetailsViewModel : BaseViewModel
    {
        public IRelayCommand GetActiveGroceryCommand { get; set; }
        public IRelayCommand GroceryIsDoneCommand { get; set; }
        public IRelayCommand<Product> MakeProductDoneCommand { get; set; }
        public IRelayCommand SyncActiveNoteCommand { get; set; }
        public IRelayCommand<Product> DragProductAdded { get; set; }
        public IRelayCommand<string> DragProductDroped { get; set; }
        public IRelayCommand AddShopCommand { get; set; }

        [ObservableProperty]
        private GroceryFull grocFull;

        [ObservableProperty]
        private string shopName;

        [ObservableProperty]
        private Product draggedProduct;

        private IToast _messageService;
        private ICustomNavigations _navigations;
        private IUnitOfWork _unitOfWork;

        public GroceryDetailsViewModel(IToast messageService, ICustomNavigations navigations, IUnitOfWork unitOfWork)
        {
            Title = "Active";
            _messageService = messageService;
            _navigations = navigations;
            _unitOfWork = unitOfWork;
            AsigningLocals();
        }

        private void AsigningLocals()
        {
            GrocFull = new GroceryFull();
            GetActiveGroceryCommand = new AsyncRelayCommand(CheckActiveGrocery);
            DragProductAdded = new AsyncRelayCommand<Product>(AddProductsToTheShop);
            MakeProductDoneCommand = new AsyncRelayCommand<Product>(MarkingProductDone);
            DragProductDroped = new AsyncRelayCommand<string>(DropProduct);
            SelectedProducts = new ObservableCollection<Product>();
            AddSelectedProducts = new AsyncRelayCommand<IEnumerable<object>>(AddSelected);
            AddShopCommand = new AsyncRelayCommand(AddShop);
            GroceryIsDoneCommand = new AsyncRelayCommand(GroceryIsDone);
            SyncActiveNoteCommand = new AsyncRelayCommand(NoteIsChanged);
        }

        private async Task NoteIsChanged()
        {
            if (grocFull.Note != "" && grocFull.Note.Length % 7 == 0)
            {
                var grocery = new Grocery()
                {
                    Id = grocFull.Id,
                    Key = grocFull.Key,
                    UsedAmount = grocFull.UsedAmount,
                    Note = grocFull.Note,
                    DateAdded = grocFull.DateAdded,
                    DateCompleted = grocFull.DateCompleted,
                    IsDone = grocFull.IsDone
                };
                _ = await _unitOfWork.Groceries.Value.Update(grocery);
            }
        }

        private async Task GroceryIsDone()
        {
            var grocery = new Grocery()
            {
                Id = grocFull.Id,
                Key = grocFull.Key,
                UsedAmount = grocFull.UsedAmount,
                Note = grocFull.Note,
                DateAdded = grocFull.DateAdded,
                DateCompleted = grocFull.DateCompleted,
                IsDone = true
            };
            _ = await _unitOfWork.Groceries.Value.Update(grocery);
            var connection = await _unitOfWork.GroceryShopProducts.Value.GetAllByFilterAsync(x => x.IsActive);
            foreach (var item in connection.elements)
            {
                item.IsActive = false;
                _ = _unitOfWork.GroceryShopProducts.Value.Update(item);
                var product = await _unitOfWork.Products.Value.GetFirstOrDefautAsync(x => x.Id == item.ProductId);
                product.element.IsDone = true;
                product.element.InFidge = true;
                _ = await _unitOfWork.Products.Value.Update(product.element);
            }
            await Task.Delay(2000);
            _messageService.MakeToast("You did it!");
            await _navigations.GoToCustomPage("FridgeView");
        }

        private async Task DropProduct(string name)
        {
            var shop = GrocFull.Shops.Where(x => x.Name == name).FirstOrDefault();
            var connection = await _unitOfWork.GroceryShopProducts.Value
                .GetFirstOrDefautAsync(x => x.ProductId == DraggedProduct.Id && x.GroceryId == grocFull.Id);
            connection.element.ShopId = shop.Id;
            await _unitOfWork.GroceryShopProducts.Value.Update(connection.element);
            shop.Products.Add(DraggedProduct);
            GrocFull.Products.Remove(DraggedProduct);
            Refresh();
        }

        public async Task<Result<Product>> AddSelected(IEnumerable<object> items)
        {
            var result = new Result<Product>();
            if (items.Count() > 0)
            {
                result.elements = items.Cast<Product>();
                SelectedProducts = new ObservableCollection<Product>(result.elements);
                result.Succeeded = true;
            }
            return result;
        }

        private void Refresh()
        {
            var bridge = GrocFull;
            GrocFull = new GroceryFull();
            GrocFull = bridge;
        }

        private async Task AddProductsToTheShop(object product)
        {
            var pr = product;
            DraggedProduct = new Product();
            DraggedProduct = product as Product;
        }

        private async Task AddShop()
        {
            var result = new Result<Shop>();
            if (!string.IsNullOrEmpty(shopName) && !string.IsNullOrWhiteSpace(shopName))
            {
                result = await _unitOfWork.Shops.Value.GetFirstOrDefautAsync(x => x.Name.ToLower() == ShopName.ToLower());
                if (result.element == null || result.element.Id == 0)
                {
                    var shop = new Shop()
                    {
                        Name = ShopName.ToLower()
                    };
                    result = await _unitOfWork.Shops.Value.Add(shop);
                    var fullShop = new ShopFull(shop);

                    GrocFull.Shops.Add(fullShop);
                    Refresh();
                }
                else
                {
                    var fullShop = new ShopFull(result.element);
                    GrocFull.Shops.Add(fullShop);
                    Refresh();
                }
            }
            ShopName = string.Empty;
        }

        private async Task MarkingProductDone(Product product)
        {
            product.IsDone = true;
            GrocFull.Products.Where(x => x.Id == product.Id).Select(x => x.IsDone == true);
            GrocFull.Shops.Select(x => x.Products.Where(x => x.Id == product.Id).Select(x => x.IsDone == true));
            Refresh();
            _ = await _unitOfWork.Products.Value.Update(product);
        }

        private async Task CheckActiveGrocery()
        {
            try
            {
                var isOnlyActive = await _unitOfWork.Groceries.Value.GetFirstOrDefautAsync(x => x.IsDone == false);
                if (isOnlyActive != null)
                {
                    var grocery = isOnlyActive.element;
                    if (grocery != null && grocery.Id != 0)
                    {
                        var getter = new GroceryFullGetter(_unitOfWork);
                        var bridge = await getter.GetActiveGroceryFull(grocery);
                        GrocFull = new GroceryFull();
                        GrocFull = bridge;
                    }
                    else
                    {
                        ShopName = "";
                        GrocFull = new GroceryFull();
                        GrocFull.IsDone = true;
                        DraggedProduct = new Product();
                    }
                }
              
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            };


           
        }
    }
}