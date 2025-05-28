namespace F_Framework.Repositories.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Task<Result<T>> GetAll();

        Task<Result<T>> GetFirstOrDefautAsync(Expression<Func<T, bool>> filter);

        Task<Result<T>> Remove(int entity, string key);

        Task<Result<T>> RemoveRange(IEnumerable<int> entities, IEnumerable<string> keys);

        Task<Result<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter);
    }
}