using Rail.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Rail.ViewModel
{
    public sealed partial class MainViewModel
    {
        private int historyIndex = -1;
        private readonly List<RailPlan> history = new List<RailPlan>();

        protected override void OnUndo()
        {
            if (OnCanUndo())
            {
                this.railPlan = history[--historyIndex];
                Invalidate();
            }
        }

        protected override bool OnCanUndo()
        {
            return historyIndex > 0;
        }

        protected override void OnRedo()
        {
            if (OnCanRedo())
            {
                this.railPlan = history[++historyIndex];
                Invalidate();
            }
        }

        protected override bool OnCanRedo()
        {
            return historyIndex >= 0 && historyIndex < history.Count - 1;
        }

        /// <summary>
        /// call always befor manipulating the RailPlan
        /// </summary>
        [Conditional("USERHISTORY")]
        public void StoreToHistory()
        {
            if (historyIndex >= 0 && historyIndex < history.Count - 1)
            {
                this.history.RemoveRange(historyIndex + 1, history.Count - 1 - historyIndex);
            }
            this.history.Add(this.railPlan.Clone());
            historyIndex = this.history.Count - 1;
        }
    }
}
