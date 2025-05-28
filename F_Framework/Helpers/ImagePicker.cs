namespace F_Framework.Helpers
{
    public static class ImagePicker
    {
        public static async Task<string> Pick()
        {
            var iconName = string.Empty;
            var path = string.Empty;
            var documentsPath = string.Empty;
            //var image = ImageSource.FromUri(new Uri("/storage/emulated/0/Android/data/com.companyname.f_main/cache/2203693cc04e0be7f4f024d5f9499e13/6622a673619b4a989b4a0db5db0811f3/Cucu.svg", UriKind.RelativeOrAbsolute));
            var image = ImageSource.FromFile("fridge.svg");
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an icon for you product!"
            });

            if (result != null)
            {
                if (result.FileName.EndsWith("svg", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    //FileStream fs = new FileStream();
                    FileStream fs = File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                    {
                        byte[] bArray = new byte[stream.Length];
                        using (stream)
                        {
                            stream.Read(bArray, 0, (int)stream.Length);
                        };
                        int length = bArray.Length;
                        fs.Write(bArray, 0, length);
                    };
                    iconName = result.FileName;
                    path = result.FullPath;

                    //var myDocuments = System.Environment.GetFolderPath(
                    //System.Environment.SpecialFolder.MyDocuments);

                    //var myFile = Path.Combine(myDocuments, iconName);
                    //var fileDirectory = Path.GetDirectoryName(myFile);
                    //if (!Directory.Exists(fileDirectory))
                    //    Directory.CreateDirectory(fileDirectory);

                    //using var assetsStream = File.Open(path, FileMode.OpenOrCreate);
                    //using var fileStream = File.OpenWrite(myFile);
                    //assetsStream.CopyTo(fileStream);

                    //documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), iconName);
                    //File.Copy(path, documentsPath, false);
                    //var str = documentsPath;
                    //image = ImageSource.FromStream(() => stream);
                    //SavePicture(stream, iconName);

                    return path;
                }
            }
            return documentsPath;
            ;
        }

        //public static void FileService
        //{
        //    //public static void mmm(string url)
        //    {
        //        // Creates an HttpWebRequest with the specified URL.
        //        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //    // Sends the HttpWebRequest and waits for the response.
        //    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
        //    // Gets the stream associated with the response.
        //    Stream receiveStream = myHttpWebResponse.GetResponseStream();
        //    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
        //    // Pipes the stream to a higher level stream reader with the required encoding format.
        //    StreamReader readStream = new StreamReader(receiveStream, encode);
        //    Console.WriteLine("\r\nResponse stream received.");
        //        Char[] read = new Char[256];
        //    // Reads 256 characters at a time.
        //    int count = readStream.Read(read, 0, 256);
        //    Console.WriteLine("HTML...\r\n");
        //        while (count > 0)
        //        {
        //            // Dumps the 256 characters on a string and displays the string to the console.
        //            String str = new String(read, 0, count);
        //    Console.Write(str);
        //            count = readStream.Read(read, 0, 256);
        //        }
        //Console.WriteLine("");
        //        // Releases the resources of the response.
        //        myHttpWebResponse.Close();
        //        // Releases the resources of the Stream.
        //        readStream.Close();
        //    }
        public static void SavePicture(Stream data, string name)
        {
            var documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name);
            //documentsPath = Path.Combine(documentsPath, "Products");
            if (!File.Exists(documentsPath))
                Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath);

            byte[] bArray = new byte[data.Length];
            // string testPath = Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath;

            using (FileStream fs = File.Create(filePath))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }
    }
}