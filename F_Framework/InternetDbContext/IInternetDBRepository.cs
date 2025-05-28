namespace F_Framework.InternetDbContext
{
    public interface IInternetDBRepository<T> where T : class, new()
    {
        public Task<Result<T>> Add(T entity);

        public Task<Result<T>> AddRange(IEnumerable<T> entities);

        public Task<Result<T>> GetAll();

        public Task<Result<T>> GetFirstOrDefautAsync(Expression<Func<T, bool>> filter);

        public Task<Result<T>> Remove(string entity);

        public Task<Result<T>> RemoveRange(IEnumerable<string> entities);

        public Task<Result<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter);

        public Task<Result<T>> Update(T product);
    }
}