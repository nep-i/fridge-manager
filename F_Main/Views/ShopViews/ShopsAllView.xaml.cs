namespace F_Main.Views.ShopViews;

public partial class ShopsAllView : ContentPage
{
    public ShopsAllView(ShopsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        //this.FridgeCollection.SelectedItems = null;
        base.OnAppearing();
    }
}