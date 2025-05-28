//using IToast = F_Framework.Services.IToast;

namespace F_Framework.ViewModels.ProductsViewModels
{
    [QueryProperty("Icon", "bag")]
    public partial class ProductNewViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string iconName;

        private IUnitOfWork _unitOfWork;
        private IToast _messageService;
        private ICustomNavigations _navigations;
        private Result<Product> _result;
        public IRelayCommand ChoosingProductIconCommand { get; set; }

        [ObservableProperty]
        private Product product;

        [ObservableProperty]
        string icon;

        public ProductNewViewModel(IUnitOfWork unitOfWork, ICustomNavigations navigations, IToast messageService)
        {
            Title = $"Create a new Product";
            product = new Product();
            //{ Source = ImageSource.FromFile("fridge.svg") };
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _navigations = navigations;
            ToCreateNewView = new AsyncRelayCommand(CreateProduct);
            ChoosingProductIconCommand = new AsyncRelayCommand(ChoosingIcon);
        }

        private async Task<Result<Product>> ChoosingIcon()
        {
            _result = new Result<Product>();

            await _navigations.GoToIcons();
            product.Icon = $"{iconName}";
            _result.Succeeded = true;
            return _result;
        }

        public async Task<Result<Product>> CreateProduct()
        {
            _result = new Result<Product>();
            if (!string.IsNullOrEmpty(product.Name) && !string.IsNullOrWhiteSpace(product.Type))
            {
                product.Name = product.Name.ToLower();
                product.Type = product.Type.ToLower();
                product.UsedAmount = 1;
                product.DateAdded = DateTime.Now;
                product.Icon = icon;
                product.ExpirationDate = DateTime.Now.AddDays(product.DaysForLife);
                _result.element = product;
                _ = await _unitOfWork.Products.Value.Add(_result.element);
                //Product = null;
                Product = new Product();
                _result.Succeeded = true;
                await _navigations.GoToCustomPage("ProductsAllView");
                await Task.Delay(2200);
                _messageService.MakeToast("Product Created!");
            }
            return _result;
        }
    }
}