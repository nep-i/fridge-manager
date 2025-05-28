namespace F_Framework.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Result<Product>> Update(Product product);

        Task<Result<Product>> Add(Product entity);

        Task<Result<Product>> AddRange(IEnumerable<Product> entities);

        Task<Result<Product>> AddOrUpdate(Product entity);
    }
}