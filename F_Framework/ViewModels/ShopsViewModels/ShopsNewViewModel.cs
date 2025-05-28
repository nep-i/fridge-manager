namespace F_Framework.ViewModels.ShopsViewModels
{
    public partial class ShopsNewViewModel : BaseViewModel
    {
        private IUnitOfWork _unitOfWork;
        private IToast _messageService;
        private ICustomNavigations _navigations;
        private Result<Shop> _result;

        [ObservableProperty]
        Shop shop;

        public ShopsNewViewModel(IUnitOfWork unitOfWork, ICustomNavigations navigations, IToast messageService)
        {
            Title = $"Create a new Shop";
            //{ Source = ImageSource.FromFile("fridge.svg") };
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _navigations = navigations;
            ToCreateNewView = new AsyncRelayCommand(CreateShop);
            shop = new Shop();
        }

        public async Task<Result<Shop>> CreateShop()
        {
            _result = new Result<Shop>();
            if (!string.IsNullOrEmpty(shop.Name) && !string.IsNullOrWhiteSpace(shop.Name))
            {
                var name = shop.Name.ToLower();
                Shop.Name = name;
                Shop.UsedAmount = 1;
                _ = await _unitOfWork.Shops.Value.Add(shop);
                Shop = new Shop();
                _result.Succeeded = true;
                await _navigations.GoToAllShops();
                await Task.Delay(2200);
                _messageService.MakeToast("Shop Created!");
            }
            else
            {
                _messageService.MakeToast("There is no name given!");
            }
            return _result;
        }
    }
}