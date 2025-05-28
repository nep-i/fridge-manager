//using F_Main.CustomControllers;
using F_Framework.Helpers;
using F_Framework.ViewModels.SettingsViewModels;
using F_Main.Helpers;
using F_Main.Toasts;
using F_Main.Views.SettingsViews;

namespace F_Main
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("fontAwesome.ttf", "FaBrands");
                    fonts.AddFont("frmnr.ttf", "OpenSansRegular");
                    fonts.AddFont("South.ttf", "OpenSansRegular");
                    fonts.AddFont("BDay.ttf", "OpenSansRegular");
                    fonts.AddFont("Apri.ttf", "OpenSansRegular");
                }).UseMauiCommunityToolkit().UseLocalNotification();

            builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IToast, Toaster>();
            builder.Services.AddSingleton<LocalNotificationCenter>();
            builder.Services.AddSingleton<GroceryFullGetter>();
            builder.Services.AddTransient<ICustomNavigations, CustomNavigationSevice>();
            builder.Services.AddTransient<ISettingsViewVisibility, SettingsVisibilityService>();
            builder.Services.AddTransient<IHeaderVisibility, HeaderVisibility>();

            builder.Services.AddTransient<GroceriesDetailsView>();
            builder.Services.AddTransient<GroceriesNewView>();
            builder.Services.AddSingleton<GroceriesAllView>();

            builder.Services.AddTransient<ShopsNewView>();
            builder.Services.AddTransient<ShopsAllView>();

            builder.Services.AddSingleton<ProductsAllView>();
            builder.Services.AddTransient<ProductsDetailsView>();
            builder.Services.AddTransient<ProductsNewView>();

            builder.Services.AddTransient<FridgeView>();
            builder.Services.AddTransient<FridgeViewModel>();
            builder.Services.AddTransient<ChoosingIconView>();
            builder.Services.AddTransient<ChoosingIconViewModel>();

            builder.Services.AddTransient<ProductDetailsViewModel>();
            builder.Services.AddTransient<ProductNewViewModel>();
            builder.Services.AddTransient<ProductsViewModel>();

            builder.Services.AddTransient<GroceriesViewModel>();
            builder.Services.AddTransient<GroceryNewViewModel>();
            builder.Services.AddTransient<GroceryDetailsViewModel>();
            builder.Services.AddTransient<ProductNewViewModel>();

            builder.Services.AddTransient<ShopsDetailsViewModel>();
            builder.Services.AddSingleton<ShopsViewModel>();
            builder.Services.AddTransient<ShopsNewViewModel>();

            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsSynchronisationView>();

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<RegisterView>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LogoutView>();

            return builder.Build();
        }
    }
}