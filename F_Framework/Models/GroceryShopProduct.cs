
using System.Diagnostics.CodeAnalysis;

namespace F_Framework.Models
{
    public class GroceryShopProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Key { get; set; } = string.Empty;

        [ForeignKey(typeof(Grocery))]
        public int GroceryId { get; set; }

        [ForeignKey(typeof(Shop)), AllowNull]
        public int ShopId { get; set; }

        [ForeignKey(typeof(Product))]
        public int ProductId { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"{Key}";
        }
    }
}