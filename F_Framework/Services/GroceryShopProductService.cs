namespace F_Framework.Services
{
    public class GroceryShopProductService : Repository<GroceryShopProduct>, IGroceryShopProductRepository
    {
        private SQLiteAsyncConnection _db;
        private Result<GroceryShopProduct> result;

        public GroceryShopProductService(SQLiteAsyncConnection db) : base(db)
        {
            _db = db;
            
        }
        public async Task<Result<GroceryShopProduct>> AddOrUpdate(GroceryShopProduct entity)
        {
            await Initialisation();
            result = new Result<GroceryShopProduct>();
            try
            {
                var checklocal = await _db.Table<GroceryShopProduct>().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();


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
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "addorupdate");
            }

            result.Succeeded = true;
            return result;
        }
        public async Task<Result<GroceryShopProduct>> Add(GroceryShopProduct entity)
        {
            await Initialisation();
            result = new Result<GroceryShopProduct>();
            try
            {
                result = new Result<GroceryShopProduct>();
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

                result.Succeeded = true;
            }
            catch (Exception ex )
            {

                throw new Exception(ex.Message + "add");
            }
           
            return result;
        }
        public async Task<Result<GroceryShopProduct>> Update(GroceryShopProduct entity)
        {
            await Initialisation();
            var result = new Result<GroceryShopProduct>();
            try
            {
                result = new Result<GroceryShopProduct>();
                if (Settings.SyncAllow)
                {
                    result = await internetDb.Value.Update(entity);
                    result.element.Key = result.Key;
                }
                await _db.UpdateAsync(entity);
                result.Succeeded = true;
            }
            catch (Exception)
            {

                throw;
            }
          
            return result;
        }
        public async Task<Result<GroceryShopProduct>> AddRange(IEnumerable<GroceryShopProduct> entities)
        {
            await Initialisation();
            result = new Result<GroceryShopProduct>();
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