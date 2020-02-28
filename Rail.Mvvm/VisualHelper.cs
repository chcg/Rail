using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rail.Mvvm
{
    public static class VisualHelper
    {
        //public static Visual FindFirst(Type type)
        //{
        //    Window mainWindow = Application.Current.MainWindow;
        //    return FindFirst(mainWindow, type);
        //}

        //private static Visual FindFirst(Visual parent, Type type)
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        //    {
        //        Visual child = (Visual)VisualTreeHelper.GetChild(parent, i);

        //        if (child.GetType() == type)
        //        {
        //            return child;
        //        }

        //        Visual res = FindFirst(child, type);
        //        if (res != null)
        //        {
        //            return res;
        //        }
        //    }
        //    return null;
        //}

        public static void InvalidateAll()
        {
            Window mainWindow = Application.Current.MainWindow;
            InvalidateAll(mainWindow);
        }

        private static void InvalidateAll(Visual parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(parent, i);
                if (child is UIElement elm)
                {
                    elm.InvalidateVisual();
                }
                InvalidateAll(child);
            }
        }

        public static void InvalidateAll(Type type)
        {
            Window mainWindow = Application.Current.MainWindow;
            InvalidateAll(mainWindow, type);
        }

        private static void InvalidateAll(Visual parent, Type type)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(parent, i);
                if (child is UIElement elm && elm.GetType() == type)                
                {
                    elm.InvalidateVisual();
                }
                InvalidateAll(child);
            }
        }

        public static void InvalidateAll(string name)
        {
            Window mainWindow = Application.Current.MainWindow;
            InvalidateAll(mainWindow, name);
        }

        private static void InvalidateAll(Visual parent, string name)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement elm && elm.Name == name)
                {
                    elm.InvalidateVisual();
                } 
                InvalidateAll(child);
            }
        }
    }
}
