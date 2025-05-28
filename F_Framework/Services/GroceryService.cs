namespace F_Framework.Services
{
    public class GroceryService : Repository<Grocery>, IGroceryRepository
    {
        private SQLiteAsyncConnection _db;
        private Result<Grocery> result;

        public GroceryService(SQLiteAsyncConnection db) : base(db)
        {
            _db = db;
        }

        public async Task<Result<Grocery>> AddOrUpdate(Grocery entity)
        {
            await Initialisation();

            result = new Result<Grocery>();
            try
            {
                var checklocal = await _db.Table<Grocery>().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();


                if (checklocal == null)
                {
                    _ = await _db.InsertAsync(entity);
                    result = await GetLastAsync();
                    var grocery = result.elements.FirstOrDefault();
                    result.element = grocery;
                    await Update(result.element);

                }
                else if (checklocal.ToString() == string.Empty)
                {
                    result = await internetDb.Value.Add(entity);
                    result.element.Key = result.Key;
                    _ = await internetDb.Value.Update(entity);
                    _ = await _db.UpdateAsync(result.element);
                }
            }
            catch (Exception)
            {

                throw;
            }
           
            result.Succeeded = true;
            return result;
        }

        public async Task<Result<Grocery>> Add(Grocery entity)
        {
            await Initialisation();

            result = new Result<Grocery>();
            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.Add(entity);
                    if (result.element != null)
                        result.element.Key = result.Key;

                    _ = await _db.InsertAsync(result.element);
                    _ = await internetDb.Value.Update(result.element);
                }
                else
                    _ = await _db.InsertAsync(entity);
                result = await GetLastAsync();
                result.element = result.elements.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
           

            result.Succeeded = true;
            return result;
        }

        public async Task<Result<Grocery>> Update(Grocery entity)
        {
            await Initialisation();

            var result = new Result<Grocery>();
            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.Update(entity);
                }
                result.element.Key = result.Key;
                await _db.UpdateAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
          
            result.Succeeded = true;
            return result;
        }

        public async Task<Result<Grocery>> AddRange(IEnumerable<Grocery> entities)
        {
            await Initialisation();

            result = new Result<Grocery>();
            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.AddRange(entities);
                    if (result.elements != null)
                    {
                        var groceries = result.elements.ToArray();
                        var keys = result.Keys.ToArray();
                        for (int i = 0; i < result.elements.Count(); i++)
                        {
                            groceries[i].Key = keys[i];
                        }
                        _ = await _db.InsertAllAsync(groceries);
                        foreach (var item in groceries)
                        {
                            _ = await internetDb.Value.Update(item);
                        }
                    }
                }
                else
                    _ = await _db.InsertAllAsync(entities);
            }
            catch (Exception)
            {

                throw;
            }
           
            result.Succeeded = true;
            return result;
        }
    }
}