using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Views.FilterPages;
using Views.NewWindows;

namespace Views.Pages
{
    /// <summary>
    /// Interaction logic for FuelcardPage.xaml
    /// </summary>
    public partial class FuelcardPage : Page
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static IFuelcardRepository fcr = new FuelcardRepository(_connectionString);
        private static FuelcardManager f = new FuelcardManager(fcr);

        public FuelcardPage()
        {
            InitializeComponent();
            SelectedItemContent.IsEnabled = false;
            IReadOnlyList<Fuelcard> fclist = f.GetAllFuelcards();
            FuelcardList.ItemsSource = new ObservableCollection<Fuelcard>(fclist);
        }

        public FuelcardPage(IReadOnlyList<Fuelcard> fclist)
        {
            InitializeComponent();
            SelectedItemContent.IsEnabled = false;
            FuelcardList.ItemsSource = new ObservableCollection<Fuelcard>(fclist);
        }

        //Done
        private void SearchButtonFuelcard_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeTextBoxesEmpty();
            FuelcardList.UnselectAll();
            SelectedItemContent.IsEnabled = false;
            string x = userInputFuelcardPage.Text;
            IReadOnlyList<Fuelcard> fclist;
            if (!string.IsNullOrWhiteSpace(x))
            {
                fclist = f.Search(x);
            }
            else
            {
                fclist = f.GetAllFuelcards();
            }

            FuelcardList.ItemsSource = new ObservableCollection<Fuelcard>(fclist);
        }

        //Done
        private void FuelcardList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemContent.IsEnabled = true;
            var item = FuelcardList.SelectedItem;
            if (item != null)
            {
                var fuelcardDetails = (Fuelcard)item;
                kaartnummerField.Text = fuelcardDetails.Cardnumber;
                vervaldatumField.Text = fuelcardDetails.ExpiryDate.ToShortDateString(); //.ToString();
                actiefField.Text = fuelcardDetails.IsActive.ToString();

                if (fuelcardDetails.FueltypeList.Count != 0)
                {
                    string x = "";
                    bool moreThanOne = false;
                    foreach (var fueltype in fuelcardDetails.FueltypeList)
                    {
                        if (moreThanOne == false)
                        {
                            x += fueltype;
                            moreThanOne = true;
                        } else
                        {
                            x += $", {fueltype}";
                        }
                    }

                    brandstofTypesField.Text = x;
                } else
                {
                    brandstofTypesField.Text = "Geen brandstoffen";
                }

                bestuurderField.Text = fuelcardDetails.Driver != null ? $"{fuelcardDetails.Driver}" : "Geen bestuurder";

                pincodeField.Text = fuelcardDetails.Pincode > 0 ? fuelcardDetails.Pincode.ToString() : "Geen pincode";
            }
        }

        //DONE
        private void UserInputFuelcardPage_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SearchButtonFuelcard_OnMouseLeftButtonUp(sender, null);
            }
        }

        //Wait on Filter page to be made
        private void FilterButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService ns = this.NavigationService;
            FuelcardFilter fcp = new FuelcardFilter();
            ns.Navigate(fcp);
        }

        //Wait on New page to be made
        private void NieuwButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NewFuelcardWindow nfcw = new NewFuelcardWindow();
            nfcw.Show();
        }

        private void MakeTextBoxesEmpty()
        {
            kaartnummerField.Text = null;
            vervaldatumField.Text = null;
            actiefField.Text = null;
            brandstofTypesField.Text = null;
            bestuurderField.Text = null;
            pincodeField.Text = null;
        }
    }
}
