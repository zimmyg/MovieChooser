using System;
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
        FileSystemInfo[] files;
        FileSystemInfo currentFile;

        public MovieController(IMovieView view, FileSystemInfo[] movieFiles)
        {
            this.view = view;
            this.files = movieFiles;

            if(this.files.Length > 0)
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
            string filenameInsert = "";

            if(currentFile.Attributes == FileAttributes.Directory)
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = false;
                fd.InitialDirectory = currentFile.FullName;
                DialogResult result = DialogResult.None;

                do
                {
                     result = fd.ShowDialog();                    
                } while(result != DialogResult.OK);

                filenameInsert = "\"" + fd.FileName + "\"";
            }
            else
            {
                filenameInsert = "\"" + currentFile.FullName + "\"";
            }

            System.Diagnostics.Process.Start(Program.CONFIG_PATH_TO_VLC, filenameInsert);
            
            if (closeOnComplete) System.Environment.Exit(0);
        }

        private FileSystemInfo getNextFile()
        {
            return files[random.Next(files.Length)];
        }
    }
}
