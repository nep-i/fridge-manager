namespace F_Main.Views.FridgeViews;

public partial class FridgeView : ContentPage
{
    public FridgeView(FridgeViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
}