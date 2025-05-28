using F_Framework.Helpers;


namespace F_Main.Helpers
{
    public class SettingsVisibilityService : ISettingsViewVisibility
    {
        public void VisibilityChanged(bool onOf)
        {
            var view = Shell.Current.FindByName<FlyoutItem>("SettingsView");
            view.IsVisible = onOf;
            view.IsEnabled = onOf;
            var loginView = Shell.Current.FindByName<FlyoutItem>("LoginView");
            loginView.IsVisible = !onOf;
            loginView.IsEnabled = !onOf;
            
        }
    }
}
