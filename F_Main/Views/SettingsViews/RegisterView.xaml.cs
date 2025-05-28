namespace F_Main.Views.SettingsViews;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}