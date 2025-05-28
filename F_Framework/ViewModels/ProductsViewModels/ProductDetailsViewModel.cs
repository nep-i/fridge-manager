namespace F_Framework.ViewModels.ProductsViewModels
{
    [QueryProperty(nameof(Product), "bag")]
    public partial class ProductDetailsViewModel : BaseViewModel
    {
        private IUnitOfWork _unitOfWork;
        public IRelayCommand Update { get; set; }
        private Result<Product> _result;

        [ObservableProperty]
        private Product product;

        private IToast _messageService;
        private ICustomNavigations _navigations;

        public ProductDetailsViewModel(IUnitOfWork unitOfWork, IToast messageService, ICustomNavigations navigations)
        {
            Title = $"Details";
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _navigations = navigations;
            Update = new AsyncRelayCommand(UpdateProduct);
        }

        public async Task<Result<Product>> UpdateProduct()
        {
            _result = new Result<Product>();
            product.DateAdded = DateTime.Now;
            product.ExpirationDate = DateTime.Now.AddDays(product.DaysForLife);
            _result.element = product;
            _ = await _unitOfWork.Products.Value.Update(_result.element);
            _result.Succeeded = true;
            product = new Product();
            _result.Succeeded = true;
            await _navigations.GoToCustomPage("ProductsAllView");
            await Task.Delay(2200);
            _messageService.MakeToast("Product Updated!");
            return _result;
        }
    }
}