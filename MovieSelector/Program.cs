using System;
using System.IO;
using System.Collections.Generic;

using MovieSelector.View;
using MovieSelector.Controller;
using MovieSelector.Configuration;

namespace MovieSelector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Config cfg = Config.Load();
            List<FileSystemInfo> files = new List<FileSystemInfo>();
            
            MainForm form = new MainForm();
            form.Visible = false;

            foreach (FileSystemInfo fsi in cfg.Directories)
            {
                if(fsi.Exists)
                {
                    FileSystemInfo[] dirFiles = ((DirectoryInfo)fsi).GetFileSystemInfos();
                    foreach(FileSystemInfo f in dirFiles)
                    {
                        files.Add(f);
                    }
                }
            }

            MovieController controller = new MovieController(form, files, cfg.VLCPath);

            form.ShowDialog();
        }
    }
}
