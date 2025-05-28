namespace F_Framework
{
    //Static Data for usage globally
    public static class StaticData
    {
        public static List<string> ProductNames { get; set; }

        public static string url = "https://fridge-manager.firebasedatabase.app/";
        public static string secret = "secret";
        public const string DatabaseFilename = "FridgeManager1.db4";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        public static string path =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public static string settings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "settings.txt");
    }
}