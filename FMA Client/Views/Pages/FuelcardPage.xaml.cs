using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Views.Pages
{
    /// <summary>
    /// Interaction logic for FuelcardPage.xaml
    /// </summary>
    public partial class FuelcardPage : Page
    {
        private IFuelcardRepository fcr =
            new FuelcardRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

        public FuelcardPage()
        {
            InitializeComponent();
            SelectedItemContent.IsEnabled = false;
            FuelcardManager f = new FuelcardManager(fcr);
            IReadOnlyList<Fuelcard> fclist = f.GetAllFuelcards();
            FuelcardList.ItemsSource = new ObservableCollection<Fuelcard>(fclist);
        }

        //TODO -> First the search method in the repo
        private void SearchButtonFuelcard_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeTextBoxesEmpty();
            FuelcardList.UnselectAll();
            SelectedItemContent.IsEnabled = false;
            string x = userInputFuelcardPage.Text;
            FuelcardManager f = new FuelcardManager(fcr);
            //Still need to make the Search method for fuelcard
            //IReadOnlyList<Fuelcard> fclist = f.Search(x);
            //FuelcardList.ItemsSource = new ObservableCollection<Fuelcard>(fclist);
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
                        if (fueltype == fuelcardDetails.FueltypeList[fuelcardDetails.FueltypeList.Count - 1])
                        {
                            if (moreThanOne == false)
                            {
                                x += fueltype;
                                moreThanOne = true;
                            } else
                            {
                                x = $", {fueltype}";
                            }
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

        //TODO -> What is the meaning for this?
        private void UserInputFuelcardPage_OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        //Wait on Filter page to be made
        private void FilterButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        //Wait on New page to be made
        private void NieuwButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

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
