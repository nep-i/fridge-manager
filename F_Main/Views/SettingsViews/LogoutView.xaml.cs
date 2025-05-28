namespace F_Main.Views.SettingsViews;

public partial class LogoutView : ContentPage
{
	public LogoutView(SettingsViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}