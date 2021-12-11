using System.Windows;
using System.Windows.Input;


namespace Views.FilterWindows
{
    /// <summary>
    /// Interaction logic for DriverFilter.xaml
    /// </summary>
    public partial class DriverFilter : Window
    {
        public DriverFilter()
        {
            InitializeComponent();
        }

        private void menubardrag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}
