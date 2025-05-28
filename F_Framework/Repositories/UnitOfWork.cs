namespace F_Framework.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private static SQLiteAsyncConnection _db;

        public UnitOfWork()
        {
            _db = new SQLiteAsyncConnection(StaticData.path, StaticData.Flags);
            Products = new Lazy<IProductRepository>(() => new ProductService(_db));
            Groceries = new Lazy<IGroceryRepository>(() => new GroceryService(_db));
            Shops = new Lazy<IShopRepository>(() => new ShopService(_db));
            GroceryShopProducts = new Lazy<IGroceryShopProductRepository>(() => new GroceryShopProductService(_db));
        }

        public Lazy<IProductRepository> Products { get; private set; }
        public Lazy<IGroceryRepository> Groceries { get; private set; }
        public Lazy<IShopRepository> Shops { get; private set; }
        public Lazy<IGroceryShopProductRepository> GroceryShopProducts { get; private set; }
    }
}