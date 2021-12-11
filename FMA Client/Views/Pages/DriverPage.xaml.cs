using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using Views.FilterWindows;


namespace Views.Pages
{

    public partial class DriverPage : Page
    {
        private IDriverRepository dr = new DriverRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

        public DriverPage()
        {
            InitializeComponent();
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
            IDriverRepository dr = new DriverRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");
            DriverManager d = new DriverManager(dr);
            var drlist = d.Search(x);
            DriverList.ItemsSource = new ObservableCollection<Driver>(drlist);

        }

        private void DriverList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                    foreach (var license in driverDetails.Licenses)
                    {
                        x = $"{license}, ";
                        if (license == driverDetails.Licenses[driverDetails.Licenses.Count - 1])
                        {
                            x += license;
                        }
                    }

                    rijbewijzenField.Text = x;
                }
                else
                {
                    rijbewijzenField.Text = "Geen rijbewijzen";
                }

                adresField.Text =
                    $"{driverDetails.Address.Street} {driverDetails.Address.Housenumber} {driverDetails.Address.Addendum} {driverDetails.Address.Postalcode} {driverDetails.Address.City}";
                if (driverDetails.AssignedFuelcard != null)
                {
                    tankkaartField.Text = driverDetails.AssignedFuelcard.Cardnumber.ToString();
                }
                else
                {
                    tankkaartField.Text = "Geen tankkaart";
                }

                if (driverDetails.AssignedCar != null)
                {
                    autoField.Text = $"{driverDetails.AssignedCar.Make} {driverDetails.AssignedCar.Model}";
                }
                else
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
            DriverFilter df = new DriverFilter();
            df.Show();
        }
    }
}
