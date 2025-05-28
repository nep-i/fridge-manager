using F_Framework.ViewModels.SettingsViewModels;

namespace F_Main.Views.SettingsViews;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}