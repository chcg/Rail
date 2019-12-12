using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Rail.Controls
{
    public class MovableDataGrid : ParentDataGrid
    {
        public bool IsEditing { get; set; }

        public MovableDataGrid()
        {
            this.AllowDrop = true;
            this.CanUserSortColumns = false;
        }

        private object targetItem;

        protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        {
            this.IsEditing = true;
            base.OnBeginningEdit(e);
        }

        protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            base.OnCellEditEnding(e);
            this.IsEditing = false;
        }

        public double CellsPanelActualWidth
        {
            get
            {
                Type type = typeof(DataGrid);
                PropertyInfo pInfo = type.GetProperty("CellsPanelActualWidth", BindingFlags.NonPublic | BindingFlags.Instance);
                return (double)pInfo.GetValue(this, null);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.IsEditing)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // don't do if mouse is over scrollbars
                    Point pos = e.GetPosition(this);
                    if (pos.X < CellsPanelActualWidth /*&& pos.Y < CellsPanelActualHeight*/)
                    {
                        //Debug.Print("************** {0} < {1}", pos.X, CellsPanelActualWidth);
                        object selectedItem = this.SelectedItem;

                        // check if item has errors
                        bool hasError = false;
                        IDataErrorInfo dataErrorInfo = selectedItem as IDataErrorInfo;
                        if (dataErrorInfo != null)
                        {
                            foreach (var property in selectedItem.GetType().GetProperties())
                            {
                                if (!string.IsNullOrEmpty(dataErrorInfo[property.Name]))
                                {
                                    hasError = true; ;
                                }
                            }
                        }

                        if (selectedItem != null && selectedItem != CollectionView.NewItemPlaceholder && !this.IsEditing && !hasError)
                        {
                            DataGridRow dataGridRow = (DataGridRow)this.ItemContainerGenerator.ContainerFromItem(selectedItem);
                            if (dataGridRow != null)
                            {
                                DragDropEffects finalDropEffect = DragDrop.DoDragDrop(dataGridRow, selectedItem, DragDropEffects.Move);
                                if ((finalDropEffect == DragDropEffects.Move) && (this.targetItem != null))
                                {
                                    dynamic itemsSource = this.ItemsSource;
                                    int oldIndex = itemsSource.IndexOf((dynamic)selectedItem);
                                    int newIndex = itemsSource.IndexOf((dynamic)this.targetItem);
                                    itemsSource.Move(oldIndex, newIndex);
                                    this.targetItem = null;
                                }
                            }
                        }
                    }
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            DataGridRow dataGridRow = FindVisualParent<DataGridRow>(e.OriginalSource as UIElement);
            if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            DataGridRow dataGridRow = FindVisualParent<DataGridRow>(e.OriginalSource as UIElement);
            if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            DataGridRow dataGridRow = FindVisualParent<DataGridRow>(e.OriginalSource as UIElement);
            if ((dataGridRow == null) || (dataGridRow.DataContext == CollectionView.NewItemPlaceholder))
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            // Verify that this is a valid drop and then store the drop target
            DataGridRow dataGridRow = FindVisualParent<DataGridRow>(e.OriginalSource as UIElement);
            if ((dataGridRow != null) && (dataGridRow.DataContext != CollectionView.NewItemPlaceholder))
            {
                this.targetItem = dataGridRow.DataContext;
                e.Effects = DragDropEffects.Move;
            }
        }

        #region UI Helper

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        #endregion
    }
}
