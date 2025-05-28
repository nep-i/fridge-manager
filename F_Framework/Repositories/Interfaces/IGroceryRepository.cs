namespace F_Framework.Repositories.Interfaces
{
    public interface IGroceryRepository : IRepository<Grocery>
    {
        Task<Result<Grocery>> Update(Grocery product);

        Task<Result<Grocery>> Add(Grocery entity);

        Task<Result<Grocery>> AddRange(IEnumerable<Grocery> entities);

        Task<Result<Grocery>> AddOrUpdate(Grocery entity);
    }
}