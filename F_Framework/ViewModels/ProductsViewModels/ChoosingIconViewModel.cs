namespace F_Framework.ViewModels.ProductsViewModels
{
    public partial class ChoosingIconViewModel : BaseViewModel
    {
        private IToast _messageService;
        private ICustomNavigations _navigations;
        private IUnitOfWork _unitOfWork;
        public ObservableCollection<string> Icons { get; set; }
        public new IAsyncRelayCommand<string> AddSelectedProducts { get; set; }

        [ObservableProperty]
        public string icon;

        //     \berry.svg" />
        //<None Remove = "Resources\Images\Products\cabbage.svg" />

        //   < None Remove="Resources\Images\Products\carrot.svg" />
        //<None Remove = "Resources\Images\Products\corn.svg" />

        //   < None Remove="Resources\Images\Products\dates.svg" />
        //<None Remove = "Resources\Images\Products\garlic.svg" />

        //   < None Remove="Resources\Images\Products\mongosteen.svg" />
        //<None Remove = "Resources\Images\Products\orange.svg" />

        //   < None Remove="Resources\Images\Products\papaya.svg" />
        //<None Remove = "Resources\Images\Products\passiefruit.svg" />

        //   < None Remove="Resources\Images\Products\pepper.svg" />
        //<None Remove = "Resources\Images\Products\potato.svg" />

        //   < None Remove="Resources\Images\Products\pumpkin.svg" />
        //<None Remove = "Resources\Images\Products\strawberry.svg" />

        //   < None Remove="Resources\Images\Products\watermelon.svg" /

        public ChoosingIconViewModel(IUnitOfWork unitOfWork, IToast messageService, ICustomNavigations navigations)
        {
            Title = $"Pick the Icon";
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _navigations = navigations;
            AddSelectedProducts = new AsyncRelayCommand<string>(AddSelected);

            var lst = new List<string>()
            {
                "potato.svg", "pepper.svg", "pumpkin.svg", "watermelon.svg", "papaya.svg", "mongosteen.svg","carrot.svg","cabbage.svg", "corn.svg", "unknown.svg", "dates.svg","passiefruit.svg", "garlic.svg"
            };

            //var d = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var d1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //var d2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
            //var d3 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //var d4 = Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources);
            //var dd = Path.Combine(d, "Images");
            //var lst = Directory.GetFileSystemEntries(d);
            //var lst1 = Directory.GetFileSystemEntries(d1);
            ////var lst2 = Directory.GetFileSystemEntries(d2);
            ////var lst3 = Directory.GetFileSystemEntries(d3);
            //var lst4 = Directory.GetFileSystemEntries(d4);
            //var q = string.Empty;
            Icons = new ObservableCollection<string>(lst);
        }

        public async Task AddSelected(string arg)
        {
            await _navigations.GoToCustomPage<string>("ProductsNewView", arg);
        }
    }
}