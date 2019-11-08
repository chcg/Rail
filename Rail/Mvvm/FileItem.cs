using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rail.Mvvm
{
    public class FileItem
    {
        public FileItem(string path)
        {
            this.Name = System.IO.Path.GetFileName(path);
            this.Path = System.IO.Path.GetFullPath(path);
            this.Image = ShellIcon.GetLargeIcon(path);          
        }

        public string Name { get; private set; }
        public string Path { get; private set; }
        public ImageSource Image { get; private set; }
    }
}
