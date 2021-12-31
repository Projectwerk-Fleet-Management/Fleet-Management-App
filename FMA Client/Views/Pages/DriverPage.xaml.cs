using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Views.FilterPages;
using Views.NewWindows;
using Views.UpdateWindows;


namespace Views.Pages
{

    public partial class DriverPage : Page
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private IDriverRepository dr = new DriverRepository(_connectionString);
        private IAddressRepository AR = new AddressRepository(_connectionString);


        public DriverPage()
        {
            InitializeComponent();
            SelectedItemContent.IsEnabled = false;
            DriverManager d = new DriverManager(dr);
            IReadOnlyList<Driver> drlist = d.GetAllDrivers();
            DriverList.ItemsSource = new ObservableCollection<Driver>(drlist);
        }
        public DriverPage(IReadOnlyList<Driver> resultDrivers)
        {
            InitializeComponent();
            SelectedItemContent.IsEnabled = false;
            DriverList.ItemsSource = new ObservableCollection<Driver>(resultDrivers);
        }

        private void update()
        {
            SelectedItemContent.IsEnabled = false;
            DriverManager d = new DriverManager(dr);
            IReadOnlyList<Driver> drlist = d.GetAllDrivers();
            DriverList.ItemsSource = new ObservableCollection<Driver>(drlist);
        }

        private void SearchButtonDriver_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DriverList.UnselectAll();
            SelectedItemContent.IsEnabled = false;
            string x = userInputDriverPage.Text;
            DriverManager d = new DriverManager(dr);
            IReadOnlyList<Driver> drlist = !string.IsNullOrWhiteSpace(x) ? d.Search(x) : d.GetAllDrivers();
            DriverList.ItemsSource = new ObservableCollection<Driver>(drlist);

        }

        private void DriverList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rijbewijzenField.Text = "";
            SelectedItemContent.IsEnabled = true;
            var item = DriverList.SelectedItem;
            if (item != null)
            {
                Driver driverDetails = (Driver)item;
                voornaamField.Text = driverDetails.FirstName;
                achternaamField.Text = driverDetails.LastName;
                geboortedatumField.Text = driverDetails.DateOfBirth.ToShortDateString(); //.ToString();
                rijksregisternummerField.Text = driverDetails.NationalIdentificationNumber;

                if (driverDetails.Licenses.Count != 0)
                {
                    string x = "";
                    bool moreThanOne = false;
                    foreach (var license in driverDetails.Licenses)
                    {
                        if (moreThanOne == false)
                        {
                            x += license;
                            moreThanOne = true;
                        } else
                        {
                            x += $", {license}";
                        }

                    }

                    rijbewijzenField.Text = x;
                } else
                {
                    rijbewijzenField.Text = "Geen rijbewijzen";
                }

                adresField.Text =
                    $"{driverDetails.Address.Street} {driverDetails.Address.Housenumber} {driverDetails.Address.Addendum} {driverDetails.Address.Postalcode} {driverDetails.Address.City}";
                if (driverDetails.AssignedFuelcard != null)
                {
                    tankkaartField.Text = driverDetails.AssignedFuelcard.Cardnumber.ToString();
                } else
                {
                    tankkaartField.Text = "Geen tankkaart";
                }

                if (driverDetails.AssignedCar != null)
                {
                    autoField.Text = $"{driverDetails.AssignedCar.Make} {driverDetails.AssignedCar.Model}";
                } else
                {
                    autoField.Text = "Geen auto";
                }

            }
        }

        private void UserInputDriverPage_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SearchButtonDriver_OnMouseLeftButtonUp(sender, null);
            }
        }

        private void FilterButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService ns = this.NavigationService;
            DriverFilter dfFilter = new DriverFilter();
            ns.Navigate(dfFilter);
        }

        private void NieuwButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NewDriverWindow ndw = new NewDriverWindow();
            ndw.Show();
        }

        private void VerwijderButton_OnClick(object sender, RoutedEventArgs e)
        {
            DriverManager dm = new DriverManager(dr);
            MessageBoxResult result = MessageBox.Show($"Ben je zeker dat je {DriverList.SelectedItem} wilt verwijderen?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                dm.DeleteDriver((Driver)DriverList.SelectedItem, AR);
                update();
            } else if (result == MessageBoxResult.No)
            {
                MessageBox.Show("Niets is verwijderd");
            }


        }

        private void BewerkButton_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateDriverWindow x = new UpdateDriverWindow((Driver)DriverList.SelectedItem);
            x.Show();
        }
    }
}
