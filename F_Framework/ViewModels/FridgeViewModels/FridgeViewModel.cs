namespace F_Framework.ViewModels.FridgeViewModels
{
    public partial class FridgeViewModel : BaseViewModel
    {
        public IRelayCommand AddProductToFridge { get; set; }
        public IRelayCommand DeleteProductFromFridge { get; set; }
        public IRelayCommand UpdateProductNotification { get; set; }
        public IRelayCommand TurnOffProductNotification { get; set; }

        [ObservableProperty]
        private ObservableCollection<Product> productsInFridge;

        private IUnitOfWork _service;
        private ICustomNavigations _navigations;
        private IToast _messageService;
        private IHeaderVisibility _headerVisibility;

        private Result<Product> _result { get; set; }
        public Product Product { get; set; }

        public FridgeViewModel(IUnitOfWork service, ICustomNavigations navigations, IToast toast, IHeaderVisibility header)
        {
            _service = service;
            _messageService = toast;
            _headerVisibility = header;
            Title = "My Fridge";
            _navigations = navigations;
            ToDetailsView = new AsyncRelayCommand(GoToDetailsView);

            DeleteProductFromFridge = new AsyncRelayCommand(DeleteSelectedProductFromFridge);
            AddSelectedProducts = new AsyncRelayCommand<IEnumerable<object>>(AddSelected);

            UpdateProductNotification = new AsyncRelayCommand(UpdateNotifications);

            TurnOffProductNotification = new AsyncRelayCommand(RemoveNotifications);

            GetAllProductsCommand = new AsyncRelayCommand(GetAllProducts);
            ProductsInFridge = new ObservableCollection<Product>();

            SelectedProducts = new ObservableCollection<Product>();
            
        }

        

        public async Task<bool> RemoveNotifications()
        {
            try
            {
                foreach (Product product in SelectedProducts)
                {
                    _result = await NotificationsEdit.CancelNotification(product);
                }
                CleaningSelection();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateNotifications()
        {
            try
            {
                foreach (Product product in SelectedProducts)
                {
                    _result = await NotificationsEdit.CreateNotification(product);
                }
                CleaningSelection();
                _messageService.MakeToast("Alarm has been set!");
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

            return true;
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

        public async Task<Result<Product>> AddProducts()
        {
            _result = new Result<Product>();
            _result.elements = SelectedProducts;
            _ = await _service.Products.Value.Update(_result.element);
            _result.Succeeded = true;
            return _result;
        }

        public async Task<Result<Product>> DeleteSelectedProductFromFridge()
        {
            _result = new Result<Product>();
            try
            {
                //var fridge = await _service.Fridges.Value.GetAll();
                //result.element.FridgeId = fridge.elements.FirstOrDefault().Id;
                var lst = SelectedProducts.ToList();
                foreach (var product in lst)
                {
                    if (product != null && product.GetType() == typeof(Product))
                        product.InFidge = false;
                        _ = await _service.Products.Value.Update(product);
                        _ = await NotificationsEdit.CancelNotification(product);
                }
                CleaningSelection();
                await GetAllProducts();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return _result;
        }

        public async Task<Result<Product>> AddSelected(IEnumerable<object> items)
        {
            _result = new Result<Product>();
            _result.elements = items.Cast<Product>();
            if (SelectedProducts != null)
            {
                SelectedProducts.Clear();
            }
            SelectedProducts = new ObservableCollection<Product>(_result.elements);
            _result.Succeeded = true;

            return _result;
        }

        public async Task GetAllProducts()
        {
            SelectedProducts.Clear();
            _result = new Result<Product>();
            _result = await _service.Products.Value.GetAllByFilterAsync(x => x.InFidge == true);

            var count = _result.elements.Count();
            if (count != ProductsInFridge.ToList().Count())
            {
                var lst = _result.elements.ToList();
                ProductsInFridge.Clear();

                foreach (var item in lst)
                    ProductsInFridge.Add(item);
            }
            else
            {
                CleaningSelection();
            }
        }

        private void CleaningSelection()
        {
            SelectedProducts = null;
            SelectedProducts = new ObservableCollection<Product>();
            _headerVisibility.VisibilityChanged(false);
            var bridge = ProductsInFridge.ToList();
            ProductsInFridge.Clear();
            foreach (var item in bridge)
            {
                ProductsInFridge.Add(item);
            }
        }
    }
}