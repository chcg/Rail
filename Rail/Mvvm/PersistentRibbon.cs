using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Rail.Mvvm
{
    public class PersistentRibbon : Ribbon
    {
        public PersistentRibbon()
        { }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            LoadQatItems();
        }

        private void SaveQatItems()
        {
            string text = string.Empty;

            if (this.QuickAccessToolBar != null && this.QuickAccessToolBar.Items != null)
            {
                List<QatItem> qatItems = this.QuickAccessToolBar.Items.Cast<object>().
                    Select(i => i as FrameworkElement).Where(e => e != null).
                    Select(e => RibbonControlService.GetQuickAccessToolBarId(e)).Where(id => id != null).
                    Select(q => new QatItem(q.GetHashCode())).ToList();
                            
                List<QatItem> remainingItems = new List<QatItem>();
                remainingItems.AddRange(qatItems);

                // add -1 to show from application menu
                remainingItems.ForEach(qat => qat.ControlIndices.Add(-1));
                SaveQatItems(remainingItems, this.ApplicationMenu);
                remainingItems.ForEach(qat => qat.ControlIndices.Clear());
                SaveQatItems(remainingItems, this);
                        
                text = qatItems.Aggregate("", (a, b) => a + "," + b.ControlIndices.ToString()).TrimStart(',');
            }
            Properties.Settings.Default.QuickAccessToolBar = text;
            Properties.Settings.Default.Save();
        }

        private void SaveQatItems(List<QatItem> remainingItems, ItemsControl itemsControl)
        {
            if (itemsControl != null && itemsControl.Items != null)
            {
                for (int index = 0; index < itemsControl.Items.Count && remainingItems.Count > 0; index++)
                {
                    SaveQatItemsAmongChildren(remainingItems, itemsControl.Items[index], index);
                }
            }
        }

        private void SaveQatItemsAmongChildren(List<QatItem> remainingItems, object control, int controlIndex)
        {
            if (control != null)
            {
                //Add the index control pending
                remainingItems.ForEach(qat => qat.ControlIndices.Add(controlIndex) );

                SaveQatItemsAmongChildrenInner(remainingItems, control);

                //Remove the index control and earrings that are not within this control
                remainingItems.ForEach(qat => qat.ControlIndices.RemoveAt(qat.ControlIndices.Count - 1));
            }
        }

        private void SaveQatItemsAmongChildrenInner(List<QatItem> remainingItems, object parent)
        {
            SaveQatItemsIfMatchesControl(remainingItems, parent);

            if (remainingItems.Count == 0 || IsLeaf(parent))
            {
                return;
            }

            int childIndex = 0;
            DependencyObject dependencyObject = parent as DependencyObject;
            if (dependencyObject != null)
            {
                IEnumerable children = LogicalTreeHelper.GetChildren(dependencyObject);
                foreach (object child in children)
                {
                    SaveQatItemsAmongChildren(remainingItems, child, childIndex);
                    childIndex++;
                }
            }
            if (childIndex != 0)
            {
                return;
            }

            // if we failed to get any logical children, enumerate the visual ones
            Visual visual = parent as Visual;
            if (visual == null)
            {
                return;
            }
            for (childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(visual); childIndex++)
            {
                Visual child = VisualTreeHelper.GetChild(visual, childIndex) as Visual;
                SaveQatItemsAmongChildren(remainingItems, child, childIndex);
            }

        }

        private bool IsLeaf(object element)
        {
            if ((element is RibbonButton) ||
            (element is RibbonToggleButton) ||
            (element is RibbonRadioButton) ||
            (element is RibbonCheckBox) ||
            (element is RibbonTextBox) ||
            (element is RibbonSeparator))
            {
                return true;
            }

            RibbonMenuItem menuItem = element as RibbonMenuItem;
            if (menuItem != null &&
                menuItem.Items.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool SaveQatItemsIfMatchesControl(List<QatItem> remainingItems, object control)
        {
            bool matched = false;
            FrameworkElement element = control as FrameworkElement;
            if (element != null)
            {
                object getQuickAccessToolBarId = RibbonControlService.GetQuickAccessToolBarId(element);
                if (getQuickAccessToolBarId != null)
                {
                    int remove = remainingItems.RemoveAll(qat => qat.HashCode == getQuickAccessToolBarId.GetHashCode());
                    matched = remove > 0;
                }
            }

            return matched;
        }
        
        private void LoadQatItems()
        {
            if (this.QuickAccessToolBar == null)
            {
                this.QuickAccessToolBar = new RibbonQuickAccessToolBar();
            }

            try
            {
                string text = Properties.Settings.Default.QuickAccessToolBar;
                if (!string.IsNullOrEmpty(text))
                {
                    List<QatItem> qatItems = text.Split(',').Select(i => Int32Collection.Parse(i)).Select(x => new QatItem() { ControlIndices = x }).ToList();
                    if ((qatItems != null) && (qatItems.Count > 0))
                    {
                        SearchInApplicationMenu(qatItems);
                        SearchInTabs(qatItems);

                        qatItems.Where(qat => qat.Owner != null).ToList().ForEach(qat =>
                        {
                            if (RibbonCommands.AddToQuickAccessToolBarCommand.CanExecute(null, qat.Owner))
                            {
                                RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, qat.Owner);
                            }
                        });

                    }
                }
            }
            catch 
            {

            }

            this.QuickAccessToolBar.ItemContainerGenerator.ItemsChanged += OnQuickAccessToolBarItemsChanged;
        }

        protected void OnQuickAccessToolBarItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            SaveQatItems();
        }

        private void SearchInApplicationMenu(List<QatItem> qatItems)
        {
            if (qatItems != null)
            {

                int remainingItemsCount = qatItems.Count(qat => qat.Owner == null);
                List<QatItem> matchedItems = new List<QatItem>();

                for (int index = 0; index < this.ApplicationMenu.Items.Count && remainingItemsCount > 0; index++)
                {
                    matchedItems.Clear();
                    matchedItems.AddRange(qatItems.Where(qat => qat.ControlIndices[0] == -1)); //-1 is applicationMenu

                    //remove -1
                    matchedItems.ForEach(qat => qat.ControlIndices.RemoveAt(0));

                    object item = this.ApplicationMenu.Items[index];
                    if (item != null)
                    {
                        if (!IsLeaf(item))
                        {
                            LoadQatItemsAmongChildren(matchedItems, 0, index, item, ref remainingItemsCount);
                        }
                        else
                        {
                            LoadQatItemIfMatchesControl(matchedItems, new List<QatItem>(), 0, index, item, ref remainingItemsCount);
                        }
                    }
                    //Add -1
                    matchedItems.ForEach(qat => qat.ControlIndices.Insert(0, -1));
                }
            }
        }
        
        private void SearchInTabs(List<QatItem> qatItems)
        {
            int remainingItemsCount = qatItems.Count(qat => qat.Owner == null);
            List<QatItem> matchedItems = new List<QatItem>();

            for (int tabIndex = 0; tabIndex < this.Items.Count && remainingItemsCount > 0; tabIndex++)
            {
                matchedItems.Clear();
                matchedItems.AddRange(qatItems.Where(qat => qat.ControlIndices[0] == tabIndex));

                RibbonTab tab = this.Items[tabIndex] as RibbonTab;
                if (tab != null)
                {
                    LoadQatItemsAmongChildren(matchedItems, 0, tabIndex, tab, ref remainingItemsCount);
                }
            }
        }

        private void LoadQatItemsAmongChildren(List<QatItem> previouslyMatchedItems, int matchLevel, int controlIndex, object parent, ref int remainingItemsCount)
        {
            if (previouslyMatchedItems.Count == 0)
            {
                return;
            }
            if (IsLeaf(parent))
            {
                return;
            }

            int childIndex = 0;
            DependencyObject dependencyObject = parent as DependencyObject;
            if (dependencyObject != null)
            {
                IEnumerable children = LogicalTreeHelper.GetChildren(dependencyObject);
                foreach (object child in children)
                {
                    if (remainingItemsCount == 0)
                    {
                        break;
                    }

                    List<QatItem> matchedItems = new List<QatItem>();
                    LoadQatItemIfMatchesControl(previouslyMatchedItems, matchedItems, matchLevel + 1, childIndex, child, ref remainingItemsCount);
                    LoadQatItemsAmongChildren(matchedItems, matchLevel + 1, childIndex, child, ref remainingItemsCount);
                    childIndex++;
                }
            }
            if (childIndex != 0)
            {
                return;
            }

            // if we failed to get any logical children, enumerate the visual ones
            Visual visual = parent as Visual;
            if (visual == null)
            {
                return;
            }
            for (childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(visual); childIndex++)
            {
                if (remainingItemsCount == 0)
                {
                    break;
                }

                Visual child = VisualTreeHelper.GetChild(visual, childIndex) as Visual;
                List<QatItem> matchedItems = new List<QatItem>();
                LoadQatItemIfMatchesControl(previouslyMatchedItems, matchedItems, matchLevel + 1, childIndex, child, ref remainingItemsCount);
                LoadQatItemsAmongChildren(matchedItems, matchLevel + 1, childIndex, child, ref remainingItemsCount);
            }
        }

        private void LoadQatItemIfMatchesControl(List<QatItem> previouslyMatchedItems, List<QatItem> matchedItems, int matchLevel, int controlIndex, object control, ref int remainingItemsCount)
        {
            for (int qatIndex = 0; qatIndex < previouslyMatchedItems.Count; qatIndex++)
            {
                QatItem qatItem = previouslyMatchedItems[qatIndex];
                if (qatItem.ControlIndices[matchLevel] == controlIndex)
                {
                    if (qatItem.ControlIndices.Count == matchLevel + 1)
                    {
                        qatItem.Owner = control as Control;
                        remainingItemsCount--;
                    }
                    else
                    {
                        matchedItems.Add(qatItem);
                    }
                }
            }
        }
        
        private class QatItem
        {
            public QatItem()
            {
                this.ControlIndices = new Int32Collection();
            }

            public QatItem(int hashCode)
                : this()
            {
                this.HashCode = hashCode;
            }

            public Int32Collection ControlIndices { get; set; }

            public int HashCode { get; set; }

            //Is only for load
            public Control Owner { get; set; }
        }
    }
}
