namespace F_Framework.Services
{
    public class ShopService : Repository<Shop>, IShopRepository
    {
        private SQLiteAsyncConnection _db;
        private Result<Shop> result;

        public ShopService(SQLiteAsyncConnection db) : base(db)
        {
            _db = db;
        }

        public async Task<Result<Shop>> AddOrUpdate(Shop entity)
        {
            result = new Result<Shop>();
            await Initialisation();
            var checklocal = await _db.Table<Shop>().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();


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

            result.Succeeded = true;
            return result;
        }

        public async Task<Result<Shop>> Add(Shop entity)
        {
            result = new Result<Shop>();
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

        public async Task<Result<Shop>> Update(Shop entity)
        {
            await Initialisation();
            var result = new Result<Shop>();
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

        public async Task<Result<Shop>> AddRange(IEnumerable<Shop> entities)
        {
            await Initialisation();
            result = new Result<Shop>();
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