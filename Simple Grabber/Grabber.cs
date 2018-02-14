using System.Configuration;
using System.IO;

namespace Simple_Grabber
{
    public class Grabber
    {
        // Initialize the directory if it doesn't exist

        private static void InitializeDirectory()
        {
            if (!Directory.Exists(ConfigurationManager.AppSettings["path"]))
            {
               Directory.CreateDirectory(ConfigurationManager.AppSettings["path"]);
            }
        }

        public static void Main()
        {
            InitializeDirectory();

            Modules.Chrome.WriteToFile(ConfigurationManager.AppSettings["path"]);
        }
    }
}
