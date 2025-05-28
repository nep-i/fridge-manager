namespace F_Framework.Repositories.Interfaces
{
    public interface IGroceryShopProductRepository : IRepository<GroceryShopProduct>
    {
        Task<Result<GroceryShopProduct>> Update(GroceryShopProduct product);

        Task<Result<GroceryShopProduct>> Add(GroceryShopProduct entity);

        Task<Result<GroceryShopProduct>> AddRange(IEnumerable<GroceryShopProduct> entities);

        Task<Result<GroceryShopProduct>> AddOrUpdate(GroceryShopProduct entity);
    }
}