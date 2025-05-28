namespace F_Framework.Helpers
{
    public class GroceryFullGetter
    {
        private IUnitOfWork _service;
        private List<ShopFull> ShopsFull { get; set; }
        private List<GroceryFull> GroceriesFull { get; set; }
        private List<Product> Products { get; set; }
        private List<GroceryShopProduct> Connections { get; set; }
        private GroceryFull Grocery { get; set; }

        public GroceryFullGetter(IUnitOfWork service)
        {
            _service = service;
            ShopsFull = new List<ShopFull>();
            Products = new List<Product>();
            Connections = new List<GroceryShopProduct>();
            GroceriesFull = new List<GroceryFull>();
            Grocery = new GroceryFull();
        }

        public async Task<GroceryFull> GetProductsAndShops(Grocery grocery)
        {
            Grocery = new GroceryFull(grocery);
            await Init();

            Product product;
            if (Connections.Count() != 0)
            {
                foreach (var conn in Connections)
                {
                    product = Products.Where(x => x.Id == conn.ProductId).FirstOrDefault();

                    if (conn.ShopId == 0)
                    {
                        Grocery.Products.Add(product);
                    }
                    else
                    {
                        if (!Grocery.Shops.Any(x => x.Id == conn.ShopId))
                        {
                            ShopsFull.Where(x => x.Id == conn.ShopId).FirstOrDefault().Products.Add(product);
                            Grocery.Shops.Add(ShopsFull.Where(x => x.Id == conn.ShopId).FirstOrDefault());
                        }
                        else
                        {
                            Grocery.Shops.Where(x => x.Id == conn.ShopId).FirstOrDefault().Products.Add(product);
                        }
                    }
                }
                    //Grocery.Shops = ShopsFull;
            }
            return Grocery;
        }

        private async Task Init()
        {
            try
            {
                if (Products.Count() == 0)
                {
                    var resultProducts = new Result<Product>();
                    resultProducts = await _service.Products.Value.GetAll();
                    Products = resultProducts.elements.ToList();
                }

                var resultConnections = new Result<GroceryShopProduct>();
                resultConnections = await _service.GroceryShopProducts.Value
                .GetAllByFilterAsync(x => x.GroceryId == Grocery.Id);

                Connections = resultConnections.elements.ToList();

                if (ShopsFull.Count() == 0)
                {
                    var resultShops = new Result<Shop>();
                    resultShops = await _service.Shops.Value.GetAll();
                    foreach (Shop shop in resultShops.elements)
                    {
                        var shopFull = new ShopFull(shop);
                        ShopsFull.Add(shopFull);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "init", ex.InnerException);
            };

        }

        public async Task<GroceryFull> GetActiveGroceryFull(Grocery grocery)
        {
            
            var fullGroc = await GetProductsAndShops(grocery);
            return fullGroc;
        }

        public async Task<IEnumerable<GroceryFull>> GetAllGroceriesFull()
        {
            var result = new Result<Grocery>();
            result = await _service.Groceries.Value.GetAll();
            foreach (Grocery grocery in result.elements)
            {
                var groceryFull = await GetProductsAndShops(grocery);
                GroceriesFull.Add(groceryFull);
            }
            return GroceriesFull;
        }
    }
}


