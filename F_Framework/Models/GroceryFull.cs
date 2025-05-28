namespace F_Framework.Models
{
    public partial class GroceryFull : Grocery
    {
        public List<ShopFull> Shops { get; set; }
        public List<Product> Products { get; set; }
        //private List<Product> _products;
        //private ObservableCollection<Product> products;
        //private Lazy<IUnitOfWork> unitOfWork;

        //public ObservableCollection<Product> Products
        //{
        //    get { return products; }
        //    set
        //    {
        //        Products.CollectionChanged += new NotifyCollectionChangedEventHandler
        //        (CollectionChangedMethod);
        //        Products = value;
        //    }
        //}

        //private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        _products = e.NewItems as List<Product>;

        //        foreach (Product product in _products)
        //        {
        //            var bridge = new GroceryShopProduct()
        //            {
        //                ProductId = product.Id,
        //                GroceryId = Id,
        //            };
        //            _ = unitOfWork.Value.GroceryShopProducts.Value.Add(bridge);

        //        }
        //    }
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        _products = e.NewItems as List<Product>;
        //        if (Id != 0)
        //        {
        //            foreach (Product product in _products)
        //            {
        //                var bridge = unitOfWork.Value.GroceryShopProducts.Value.GetFirstOrDefautAsync(x =>
        //                x.ProductId == product.Id && x.GroceryId == Id
        //                ).Result;
        //                _ = unitOfWork.Value.GroceryShopProducts.Value.Remove(bridge.Id, bridge.Key);
        //            }
        //        }
        //    }
        //}

        public GroceryFull GetGroceryFull()
        {
            var groceryFull = new GroceryFull(this);
            return groceryFull;
        }

        public GroceryFull()
        {
            Shops = new List<ShopFull>();
            Products = new List<Product>();
            //Products = new ObservableCollection<Product>();
            //unitOfWork = new Lazy<IUnitOfWork>();
        }

        public GroceryFull(Grocery grocery) : this()
        {
            Id = grocery.Id;
            Key = grocery.Key;
            UsedAmount = grocery.UsedAmount;
            Note = grocery.Note;
            DateAdded = grocery.DateAdded;
            DateCompleted = grocery.DateCompleted;
            IsDone = grocery.IsDone;
        }
    }
}