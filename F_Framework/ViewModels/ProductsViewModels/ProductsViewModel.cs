namespace F_Framework.ViewModels.ProductsViewModels
{
    public partial class ProductsViewModel : BaseViewModel
    {
        public IRelayCommand AddProductToFridgeCommand { get; set; }
        public IRelayCommand AddProductToGroceryCommand { get; set; }
        public IRelayCommand DeleteProductsCommand { get; set; }
        //public IRelayCommand NewProductsCommand { get; private set; }

        [ObservableProperty]
        private ObservableCollection<Product> allProducts;

        private IUnitOfWork _service;
        private ICustomNavigations _navigations;
        private IToast _messageService;

        private Result<Product> _result { get; set; }

        public ProductsViewModel(IUnitOfWork service, ICustomNavigations navigations, IToast messageService)
        {
            _service = service;
            _navigations = navigations;
            _result = new Result<Product>();
            Title = "My Products";
            AllProducts = new ObservableCollection<Product>();
            SelectedProducts = new ObservableCollection<Product>();
            AddSelectedProducts = new AsyncRelayCommand<IEnumerable<object>>(AddSelected);
            GetAllProductsCommand = new AsyncRelayCommand(GetAllProducts);
            AddProductToFridgeCommand = new AsyncRelayCommand(AddProductsToFridge);
            DeleteProductsCommand = new AsyncRelayCommand(DeleteProducts);
            ToCreateNewView = new AsyncRelayCommand(AddNewProduct);
            AddProductToGroceryCommand = new AsyncRelayCommand(AddProductsToGrocery);
            ToDetailsView = new AsyncRelayCommand(GoToDetailsView);
            _messageService = messageService;
        }

        private async Task AddProductsToGrocery()
        {
            _result = new Result<Product>();
            var activeGrocery = await _service.Groceries.Value.GetFirstOrDefautAsync(x => x.IsDone == false);
            if (activeGrocery.element == null || activeGrocery.element.Id == 0)
                _messageService.MakeToast("There is no active Grocery list!");
            else
                foreach (var item in SelectedProducts)
                {
                    item.IsDone = false;
                    _ = await _service.Products.Value.Update(item);
                    var resultForExistanceOfCennection = await _service.GroceryShopProducts.Value.GetFirstOrDefautAsync(x => x.GroceryId == activeGrocery.element.Id &&
                       x.ProductId == item.Id);
                    if (resultForExistanceOfCennection.element == null)
                    {
                        var connection = new GroceryShopProduct()
                        {
                            GroceryId = activeGrocery.element.Id,
                            ProductId = item.Id
                        };
                        item.UsedAmount = +1;
                        _ = await _service.GroceryShopProducts.Value.Add(connection);
                        _ = await _service.Products.Value.Update(item);
                        _messageService.MakeToast("Products Added!");
                        var fullGrocery = new GroceryFull(activeGrocery.element);
                        await _navigations.GoToGroceryDetails(fullGrocery);
                    }
                    else
                    {
                        _messageService.MakeToast("Products already added!");
                    }
                }
            SelectedProducts = null;
            SelectedProducts = new ObservableCollection<Product>();
            SelectedProducts.Clear();
        }

        public async Task<Task> AddNewProduct()
        {
            await _navigations.GoToNewProduct();
            return Task.CompletedTask;
        }

        public async Task<Result<Product>> DeleteProducts()
        {
            _result = new Result<Product>();
            var toGetAllowence = new Result<GroceryShopProduct>();
            var ids = new List<int>();
            var keys = new List<string>();
            if (SelectedProducts != null && SelectedProducts.Count() > 0)
            {
                foreach (Product item in SelectedProducts)
                {
                    ids.Add(item.Id);
                    if (item.Key != string.Empty)
                        keys.Add(item.Key);
                }
                toGetAllowence = await _service.GroceryShopProducts.Value.GetAllByFilterAsync(x => ids.Contains(x.ProductId));
            }

            if (toGetAllowence.elements == null || toGetAllowence.elements.Count() == 0)
            {
                _result = await _service.Products.Value.RemoveRange(ids, keys);
                if (_result.Succeeded)
                {
                    //SelectedProducts.Clear();
                    //AllProducts.Clear();
                    //await _navigations.GoToCustomPage<int>("ProductsAllView", 1);
                    await GetAllProducts();
                    await Task.Delay(2200);
                    _messageService.MakeToast("Products Deleted!");
                }
            }
            else
                _messageService.MakeToast("One of the products is in use!");

            return _result;
        }

        public async Task<Result<Product>> AddProductsToFridge()
        {
            _result = new Result<Product>();
            try
            {
                foreach (Product product in SelectedProducts)
                {
                    product.InFidge = true;
                    product.UsedAmount += 1;
                    _ = await _service.Products.Value.Update(product);
                }
                _result.Succeeded = true;
                if (_result.Succeeded == true)
                {
                    await GetAllProducts();
                    await Task.Delay(2200);
                    _messageService.MakeToast("Products added to the fridge!");
                }
               
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            _result.Succeeded = true;
            

            return _result;
        }

        public async Task GetAllProducts()
        {
            _result = new Result<Product>();
            _result = await _service.Products.Value.GetAll();
            if (AllProducts != null)
            {
                AllProducts = null;
                AllProducts = new ObservableCollection<Product>();
                AllProducts.Clear();

                if (_result.elements.Any())
                {
                    var lst = _result.elements.ToList();
                    foreach (var item in lst)
                        AllProducts.Add(item);
                }
            }

            SelectedProducts = null;
            SelectedProducts = new ObservableCollection<Product>();
            SelectedProducts.Clear();
        }

        public async Task<Result<Product>> GoToDetailsView()
        {
            _result = new Result<Product>();
            if (SelectedProducts != null && SelectedProducts.Count == 1)
            {
                _result.element = SelectedProducts.FirstOrDefault();
                SelectedProducts.Clear();

                await _navigations.GoToProductDetails(_result.element);
            }
            _result.Succeeded = true;
            return _result;
        }

        public async Task<Result<Product>> AddSelected(IEnumerable<object> items)
        {
            var result = new Result<Product>();
            result.elements = items.Cast<Product>();
            SelectedProducts = new ObservableCollection<Product>(result.elements);
            result.Succeeded = true;

            return result;
        }

        //public async Task<Result<Product>> UpdateProductRating(this Product product)
        //{
        //    var result = new Result<Product>();
        //    product.UsedAmount += 1;
        //    await _service.Products.Value.Update(product);
        //    result.Succeeded = true;
        //    return result;
        //}
    }
}