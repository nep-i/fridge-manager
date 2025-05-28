namespace F_Framework.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private SQLiteAsyncConnection db;
        public Lazy<IInternetDBRepository<T>> internetDb;
        public Result<T> result;

        public async Task Initialisation()
        {
            //await db.DeleteAllAsync<T>();
            var name = typeof(T).Name;
            result = new Result<T>();
            _ = await db.CreateTableAsync<T>();
        }

        public Repository(SQLiteAsyncConnection _db)
        {
            internetDb = new Lazy<IInternetDBRepository<T>>(() => new InternetDBService<T>());
            this.db = _db;
        }

        public async Task<Result<T>> GetAll()
        {
            await Initialisation();
            var dbLocal = new List<T>();
            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.GetAll();
                    dbLocal = await db.Table<T>().ToListAsync();
                }
                //if ((Settings.SyncAllow || result.elements.Count() < dbLocal.Count() && (result.elements == null || result.elements.Count() == 0)) || Settings.SyncAllow == false)
                if ((Settings.SyncAllow && result.elements.Count() < dbLocal.Count() || (result.elements == null || result.elements.Count() == 0))
                    || Settings.SyncAllow == false)
                {
                    result.elements = await db.Table<T>().ToListAsync();
                    result.Succeeded = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "getall", ex.InnerException);
            }



            return result;
        }

        public async Task<Result<T>> GetFirstOrDefautAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                await Initialisation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.GetFirstOrDefautAsync(filter);
                }
                if ((Settings.SyncAllow && (result.element == null || result.element.ToString() == "")) || Settings.SyncAllow == false)
                {
                    result.element = await db.Table<T>().Where(filter).FirstOrDefaultAsync();
                    result.Succeeded = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "getfirst", ex.InnerException);
            }



            return result;
        }

        public async Task<Result<T>> GetLastAsync()
        {
            await Initialisation();

            var tableName = typeof(T).Name;
            result.elements = await db.QueryAsync<T>(query: $"SELECT * from {tableName} order by ROWID DESC limit 1", "Id");
            result.Succeeded = true;
            return result;
        }

        public async Task<Result<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
        {
            await Initialisation();

            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.GetAllByFilterAsync(filter);
                }

                if ((Settings.SyncAllow && (result.elements == null || result.elements.Count() == 0)) || Settings.SyncAllow == false)
                {
                    result.elements = await db.Table<T>().Where(filter).ToListAsync();
                    result.Succeeded = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "getall", ex.InnerException);
            }


            return result;
        }

        public async Task<Result<T>> Remove(int entity, string key)
        {

            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.Remove(key);
                }
                await Initialisation();
                _ = await db.DeleteAsync<T>(entity);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "remove", ex.InnerException);
            }

            return result;
        }

        public async Task<Result<T>> RemoveRange(IEnumerable<int> entities, IEnumerable<string> keys)
        {

            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.RemoveRange(keys);
                }
                await Initialisation();
                foreach (int entity in entities)
                    _ = await db.DeleteAsync<T>(entity);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "removerage", ex.InnerException);
            }


            result.Succeeded = true;

            return result;
        }
    }
}