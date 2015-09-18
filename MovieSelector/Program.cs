using System;
using System.IO;

using MovieSelector.View;
using MovieSelector.Controller;

namespace MovieSelector
{
    static class Program
    {
        public const string CONFIG_DRIVE_NAME = "EXTERNAL";
        public const string CONFIG_MOVIES_DIRECTORY = "Movies";
        public const string CONFIG_PATH_TO_VLC = "C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc.exe";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainForm form = new MainForm();
            form.Visible = false;

            FileSystemInfo[] movieFiles = null;

            DriveInfo[] drives = DriveInfo.GetDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                if (drives[i].IsReady && drives[i].VolumeLabel.Equals(CONFIG_DRIVE_NAME))
                {
                    DirectoryInfo[] rootDirFiles = drives[i].RootDirectory.GetDirectories(CONFIG_MOVIES_DIRECTORY);
                    for (int j = 0; j < rootDirFiles.Length; j++)
                    {
                        if(rootDirFiles[j].Name.Equals(CONFIG_MOVIES_DIRECTORY))
                        {
                            movieFiles = rootDirFiles[j].GetFileSystemInfos();
                            break;
                        } 
                    }
                    if (movieFiles != null) break;
                }
            }

            if (movieFiles == null) System.Environment.Exit(-1);

            MovieController controller = new MovieController(form, movieFiles);

            form.ShowDialog();
        }
    }
}
