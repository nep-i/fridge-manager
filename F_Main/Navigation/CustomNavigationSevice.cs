using F_Main.Views.SettingsViews;

namespace F_Main.Navigation
{
    public class CustomNavigationSevice : ICustomNavigations
    {
        public Dictionary<string, object> Bag { get; set; }

        public async Task GoToNewProduct()
        {
            await Shell.Current.GoToAsync(nameof(ProductsNewView));
        }

        public async Task GoToGroceryDetails(GroceryFull grocery)
        {
            BagCreate<GroceryFull>(grocery);
            await Shell.Current.GoToAsync($"///{nameof(GroceriesDetailsView)}", Bag);
        }

        public async Task GoToProductDetails(Product product)
        {
            BagCreate<Product>(product);
            await Shell.Current.GoToAsync(nameof(ProductsDetailsView), Bag);
        }

        public async Task GoToShopDetails(Shop shop)
        {
            BagCreate<Shop>(shop);

            await Shell.Current.GoToAsync(nameof(ShopsNewView), Bag);
        }

        public async Task GoToNewShop()
        {
            await Shell.Current.GoToAsync(nameof(ShopsNewView));
        }

        public async Task GoToNewGrocery()
        {
            await Shell.Current.GoToAsync(nameof(GroceriesNewView));
        }

        public async Task GoToNewGrocery(Grocery grocery)
        {
            BagCreate<Grocery>(grocery);
            await Shell.Current.GoToAsync(nameof(GroceriesNewView), Bag);
        }

        public void BagCreate<T>(object obj)
        {
            var item = (T)obj;
            Bag = new Dictionary<string, object>()
                    {
                        {"bag", item }
                    };
        }

        public async Task GoToCustomPage(string page)
        {
            await Shell.Current.GoToAsync($"///{page}");
        }

        public async Task GoToCustomPage<T>(string page, object bag)
        {
            BagCreate<T>(bag);
            await Shell.Current.GoToAsync($"///{page}", Bag);
        }

        public async Task GoToAllShops()
        {
            await Shell.Current.GoToAsync($"///{nameof(ShopsAllView)}");
        }

        public async Task GoToIcons()
        {
            await Shell.Current.GoToAsync($"/{nameof(ChoosingIconView)}");
        }

        public async Task GoToGroceryDetails()
        {
            await Shell.Current.GoToAsync($"///{nameof(GroceriesDetailsView)}");
        }

        public async Task GoToFridge()
        {
            await Shell.Current.GoToAsync($"///{nameof(FridgeView)}");
        }

        public async Task GoToRegistration()
        {
            await Shell.Current.GoToAsync(nameof(RegisterView));
        }

        public async Task GoToProductsAllView()
        {
            await Shell.Current.GoToAsync($"/{nameof(ProductsAllView)}");
        }
    }
}