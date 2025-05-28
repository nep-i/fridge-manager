namespace F_Framework.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Lazy<IProductRepository> Products { get; }
        Lazy<IGroceryRepository> Groceries { get; }
        Lazy<IShopRepository> Shops { get; }
        Lazy<IGroceryShopProductRepository> GroceryShopProducts { get; }
    }
}