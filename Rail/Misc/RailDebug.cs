using Rail.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Rail.Misc
{
    public static class RailDebug
    {
		[Conditional("DEBUG")]
		public static void DebugList<T>(this IEnumerable<T> railItems, string title) where T : RailBase
		{
			Debug.WriteLine(title);
			Debug.Indent();
			foreach (RailBase railItem in railItems)
			{
				railItem.DebugInfo();
			}
			Debug.Unindent();
		}

		[Conditional("DEBUG")]
		public static void DebugList<T>(this IEnumerable<T> dockPoints) where T : RailDockPoint
		{
			if (dockPoints != null)
			{
				foreach (RailDockPoint dockPoint in dockPoints)
				{
					dockPoint.DebugInfo();
				}
			}
		}
	}
}
