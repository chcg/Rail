using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace Rail.Controls
{
    public class ParentDataGrid : DataGrid
    {
        protected override void OnAddingNewItem(AddingNewItemEventArgs e)
        {
            Type listType = this.ItemsSource.GetType();
            Type itemType = listType.GenericTypeArguments[0];
            object item = itemType.GetConstructor(Type.EmptyTypes).Invoke(null);

            //if (this.DataContext is BindingViewModel)
            //{
            //    PropertyInfo pi = itemType.GetProperty("Parent");
            //    pi?.SetValue(item, this.DataContext);
            //}

            e.NewItem = item;

            base.OnAddingNewItem(e);
        }
    }
}
