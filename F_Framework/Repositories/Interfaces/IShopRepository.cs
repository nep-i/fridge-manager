namespace F_Framework.Repositories.Interfaces
{
    public interface IShopRepository : IRepository<Shop>
    {
        Task<Result<Shop>> Update(Shop shop);

        Task<Result<Shop>> Add(Shop entity);

        Task<Result<Shop>> AddRange(IEnumerable<Shop> entities);

        Task<Result<Shop>> AddOrUpdate(Shop entity);
    }
}