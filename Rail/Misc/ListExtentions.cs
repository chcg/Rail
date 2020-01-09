using System;
using System.Collections.Generic;
using System.Text;

namespace Rail.Misc
{
    public static class ListExtentions
    {
        public static void RemoveLast<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        public static T GetLastOrDefault<T>(this List<T> list) where T : class
        {
            if (list.Count == 0)
            {
                return null;
            }
            return list[list.Count - 1];
        }

        public static T TakeLastOrDefault<T>(this List<T> list) where T : class
        {
            if (list.Count == 0)
            {
                return null;
            }
            T item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }
    }
}
