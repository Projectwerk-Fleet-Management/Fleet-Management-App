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
    /// Interaction logic for CarPage.xaml
    /// </summary>
    public partial class CarPage : Page
    {
        private ICarRepository cr =
            new CarRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

        public CarPage()
        {
            InitializeComponent();
            SelectedItemContent.IsEnabled = false;
            CarManager c = new CarManager(cr);
            IReadOnlyList<Car> clist = c.GetAllCars();
            CarList.ItemsSource = new ObservableCollection<Car>(clist);
        }

        //TODO -> First the search method in the repo
        private void SearchButtonCar_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeTextBoxesEmpty();
            CarList.UnselectAll();
            SelectedItemContent.IsEnabled = false;
            string x = userInputCarPage.Text;
            CarManager c = new CarManager(cr);
            //Still need to make the Search method for fuelcard
            //IReadOnlyList<Car> clist = c.Search(x);
            //CarList.ItemsSource = new ObservableCollection<Car>(clist);
        }

        //Done
        private void CarList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemContent.IsEnabled = true;
            var item = CarList.SelectedItem;
            if (item != null)
            {
                var carDetails = (Car)item;
                chassisnummerField.Text = carDetails.Vin;
                nummerplaatField.Text = carDetails.Licenseplate;
                merkField.Text = carDetails.Make;
                modelField.Text = carDetails.Model;
                typeWagenField.Text = carDetails.Type;


                string x = "";
                bool moreThanOne = false;
                foreach (var fueltype in carDetails.FuelType)
                {
                    if (fueltype == carDetails.FuelType[carDetails.FuelType.Count - 1])
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

                    brandstofTypesField.Text = x;
                }

                bestuurderField.Text = carDetails.Driver != null ? $"{carDetails.Driver}" : "Geen bestuurder";

                deurenField.Text = carDetails.Doors != null ? carDetails.Doors : "Geen deuren meegegeven";

                kleurField.Text = carDetails.Colour != null ? carDetails.Colour : "Geen deuren meegegeven";
            }
        }

        //TODO -> What is the meaning for this?
        private void UserInputCarPage_OnKeyDown(object sender, KeyEventArgs e)
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
            chassisnummerField.Text = null;
            nummerplaatField.Text = null;
            merkField.Text = null;
            modelField.Text = null;
            typeWagenField.Text = null;
            brandstofTypesField.Text = null;
            bestuurderField.Text = null;
            deurenField.Text = null;
            kleurField.Text = null;
        }
    }
}
