namespace F_Framework.Models
{
    public class Product : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Key { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDone { get; set; }

        public int UsedAmount { get; set; }
        public int AmountToBuy { get; set; }
        public bool InFidge { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        public int DaysForLife { get; set; } = 1;
        public bool ToNotify { get; set; }
        public string Icon { get; set; }

        public override string ToString()
        {
            return $"{Key}";
        }
    }
}