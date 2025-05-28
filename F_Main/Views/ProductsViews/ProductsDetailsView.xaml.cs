namespace F_Main.Views.ProductsViews;

public partial class ProductsDetailsView : ContentPage
{
    public ProductsDetailsView(ProductDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}