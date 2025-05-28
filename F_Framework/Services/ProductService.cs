namespace F_Framework.Services
{
    public class ProductService : Repository<Product>, IProductRepository
    {
        private SQLiteAsyncConnection _db;
        private Result<Product> result;

        public ProductService(SQLiteAsyncConnection db) : base(db)
        {
            _db = db;
        }

        public async Task<Result<Product>> AddOrUpdate(Product entity)
        {
            await Initialisation();
            result = new Result<Product>();
            try
            {
                var checklocal = await _db.Table<Product>().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();


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

        public async Task<Result<Product>> Add(Product entity)
        {
            await Initialisation();
            result = new Result<Product>();
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message + " " + "productAdd");
            }
          

            result.Succeeded = true;
            return result;
        }

        public async Task<Result<Product>> Update(Product entity)
        {
            await Initialisation();
            var result = new Result<Product>();
            try
            {
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.Update(entity);
                    result.element.Key = result.Key;
                }
                if (result.element != null)
                    await _db.UpdateAsync(entity);
                else
                    _ = _db.UpdateAsync(result.element);
            }
            catch (Exception)
            {

                throw;
            }


            result.Succeeded = true;
            return result;
        }

        public async Task<Result<Product>> AddRange(IEnumerable<Product> entities)
        {
            result = new Result<Product>();
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