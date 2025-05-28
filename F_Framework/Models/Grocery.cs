namespace F_Framework.Models
{
    public partial class Grocery : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Key { get; set; } = string.Empty;
        public int UsedAmount { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public DateTime DateCompleted { get; set; }
        public bool IsDone { get; set; } = true;

        public override string ToString()
        {
            return $"{Key}";
        }
    }
}