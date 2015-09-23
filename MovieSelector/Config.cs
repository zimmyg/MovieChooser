using System;
using System.IO;

using System.Xml.Linq;

using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;

namespace MovieSelector.Configuration
{
    struct drive_dirs
    {
        public string DriveName;
        public string[] Directories;
    }

    public class Config
    {
        private const string CONFIG_DEFAULT_FILEPATH = "selector_config.xml";
        private const string DEFAULT_VLC_PATH = "C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc";
        private const string DRIVES_ELEMENT_NAME = "Drives";
        private const string VLCPATH_ELEMENT_NAME = "VLCPath";

        public string VLCPath
        {
            get;
            set;
        }

        public List<FileSystemInfo> Directories
        {
            get;
            set;
        }

        private Config() { }

        public static Config Load()
        {
            Config result = new Config();

            if(!File.Exists(CONFIG_DEFAULT_FILEPATH))
            {
                CreateDefaultXMLFile(CONFIG_DEFAULT_FILEPATH);
            }

            XDocument doc = XDocument.Load(CONFIG_DEFAULT_FILEPATH);
            List<drive_dirs> drives = new List<drive_dirs>();
                
            XElement drivesElem = doc.Root.Element(DRIVES_ELEMENT_NAME);
            foreach (XElement drive in drivesElem.Elements())
            {
                List<string> names = new List<string>();
                foreach (XElement dir in drive.Elements())
                {
                    names.Add(dir.Value);
                }

                drive_dirs dirs;
                dirs.DriveName = drive.Attribute("Name").Value;
                dirs.Directories = names.ToArray();

                drives.Add(dirs);
            }

            string vlcPath = doc.Root.Element(VLCPATH_ELEMENT_NAME).Value;

            result.Directories = resolveDirectories(drives);
            result.VLCPath = vlcPath;

            return result;
        }

        private static List<FileSystemInfo> resolveDirectories(List<drive_dirs> dirs)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            List<FileSystemInfo> resolved = new List<FileSystemInfo>();
            foreach (drive_dirs d in dirs)
            {
                foreach (DriveInfo di in drives)
                {
                    if (di.IsReady && di.VolumeLabel.Equals(d.DriveName))
                    {
                        foreach (string dir in d.Directories)
                        {
                            FileSystemInfo[] foundDirs = di.RootDirectory.GetFileSystemInfos(dir);
                            foreach (FileSystemInfo fsi in foundDirs)
                            {
                                resolved.Add(fsi);
                            }
                        }

                        break;
                    }
                }
            }

            return resolved;
        }

        private static void CreateDefaultXMLFile(string filename)
        {
            XElement drives = new XElement(DRIVES_ELEMENT_NAME);
            XElement vlcPath = new XElement(VLCPATH_ELEMENT_NAME, DEFAULT_VLC_PATH);
            XElement rootElement = new XElement("Config", drives, vlcPath);

            XDocument doc = new XDocument(rootElement);
            doc.Save(filename);
        }
    }
}
