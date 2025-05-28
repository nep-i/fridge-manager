using F_Framework.ViewModels.SettingsViewModels;

namespace F_Main.Views.SettingsViews;

public partial class SettingsSynchronisationView : ContentPage
{
    public SettingsSynchronisationView(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}