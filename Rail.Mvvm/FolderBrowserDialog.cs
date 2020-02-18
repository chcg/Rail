using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32
{
    public sealed class FolderBrowserDialog : CommonDialog
    {
        internal delegate int BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

        // Fields
        private BrowseCallbackProc callback;
        private string descriptionText;
        private Environment.SpecialFolder rootFolder;
        private string selectedPath;
        private bool selectedPathNeedsCheck;
        private bool showNewFolderButton;
        
        public static readonly int BFFM_SETSELECTION;
        public const int BFFM_INITIALIZED = 1;
        public const int WM_DESTROY = 2;
        
        // Methods
        static FolderBrowserDialog()
        {
            if (Marshal.SystemDefaultCharSize == 1)
            {
                BFFM_SETSELECTION = 0x466;
            }
            else
            {
                BFFM_SETSELECTION = 0x467;

            }
        }

        // Methods
        public FolderBrowserDialog()
        {
            this.Reset();
        }

        private int FolderBrowserDialog_BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
        {
            switch (msg)
            {
            case BFFM_INITIALIZED:
                if (this.selectedPath.Length != 0)
                {
                    NativeMethods.SendMessage(new HandleRef(null, hwnd), BFFM_SETSELECTION, (IntPtr)1, this.selectedPath);
                }
                break;
                
            case WM_DESTROY:
                {
                    IntPtr pidl = lParam;
                    if (pidl != IntPtr.Zero)
                    {
                        IntPtr pszPath = Marshal.AllocHGlobal((int)(260 * Marshal.SystemDefaultCharSize));
                        bool flag = NativeMethods.SHGetPathFromIDList(pidl, pszPath);
                        Marshal.FreeHGlobal(pszPath);
                        NativeMethods.SendMessage(new HandleRef(null, hwnd), 0x465, (IntPtr)0, (IntPtr)(flag ? 1 : 0));
                    }
                    break;
                }
            }
            return 0;
        }

        private static NativeMethods.IMalloc GetSHMalloc()
        {
            NativeMethods.IMalloc[] ppMalloc = new NativeMethods.IMalloc[1];
            NativeMethods.SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        public override void Reset()
        {
            this.rootFolder = Environment.SpecialFolder.Desktop;
            this.descriptionText = string.Empty;
            this.selectedPath = string.Empty;
            this.selectedPathNeedsCheck = false;
            this.showNewFolderButton = true;
        }

        protected override bool RunDialog(IntPtr hWndOwner)
        {
            IntPtr zero = IntPtr.Zero;
            bool flag = false;
            NativeMethods.SHGetSpecialFolderLocation(hWndOwner, (int)this.rootFolder, ref zero);
            if (zero == IntPtr.Zero)
            {
                NativeMethods.SHGetSpecialFolderLocation(hWndOwner, 0, ref zero);
                if (zero == IntPtr.Zero)
                {
                    throw new InvalidOperationException("FolderBrowserDialogNoRootFolder");
                }
            }
            int num = 0x40;
            if (!this.showNewFolderButton)
            {
                num += 0x200;
            }
            
            IntPtr pidl = IntPtr.Zero;
            IntPtr hglobal = IntPtr.Zero;
            IntPtr pszPath = IntPtr.Zero;
            try
            {
                NativeMethods.BROWSEINFO lpbi = new NativeMethods.BROWSEINFO();
                hglobal = Marshal.AllocHGlobal((int)(260 * Marshal.SystemDefaultCharSize));
                pszPath = Marshal.AllocHGlobal((int)(260 * Marshal.SystemDefaultCharSize));
                this.callback = new BrowseCallbackProc(this.FolderBrowserDialog_BrowseCallbackProc);
                lpbi.pidlRoot = zero;
                lpbi.hwndOwner = hWndOwner;
                lpbi.pszDisplayName = hglobal;
                lpbi.lpszTitle = this.descriptionText;
                lpbi.ulFlags = num;
                lpbi.lpfn = this.callback;
                lpbi.lParam = IntPtr.Zero;
                lpbi.iImage = 0;
                pidl = NativeMethods.SHBrowseForFolder(lpbi);
                if (pidl != IntPtr.Zero)
                {
                    NativeMethods.SHGetPathFromIDList(pidl, pszPath);
                    this.selectedPathNeedsCheck = true;
                    this.selectedPath = Marshal.PtrToStringAuto(pszPath);
                    flag = true;
                }
            }
            finally
            {
                NativeMethods.IMalloc sHMalloc = GetSHMalloc();
                sHMalloc.Free(zero);
                if (pidl != IntPtr.Zero)
                {
                    sHMalloc.Free(pidl);
                }
                if (pszPath != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pszPath);
                }
                if (hglobal != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(hglobal);
                }
                this.callback = null;
            }
            return flag;
        }

        // Properties
        public string Description
        {
            get
            {
                return this.descriptionText;
            }
            set
            {
                this.descriptionText = (value == null) ? string.Empty : value;
            }
        }

        public Environment.SpecialFolder RootFolder
        {
            get
            {
                return this.rootFolder;
            }
            set
            {
                if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(Environment.SpecialFolder));
                }
                this.rootFolder = value;
            }
        }

        public string SelectedPath
        {
            get
            {
                if (((this.selectedPath != null) && (this.selectedPath.Length != 0)) && this.selectedPathNeedsCheck)
                {
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.selectedPath).Demand();
                }
                return this.selectedPath;
            }
            set
            {
                this.selectedPath = (value == null) ? string.Empty : value;
                this.selectedPathNeedsCheck = false;
            }
        }

        public bool ShowNewFolderButton
        {
            get
            {
                return this.showNewFolderButton;
            }
            set
            {
                this.showNewFolderButton = value;
            }
        }

        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            internal class BROWSEINFO
            {
                public IntPtr hwndOwner;
                public IntPtr pidlRoot;
                public IntPtr pszDisplayName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpszTitle;
                public int ulFlags;
                public BrowseCallbackProc lpfn;
                public IntPtr lParam;
                public int iImage;
            }

            [ComImport, Guid("00000002-0000-0000-c000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IMalloc
            {
                [PreserveSig]
                IntPtr Alloc(int cb);
                [PreserveSig]
                IntPtr Realloc(IntPtr pv, int cb);
                [PreserveSig]
                void Free(IntPtr pv);
                [PreserveSig]
                int GetSize(IntPtr pv);
                [PreserveSig]
                int DidAlloc(IntPtr pv);
                [PreserveSig]
                void HeapMinimize();
            }

            [DllImport("shell32.dll")]
            internal static extern IntPtr SHBrowseForFolder([In] BROWSEINFO lpbi);

            [DllImport("shell32.dll")]
            internal static extern int SHGetMalloc([Out, MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);

            [DllImport("shell32.dll")]
            internal static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);
            
            [DllImport("shell32.dll")]
            internal static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, string lParam);
            
            [DllImport("user32.dll")]
            internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
                
        }
    }
}
