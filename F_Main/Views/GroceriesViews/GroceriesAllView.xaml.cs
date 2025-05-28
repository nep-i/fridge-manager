namespace F_Main.Views.GroceriesViews;

public partial class GroceriesAllView : ContentPage
{
    public GroceriesAllView(GroceriesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        //GroceryCollection.SelectedItems = null;
        //this.GroceryCollection.ItemsSource = null;
        base.OnAppearing();
    }
}