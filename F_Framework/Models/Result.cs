namespace F_Framework.Models
{
    public class Result<T> where T : class, new()
    {
        public Result()
        {
            element = new T();
            elements = new List<T>();
            Keys = new List<string>();
        }

        public T element { get; set; }
        public IEnumerable<T> elements { get; set; }
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public IEnumerable<string> Keys { get; set; } 

        public bool Succeeded { get; set; }
    }
}