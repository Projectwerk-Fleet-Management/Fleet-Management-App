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
using BusinessLayer;
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

        private List<LicenseType> createDriverLicenseList()
        {
            List<LicenseType> drlList = new List<LicenseType>();
            if (AM.IsChecked == true)
            {
                drlList.Add(LicenseType.AM);
            }

            if (A.IsChecked == true)
            {
                drlList.Add(LicenseType.A);
            }

            if (A1.IsChecked == true)
            {
                drlList.Add(LicenseType.A1);
            }

            if (A2.IsChecked == true)
            {
                drlList.Add(LicenseType.A2);
            }

            if (B.IsChecked == true)
            {
                drlList.Add(LicenseType.B);
            }

            if (BE.IsChecked == true)
            {
                drlList.Add(LicenseType.BE);
            }

            if (C.IsChecked == true)
            {
                drlList.Add(LicenseType.C);
            }

            if (CE.IsChecked == true)
            {
                drlList.Add(LicenseType.CE);
            }

            if (C1.IsChecked == true)
            {
                drlList.Add(LicenseType.C1);
            }

            if (C1E.IsChecked == true)
            {
                drlList.Add(LicenseType.C1E);
            }

            if (D.IsChecked == true)
            {
                drlList.Add(LicenseType.D);
            }

            if (DE.IsChecked == true)
            {
                drlList.Add(LicenseType.DE);
            }

            if (D1.IsChecked == true)
            {
                drlList.Add(LicenseType.D1);
            }

            if (D1E.IsChecked == true)
            {
                drlList.Add(LicenseType.D1E);
            }

            if (G.IsChecked == true)
            {
                drlList.Add(LicenseType.G);
            }

            return drlList;

        }

        private void createDriverAddress()
        {
            Address driverAddress = new Address(null, straatnaamField, housenumberField, addendumField, cityField,
                postalcodeField, null);
        }
    }
}
