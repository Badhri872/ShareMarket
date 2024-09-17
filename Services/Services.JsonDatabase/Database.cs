namespace Services.Database
{
    public class Database
    {
        private static string _dataDirectory;
        public static void SaveData(string fileName, string data)
        {
            if (string.IsNullOrEmpty(_dataDirectory))
            {
                var todayDate = DateTime.Now.ToShortDateString();
                _dataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) 
                                    + @"\ShareMarket\" + todayDate + @"\";
            }
            if (!File.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
            var fileFullPath = _dataDirectory + fileName + ".json";
            File.WriteAllText(fileFullPath, data);
        }

        public static bool Present(string fileName)
        {            
            if (string.IsNullOrEmpty(_dataDirectory))
            {
                var todayDate = DateTime.Now.ToShortDateString();
                _dataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                                + @"\ShareMarket\" + todayDate + @"\";
            }
            return File.Exists(_dataDirectory + fileName + ".json");
        }

        public static string ReadJson(string fileName)
        {
            if (Present(fileName))
            {
                var data = 
                    File.ReadAllText(_dataDirectory + fileName + ".json");
                return data;
            }
            return string.Empty;
        }
    }
}
