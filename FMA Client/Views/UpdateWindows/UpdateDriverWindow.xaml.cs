using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Views.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateDriverWindow.xaml
    /// </summary>
    public partial class UpdateDriverWindow : Window
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static IAddressRepository addressRepository = new AddressRepository(_connectionString);
        private static IDriverRepository driverRepository = new DriverRepository(_connectionString);

        private AddressManager am = new AddressManager(addressRepository);
        private DriverManager dm = new DriverManager(driverRepository);
        private Driver _driver;
        public UpdateDriverWindow(Driver driver)
        {
            InitializeComponent();
            _driver = driver;

            voornaamField.Text = driver.FirstName;
            achternaamField.Text = driver.LastName;
            geboortedatumField.Text = driver.DateOfBirth.ToString();
            rijksregisternummerField.Text = driver.NationalIdentificationNumber;
            List <LicenseType> x = driver.Licenses;
            foreach (var license in x)
            {
                switch (license)
                {
                    case LicenseType.AM:
                        AM.IsChecked = true;
                        break;
                    case LicenseType.A:
                        A.IsChecked = true;
                        break;
                    case LicenseType.A1:
                        A1.IsChecked = true;
                        break;
                    case LicenseType.A2:
                        A2.IsChecked = true;
                        break;
                    case LicenseType.B:
                        B.IsChecked = true;
                        break;
                    case LicenseType.BE:
                        BE.IsChecked = true;
                        break;
                    case LicenseType.C:
                        C.IsChecked = true;
                        break;
                    case LicenseType.CE:
                        CE.IsChecked = true;
                        break;
                    case LicenseType.C1:
                        C1.IsChecked = true;
                        break;
                    case LicenseType.C1E:
                        C1E.IsChecked = true;
                        break;
                    case LicenseType.D:
                        D.IsChecked = true;
                        break;
                    case LicenseType.DE:
                        DE.IsChecked = true;
                        break;
                    case LicenseType.D1E:
                        D1E.IsChecked = true;
                        break;
                    case LicenseType.G:
                        G.IsChecked = true;
                        break;
                }
            }

            Address a = driver.Address;
            Car c = driver.AssignedCar;
            Fuelcard f = driver.AssignedFuelcard;

            straatnaamField.Text = a.Street;
            housenumberField.Text = a.Housenumber;
            addendumField.Text = a.Addendum;
            postalcodeField.Text = a.Postalcode.ToString();
            cityField.Text = a.City;


            ObservableCollection<Car> cars = new ObservableCollection<Car>();
            cars.Add(c);
            carlist.ItemsSource = cars;
            carlist.SelectedItem = c;

            ObservableCollection<Fuelcard> fuelcards = new ObservableCollection<Fuelcard>();
            fuelcards.Add(f);
            tankkaarlist.ItemsSource = fuelcards;
            tankkaarlist.SelectedItem = f;
        }


        private void updateDriver()
        {
            try
            {
                List<LicenseType> driverslicense = createDriverLicenseList();
                DateTime dt = geboortedatumField.SelectedDate.Value;
                Car car = (Car)carlist.SelectedItem;
                Fuelcard fuelcard = (Fuelcard)tankkaarlist.SelectedItem;
                if (car.Driver != null) car.RemoveDriver();
                if (fuelcard.Driver != null) fuelcard.RemoveDriver();
                Address a = new Address(_driver.Address.AddressId, straatnaamField.Text, housenumberField.Text,
                    addendumField.Text, cityField.Text, int.Parse(postalcodeField.Text), _driver);
                if (a != _driver.Address)
                {
                    am.Update(_driver.Address, a);
                }

                Driver d = new Driver(_driver.DriverId, voornaamField.Text, achternaamField.Text, a, dt,
                    rijksregisternummerField.Text, createDriverLicenseList(), car, fuelcard);

                dm.UpdateDriver(_driver, d);
                MessageBox.Show("Driver updated");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to update driver {ex.Message} - {ex.InnerException.Message}");
            }

        }

        private void menubardrag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void OpslaanButton_OnClick(object sender, RoutedEventArgs e)
        {
            updateDriver();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NummerplaatZoekBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
            string x = nummerplaatField.Text;
            CarRepository cr =
                new CarRepository(_connectionString);
            CarManager cm = new CarManager(cr);
            var list = cm.GetCars(null, null, x, null, null, null, null, null, null);
            carlist.UnselectAll();
            carlist.ItemsSource = new ObservableCollection<Car>(list);
        }

        private void tankkaartZoekBtn_OnClick(object sender, RoutedEventArgs e)
        {
            tankkaarlist.ItemsSource = null;
            string x = tankkaartField.Text;
            FuelcardRepository fr =
                new FuelcardRepository(
                    _connectionString);
            FuelcardManager fm = new FuelcardManager(fr);

            var list = fm.GetFuelcard(x);
            tankkaarlist.ItemsSource = new ObservableCollection<Fuelcard>(list);
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
