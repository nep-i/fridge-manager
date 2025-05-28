namespace F_Main.Views.ShopViews;

public partial class ShopsNewView : ContentPage
{
    public ShopsNewView(ShopsNewViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}