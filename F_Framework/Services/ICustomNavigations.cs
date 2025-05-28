namespace F_Framework.Services
{
    public interface ICustomNavigations
    {
        public Dictionary<string, object> Bag { get; set; }

        public Task GoToProductDetails(Product product);

        public Task GoToShopDetails(Shop shop);

        public Task GoToGroceryDetails(GroceryFull grocery);

        public Task GoToGroceryDetails();

        public Task GoToNewProduct();

        public Task GoToNewShop();

        public Task GoToAllShops();

        public Task GoToNewGrocery();

        public Task GoToNewGrocery(Grocery grocery);

        public Task GoToCustomPage(string page);

        public Task GoToCustomPage<T>(string page, object bag);

        public Task GoToIcons();
        public Task GoToFridge();
        public Task GoToRegistration();
        public Task GoToProductsAllView();
    }
}