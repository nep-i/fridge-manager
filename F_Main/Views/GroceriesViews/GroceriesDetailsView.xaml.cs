namespace F_Main.Views.GroceriesViews;

public partial class GroceriesDetailsView : ContentPage
{
    public GroceriesDetailsView(GroceryDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}