using Microsoft.Maui.Controls;

namespace F_Framework.ViewModels.SettingsViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        private IToast _messageService;
        private ICustomNavigations _navigations;
        private IUnitOfWork _unitOfWork;
        private ISettingsViewVisibility _settingsVisibility;

        public ObservableCollection<string> Icons { get; set; }
        public IAsyncRelayCommand GetSynced { get; set; }
        public IAsyncRelayCommand AllowSyncCommand { get; set; }
        public IAsyncRelayCommand OnAppearingCommand { get; set; }
        public IAsyncRelayCommand LogOutCommand { get; set; }

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool allowSync;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string name;

        public SettingsViewModel(IUnitOfWork unitOfWork, IToast messageService, ICustomNavigations navigations, ISettingsViewVisibility settingsVisibility)
        {
            Title = $"Settings";
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _navigations = navigations;
            _settingsVisibility = settingsVisibility;
            GetSynced = new AsyncRelayCommand(GetAllSync);
            AllowSyncCommand = new AsyncRelayCommand(SettingAllowOnlineSync);
            OnAppearingCommand = new AsyncRelayCommand(RefreshAllowSyncStatus);
            LogOutCommand = new AsyncRelayCommand(LogOut);
            Email = Settings.Email;
            Name = Settings.UserName;
        }

        private async Task LogOut()
        {
            Preferences.Default.Set("AutomaticLogin", false);
            Preferences.Default.Set("IsLoggedin", false);
            Preferences.Default.Set("User", string.Empty);
            Preferences.Default.Set("Email", string.Empty);
            Preferences.Default.Set("Password", string.Empty);
            FirebaseAuthLink aut = Preferences.Default.Get<FirebaseAuthLink>("AuthenticationLink", null);
            if (aut != null)
                await aut.UnlinkFromAsync(FirebaseAuthType.EmailAndPassword);
            Preferences.Default.Set<FirebaseAuthLink>("AuthenticationLink", null);
            _settingsVisibility.VisibilityChanged(false);
            await _navigations.GoToFridge();
            await Task.Delay(2200);
            _messageService.MakeToast("You've been loged out!");

        }

        private Task RefreshAllowSyncStatus()
        {
            AllowSync = Settings.SyncAllow;
            return Task.CompletedTask;
        }

        private async Task GetAllSync()
        {
            IsLoading = true;
            var previousBool = Settings.SyncAllow;
            Settings.SyncAllow = true;

            var productsResult = await _unitOfWork.Products.Value.GetAll();
            var groceriesResult = await _unitOfWork.Groceries.Value.GetAll();
            var gspproductsResult = await _unitOfWork.GroceryShopProducts.Value.GetAll();
            //var items = gspproducts.elements;
            var shopsResult = await _unitOfWork.Shops.Value.GetAll();

            var products = productsResult.elements.ToArray();
            var groceries = groceriesResult.elements.ToArray();
            var gsp = gspproductsResult.elements.ToArray();
            var shops = shopsResult.elements.ToArray();

            if (gsp.Count() > 0)
            {
                for (int i = 0; i < gsp.Length; i++)
                {
                    Result<Shop> shopNew = null;
                    var item = gsp[i];
                    var newGsp = new GroceryShopProduct();
                    var groceryNew = new Result<Grocery>();
                    var productNew = new Result<Product>();
                    newGsp.Id = item.Id;
                    var grocery = groceries.Where(x => x.Id == item.GroceryId).FirstOrDefault();
                    var id = grocery.Id;
                    if (grocery != null)
                        groceryNew = await _unitOfWork.Groceries.Value.AddOrUpdate(grocery);

                    if (grocery.Id != id)
                    {
                        for (int j = 0; j < gsp.Length; j++)
                        {
                            var element = gsp[j];
                            if (element.GroceryId == id)
                            {
                                element.GroceryId = groceryNew.element.Id;
                            }
                        }

                    }

                    var product = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    if (product != null)
                        productNew = await _unitOfWork.Products.Value.AddOrUpdate(product);

                    if (item.ShopId != 0)
                    {
                        var shop = shops.Where(x => x.Id == item.ShopId).FirstOrDefault();
                        shopNew = await _unitOfWork.Shops.Value.AddOrUpdate(shop);
                    }
                    //if (groceryNew.element != null && groceryNew.element.Id != 0)
                    //    newGsp.GroceryId = groceryNew.element.Id;
                    if (shopNew != null && shopNew.element.Id != 0)
                        item.ShopId = shopNew.element.Id;
                    if (productNew != null && productNew.element.Id != 0)
                        item.ProductId = productNew.element.Id;
                    _ = await _unitOfWork.GroceryShopProducts.Value.AddOrUpdate(item);
                }
            }


            if (products.Count() > 0)
            {
                foreach (var item in products)
                {
                    await _unitOfWork.Products.Value.AddOrUpdate(item);
                }
            }
            //if (groceries.elements.Count() > 0)
            //{
            //    foreach (var item in groceries.elements)
            //    {
            //        await _unitOfWork.Groceries.Value.AddOrUpdate(item);
            //    }
            //}
            //try
            //{

            //    if (items != null && items.Count() > 0)
            //    {
            //        foreach (var item in items)
            //        {
            //            _ = await _unitOfWork.GroceryShopProducts.Value.AddOrUpdate(item);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception(ex.Message + "gspproducts");
            //}


            if (shops.Count() > 0)
            {
                foreach (var item in shops)
                {
                    await _unitOfWork.Shops.Value.AddOrUpdate(item);
                }
            }

            Settings.SyncAllow = previousBool;
            IsLoading = false;
            await Task.Delay(2200);
            _messageService.MakeToast("Syncronized!");
        }

        private async Task SettingAllowOnlineSync()
        {
            Settings.SyncAllow = allowSync;
            var jsonfile = Settings.SyncAllow.ToString();
            File.WriteAllText(StaticData.settings, jsonfile);

            var test = File.ReadAllText(StaticData.settings);
        }
    }
}