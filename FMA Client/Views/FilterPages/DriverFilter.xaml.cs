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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using BusinessLayer.Model;
using DAL;
using Views.Pages;

namespace Views.FilterPages
{
    /// <summary>
    /// Interaction logic for DriverFilter.xaml
    /// </summary>
    public partial class DriverFilter : Page
    {
        private IReadOnlyList<Driver> driverList = new List<Driver>();
        private IDriverRepository dr =
            new DriverRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

        
        public DriverFilter()
        {
            InitializeComponent();
            geboortedatumField.SelectedDate = null;
        }

        private void returnToDriver()
        {
            NavigationService ns = this.NavigationService;
            DriverPage dpPage = new DriverPage(driverList);
            ns.Navigate(dpPage);
        }

        private void OpslaanButton_OnClick(object sender, RoutedEventArgs e)
        {
            DriverManager dm = new DriverManager(dr);
            DateTime dt = new DateTime();
            if (geboortedatumField.SelectedDate != null)
            {
                dt = geboortedatumField.SelectedDate.Value;
            }
            driverList = dm.GetDrivers(null, voornaamField.Text, achternaamField.Text, dt.ToString("yyyy-MM-dd"), rijksregisternummerField.Text, createDriverLicenseList());

            returnToDriver();
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
    }
}
