namespace F_Framework.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        public string Title { get; set; }

        public IRelayCommand ToDetailsView { get; set; }
        public IRelayCommand ToCreateNewView { get; set; }
        public IRelayCommand<IEnumerable<object>> AddSelectedProducts { get; set; }
        public IRelayCommand GetAllProductsCommand { get; set; }

        [ObservableProperty]
        private ObservableCollection<Product> selectedProducts;
    }
}