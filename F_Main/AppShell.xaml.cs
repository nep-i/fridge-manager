using F_Framework;
using F_Framework.Helpers;
using F_Main.Helpers;
using F_Main.Toasts;

namespace F_Main
{
    public partial class AppShell : Shell
    {
       
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GroceriesDetailsView), typeof(GroceriesDetailsView));
            Routing.RegisterRoute(nameof(GroceriesNewView), typeof(GroceriesNewView));

            Routing.RegisterRoute(nameof(ProductsDetailsView), typeof(ProductsDetailsView));
            Routing.RegisterRoute(nameof(ProductsNewView), typeof(ProductsNewView));

            //Routing.RegisterRoute(nameof(ShopsDetailsView), typeof(ShopsDetailsView));
            Routing.RegisterRoute(nameof(ShopsNewView), typeof(ShopsNewView));

            //Routing.RegisterRoute(nameof(FridgeView), typeof(FridgeView));

            //Routing.RegisterRoute(nameof(ProductsAllView), typeof(ProductsAllView));
            Routing.RegisterRoute(nameof(ShopsNewView), typeof(ShopsNewView));
            Routing.RegisterRoute(nameof(ChoosingIconView), typeof(ChoosingIconView));

            //Routing.RegisterRoute(nameof(ShopsAllView), typeof(ShopsAllView));
            Routing.RegisterRoute(nameof(ShopsNewView), typeof(ShopsNewView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
            

            Routing.RegisterRoute(nameof(GroceriesAllView), typeof(GroceriesAllView));
            Routing.RegisterRoute(nameof(GroceriesNewView), typeof(GroceriesNewView));
            Routing.RegisterRoute(nameof(GroceriesDetailsView), typeof(GroceriesDetailsView));
            //Login = new AsyncRelayCommand(GoToLoginView);
            //Routing.RegisterRoute(nameof(ProductsAllView), typeof(ProductsAllView));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FileChk();
            var toaster = new Toaster();
            var visibilityService = new SettingsVisibilityService();
            var loginSerive = new Login(toaster, visibilityService);
            _ = loginSerive.LoginAsync();
        }

        private void FileChk()
        {
            if (File.Exists(StaticData.settings))
            {
                var text = File.ReadAllText(StaticData.settings);
                if (text == "")
                {
                    text = Settings.SyncAllow.ToString();
                    File.WriteAllText(StaticData.settings, text);
                }
                Settings.SyncAllow = Convert.ToBoolean(text);
            }
            else
            {
                var create = File.Create(StaticData.settings);
                create.Close();
                var jsonfile = Settings.SyncAllow.ToString();
                File.WriteAllText(StaticData.settings, jsonfile);
            }
        }
    }
}