namespace F_Framework.InternetDbContext
{
    public class InternetDBService<T> : IInternetDBRepository<T> where T : class, new()
    {
        private FirebaseClient firebase = new FirebaseClient(StaticData.url);
        private FirebaseObject<T> response;
        private Result<T> result;

        public void Initialisation()
        {
            result = new Result<T>();
        }

        private bool conn;
        private string name;
        private string user;
        private List<T> lst;
        private List<string> keys;

        public bool CheckingConnectivity()
        {
            var access = Connectivity.Current.NetworkAccess;
            var logedIn = Preferences.Default.Get("IsLoggedin", false);
            if (access == NetworkAccess.Internet && logedIn && logedIn)
            {
                user = Preferences.Default.Get("User", String.Empty);
                return true;
            }
            return false;
        }

        public async Task<Result<T>> Add(T entity)
        {
            conn = CheckingConnectivity();
            Initialisation();
            if (conn == true)
            {
                name = typeof(T).Name;
                response = await firebase.Child(user).Child(name).PostAsync(entity, true);
                if (response != null )
                {
                    result.element = response.Object as T;
                    result.Key = response.Key;
                }

                result.Succeeded = true;
            }
            return result;
        }

        public async Task<Result<T>> AddRange(IEnumerable<T> entities)
        {
            conn = CheckingConnectivity();
            Initialisation();
            if (conn == true)
            {
                name = typeof(T).Name;
                lst = new List<T>();
                keys = new List<string>();
                foreach (T item in entities)
                {
                    response = await firebase.Child(user).Child(name).PostAsync(item, true);
                    var obj = response.Object as T;
                    lst.Add(obj);
                    keys.Add(response.Key);
                }
                result.elements = lst;
                result.Keys = keys;
                result.Succeeded = true;
            }
            return result;
        }

        public async Task<Result<T>> GetAll()
        {
            conn = CheckingConnectivity();
            Initialisation();
            if (conn == true)
            {
                name = typeof(T).Name;
                var response = await firebase.Child(user).Child(name).OnceAsync<T>();
                keys = new List<string>();
                lst = new List<T>();
                if (response != null && response.Count() > 0)
                {
                    foreach (FirebaseObject<T> item in response)
                    {
                        lst.Add(item.Object);
                        keys.Add(item.Key);
                    }
                    result.elements = lst;
                    result.Keys = keys;
                }
                result.Succeeded = true;
            }
            return result;
        }

        public async Task<Result<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
        {
            conn = CheckingConnectivity();
            Initialisation();
            if (conn == true)
            {
                name = typeof(T).Name;
                lst = new List<T>();
                keys = new List<string>();
                var chek = user;
                try
                {
                    var response = await firebase.Child(this.user).Child(name).OnceAsync<T>();
                    if (response != null || response.Count() > 0)
                    {
                        foreach (FirebaseObject<T> item in response)
                        {
                            lst.Add(item.Object);
                            keys.Add(item.Key);
                        }
                        result.elements = lst.AsQueryable().Where(filter).ToList();
                        result.Keys = keys;
                        result.Succeeded = true;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
               
                
            }
            return result;
        }

        public async Task<Result<T>> GetFirstOrDefautAsync(Expression<Func<T, bool>> filter)
        {
            conn = CheckingConnectivity();
            Initialisation();
            if (conn == true)
            {
                var name = typeof(T).Name;
                keys = new List<string>();
                var response = await firebase.Child(user).Child(name).OnceAsync<T>();
                if (response != null || response.Count() > 0)
                {
                    var shop = response.AsQueryable().Select(x => x.Object).Where(filter).FirstOrDefault();
                    result.element = shop;
                }
                
                result.Succeeded = true;
            }
            return result;
        }

        public async Task<Result<T>> Remove(string key)
        {
            conn = CheckingConnectivity();
            Initialisation();
            name = typeof(T).Name;

            if (conn == true && key != "")
            {
                await firebase.Child(user).Child(name).Child(key).DeleteAsync();
            }
            result.Key = key;
            result.Succeeded = true;
            return result;
        }

        public async Task<Result<T>> RemoveRange(IEnumerable<string> entities)
        {
            conn = CheckingConnectivity();
            Initialisation();
            name = typeof(T).Name;

            if (conn == true)
            {
                foreach (string key in entities)
                {
                    await firebase.Child(user).Child(name).Child(key).DeleteAsync();
                }
            }
            result.Succeeded = true;
            return result;
        }

        public async Task<Result<T>> Update(T product)
        {
            conn = CheckingConnectivity();
            Initialisation();
            if (conn == true)
            {
                if (product.ToString() != string.Empty)
                {
                    name = typeof(T).Name;
                    await firebase.Child(user).Child(name).Child(product.ToString()).PatchAsync(product);
                    result.Succeeded = true;
                }
            }
            return result;
        }
    }
}