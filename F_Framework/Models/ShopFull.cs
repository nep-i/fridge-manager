namespace F_Framework.Models
{
    public class ShopFull : Shop
    {
        public int GroceryId { get; set; }

        public List<Product> Products { get; set; }

        public ShopFull()
        {
            Products = new List<Product>();
        }

        public ShopFull(Shop shop) : this()
        {
            Id = shop.Id;
            Key = shop.Key;
            Name = shop.Name;
            UsedAmount = shop.UsedAmount;
            Location = shop.Location;
        }
    }
}