namespace F_Framework.ViewModels.ShopsViewModels
{
    public partial class ShopsViewModel : BaseViewModel
    {
        public IRelayCommand DeleteShopsCommand { get; set; }
        public IRelayCommand<IEnumerable<object>> AddSelectedShops { get; set; }
        public IRelayCommand GetAllShopsCommand { get; set; }

        [ObservableProperty]
        ObservableCollection<Shop> allShops;

        [ObservableProperty]
        ObservableCollection<Shop> selectedShops;

        private IUnitOfWork _service;
        private ICustomNavigations _navigations;
        private IToast _messageService;

        private Result<Shop> _result { get; set; }

        public ShopsViewModel(IUnitOfWork service, ICustomNavigations navigations, IToast messageService)
        {
            _service = service;
            _navigations = navigations;
            _result = new Result<Shop>();
            Title = "My Shops";
            AllShops = new ObservableCollection<Shop>();
            SelectedShops = new ObservableCollection<Shop>();
            AddSelectedShops = new AsyncRelayCommand<IEnumerable<object>>(AddSelected);
            GetAllShopsCommand = new AsyncRelayCommand(GetAllShops);
            DeleteShopsCommand = new AsyncRelayCommand(DeleteShops);
            ToDetailsView = new AsyncRelayCommand(GoToDetailsView);
            _messageService = messageService;
        }

        public async Task<Result<Shop>> DeleteShops()
        {
            _result = new Result<Shop>();
            var toGetAllowence = new Result<GroceryShopProduct>();
            var ids = new List<int>();
            var keys = new List<string>();
            foreach (Shop item in SelectedShops)
            {
                ids.Add(item.Id);
                keys.Add(item.Key);
            }
            toGetAllowence = await _service.GroceryShopProducts.Value.GetAllByFilterAsync(x => ids.Contains(x.ProductId));
            if (toGetAllowence.elements == null || toGetAllowence.elements.Count() == 0)
            {
                foreach (Shop item in SelectedShops)
                {
                    ids.Add(item.Id);
                    keys.Add(item.Key);
                }

                _result = await _service.Shops.Value.RemoveRange(ids, keys);
                if (_result.Succeeded)
                {
                    //SelectedShops.Clear();
                    //await _navigations.GoToCustomPage("ShopsAllView");
                    await GetAllShops();
                    await Task.Delay(2200);
                    _messageService.MakeToast("Shops Deleted!");
                }
            }
            else
                _messageService.MakeToast("One of the shops is in use!");
            return _result;
        }

        public async Task GetAllShops()
        {
            _result = await _service.Shops.Value.GetAll();
            if (AllShops != null)
            {

                AllShops = null;
                AllShops = new ObservableCollection<Shop>();
                AllShops.Clear();

                if (_result.elements.Any())
                {
                    var lst = new ObservableCollection<Shop>(_result.elements);
                    foreach (var item in lst)
                        AllShops.Add(item);
                }

            }
            SelectedShops = null;
            SelectedShops = new ObservableCollection<Shop>();
            SelectedShops.Clear();
        }

        //To bring to groceryviewmodel later
        public async Task<Result<Shop>> GoToDetailsView()
        {
            _result = new Result<Shop>();
            if (SelectedShops != null && SelectedShops.Count == 1)
            {
                _result.element = SelectedShops.FirstOrDefault();
                SelectedShops.Clear();

                await _navigations.GoToShopDetails(_result.element);
            }
            _result.Succeeded = true;
            return _result;
        }

        public async Task<Result<Shop>> AddSelected(IEnumerable<object> items)
        {
            var result = new Result<Shop>();
            result.elements = items.Cast<Shop>();
            SelectedShops = new ObservableCollection<Shop>(result.elements);
            result.Succeeded = true;

            return result;
        }
    }
}