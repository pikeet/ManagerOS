using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace OSManager.Style.ListViewCustom
{
    public class ListViewCustom : ListView
    {
        public ListViewCustom()
        {
            this.SelectionChanged += CustomListBox_SelectionChanged;
        }

        private void CustomListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }


        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemsList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(ListViewCustom), new PropertyMetadata(null));


    }
}
