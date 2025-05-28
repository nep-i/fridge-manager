namespace F_Main.Views.ProductsViews;

public partial class ProductsAllView : ContentPage
{
    public ProductsAllView(ProductsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}