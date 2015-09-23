using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MovieSelector.Controller
{
    public interface IMovieView
    {
        void setController(MovieController controller);
        void setViewFilename(string name);

        string Filename { set; }
    }

    public class MovieController
    {
        Random random = new Random();

        IMovieView view;
        List<FileSystemInfo> files;
        FileSystemInfo currentFile;
        string vlcPath;

        public MovieController(IMovieView view, List<FileSystemInfo> files, string vlcPath)
        {
            this.view = view;
            this.files = files;
            this.vlcPath = vlcPath;

            if(files != null)
            {
                currentFile = getNextFile();
                this.view.setViewFilename(currentFile.Name); 
            }

            view.setController(this);
        }

        public string getNextFileName()
        {
            currentFile = getNextFile();
            return currentFile.Name;
        }

        public void tryPlayVideo(bool closeOnComplete)
        {
            if(currentFile.Exists)
            {
                string filenameInsert = string.Format("\"{0}\"", currentFile.FullName);
                System.Diagnostics.Process.Start(vlcPath, filenameInsert);
            
                if (closeOnComplete) System.Environment.Exit(0);
            }
        }

        private FileSystemInfo getNextFile()
        {
            if (files.Count == 0) return new DirectoryInfo("C:\\");
            return files[random.Next(files.Count - 1)];
        }
    }
}
