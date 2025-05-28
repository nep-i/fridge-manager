namespace F_Main.Views.ProductsViews;

public partial class ChoosingIconView : ContentPage
{
    public ChoosingIconView(ChoosingIconViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}