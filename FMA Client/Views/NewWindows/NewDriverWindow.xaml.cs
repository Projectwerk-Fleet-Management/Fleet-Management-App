using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
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
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using BusinessLayer.Model;
using DAL;

namespace Views.NewWindows
{
    /// <summary>
    /// Interaction logic for NewDriverWindow.xaml
    /// </summary>
    public partial class NewDriverWindow : Window
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static IAddressRepository addressRepository = new AddressRepository(_connectionString);
        private static IDriverRepository driverRepository = new DriverRepository(_connectionString);

        private AddressManager am = new AddressManager(addressRepository);
        private DriverManager dm = new DriverManager(driverRepository);
        private IReadOnlyList<Address> a;


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
           CreateDriver();
        }

        private void CreateDriver()
        {
            try
            {
                List <LicenseType> driverslicense  = createDriverLicenseList();
                DateTime dt = geboortedatumField.SelectedDate.Value;
                createDriverAddress();
                Address address = a.First();
                Car car = (Car)carlist.SelectedItem;
                Fuelcard fuelcard = (Fuelcard) tankkaarlist.SelectedItem;

                dm.InsertDriver(voornaamField.Text, achternaamField.Text, dt.ToString("yyyy-MM-dd"), rijksregisternummerField.Text, createDriverLicenseList(), address.AddressId, fuelcard.FuelcardId, car.CarId);
                if (!dm.Exists(null, voornaamField.Text, achternaamField.Text, dt.ToString("yyyy/MM/dd"),
                    rijksregisternummerField.Text, createDriverLicenseList()))
                {
                    throw new UserInterfaceException("Failed to create driver in newdriverwindow");
                }
                else
                {
                    MessageBox.Show("Nieuwe Driver is aangemaakt en toegevoegd");
                    this.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Driver kon niet aangemaakt worden, {e.Message} - {e.InnerException.Message} ");
            }
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
            am.Insert(straatnaamField.Text, housenumberField.Text, addendumField.Text, cityField.Text, int.Parse(postalcodeField.Text));
            if(!am.Exists(null,straatnaamField.Text, housenumberField.Text, addendumField.Text, cityField.Text, int.Parse(postalcodeField.Text)))
            {
                throw new UserInterfaceException("Failed to create address in newdriverwindow");
            }
            else
            {
                a = am.GetAddress(null, straatnaamField.Text, housenumberField.Text, addendumField.Text, cityField.Text,
                    int.Parse(postalcodeField.Text));
            }
        }

        private void NummerplaatZoekBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string x = nummerplaatField.Text;
            CarRepository cr =
                new CarRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");
            CarManager cm = new CarManager(cr);
            var list = cm.GetCars(null, null, x, null, null, null, null, null, null);
            carlist.ItemsSource = new ObservableCollection<Car>(list);
        }

        private void tankkaartZoekBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string x = tankkaartField.Text;
            FuelcardRepository fr =
                new FuelcardRepository(
                    @"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");
            FuelcardManager fm = new FuelcardManager(fr);

            var list = fm.GetFuelcard(x);
            tankkaarlist.ItemsSource = new ObservableCollection<Fuelcard>(list);
        }
    }
}
