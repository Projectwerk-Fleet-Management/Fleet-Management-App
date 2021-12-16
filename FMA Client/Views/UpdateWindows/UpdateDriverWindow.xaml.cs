using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Views.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateDriverWindow.xaml
    /// </summary>
    public partial class UpdateDriverWindow : Window
    {
        public UpdateDriverWindow()
        {
            InitializeComponent();
        }

        private void menubardrag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}
