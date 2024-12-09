using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace wpfapp.ViewModel
{
    public sealed class ScrollDataGrid : DataGrid
    {
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (this.Items.Count > 0) this.ScrollIntoView(this.Items[this.Items.Count - 1]);
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
           
            if (e.ClickCount ==  1)
            {
                e.Handled = true;
            }
            else
            {
                base.OnPreviewMouseRightButtonDown(e);
            }
        }
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ClickCount == 2)
            {
                OnMouseDoubleClick(e);
            }
            else
            {
                base.OnPreviewMouseLeftButtonDown(e);
            }
        }
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.ChangedButton == MouseButton.Left)
            {
                var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);
                if (row != null && row.Item != null)
                { this.SelectedItem = row.Item; }
                e.Handled = true;
            }
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
            { return parent; }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }
    }
}
