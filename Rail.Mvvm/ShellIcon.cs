using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rail.Mvvm
{
    [SuppressUnmanagedCodeSecurity]
    public static class ShellIcon
    {
        static ShellIcon()
        { }

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        private const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        public static ImageSource GetSmallIcon(string path)
        {
            return GetIcon(path, ShellIconSize.Small);
        }

        public static ImageSource GetLargeIcon(string path)
        {
            return GetIcon(path, ShellIconSize.Large);
        }

        private static ImageSource GetIcon(string path, ShellIconSize shellIconSize)
        {
            NativeMethods.SHFILEINFO shinfo = new NativeMethods.SHFILEINFO();
            NativeMethods.SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | (shellIconSize == ShellIconSize.Small ? SHGFI_SMALLICON : SHGFI_LARGEICON));
            IntPtr iconHandle = shinfo.hIcon;
            if (IntPtr.Zero == iconHandle)
            {
                return null;
            }
            ImageSource img = Imaging.CreateBitmapSourceFromHIcon(iconHandle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DestroyIcon(iconHandle);
            return img;
        }

        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct SHFILEINFO
            {
                public IntPtr hIcon;
                public IntPtr iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            };

            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

            [DllImport("User32.dll")]
            internal static extern int DestroyIcon(IntPtr hIcon);
        }
    }
}
