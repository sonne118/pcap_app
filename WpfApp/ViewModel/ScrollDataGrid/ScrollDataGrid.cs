﻿using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace MVVM
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
    }
}
