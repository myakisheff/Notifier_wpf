using System.Windows;
using System.Windows.Controls;

namespace Notifier.ui
{
    public class LayoutListTasks : ListBox
    {
        public Layout layout
        {
            get { return (Layout)GetValue(layoutProperty); }
            set { SetValue(layoutProperty, value); }
        }

        public static readonly DependencyProperty layoutProperty =
            DependencyProperty.Register("layout", typeof(Layout), typeof(LayoutListTasks), new PropertyMetadata(Layout.Tile));
    }

    public enum Layout
    {
        Tile,
        List
    }
}
