namespace F_Framework.Models
{
    public class Shop
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //[Indexed, Unique]
        public string Name { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public int UsedAmount { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"{Key}";
        }
    }
}