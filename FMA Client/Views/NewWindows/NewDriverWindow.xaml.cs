using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BusinessLayer.Model;

namespace Views.NewWindows
{
    /// <summary>
    /// Interaction logic for NewDriverWindow.xaml
    /// </summary>
    public partial class NewDriverWindow : Window
    {
        public NewDriverWindow()
        {
            InitializeComponent();
        }

        private void menubardrag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpslaanButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void createDriverLicenseList()
        {
            List<LicenseType> drlList = new List<LicenseType>();
            if (A1.IsChecked == true)
            {
                drlList.Add(LicenseType.A1);
            }
        }
    }
}
