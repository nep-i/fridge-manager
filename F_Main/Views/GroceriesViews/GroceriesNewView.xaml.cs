namespace F_Main.Views.GroceriesViews;

public partial class GroceriesNewView : ContentPage
{
    public GroceriesNewView(GroceryNewViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //ProductsCollection.ItemsSource = null;
    }
}