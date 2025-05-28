namespace F_Main.Views.ProductsViews;

public partial class ProductsNewView : ContentPage
{
    public ProductsNewView(ProductNewViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        //TestImage.Source = ImageSource.FromUri(new Uri("/storage/emulated/0/Android/data/com.companyname.f_main/cache/2203693cc04e0be7f4f024d5f9499e13/6622a673619b4a989b4a0db5db0811f3/Cucu.svg", UriKind.Relative));
        //TestImage.Source = ;
    }
}