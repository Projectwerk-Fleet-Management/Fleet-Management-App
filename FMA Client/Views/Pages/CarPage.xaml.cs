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
using Views.NewWindows;

namespace Views.Pages
{
    /// <summary>
    /// Interaction logic for CarPage.xaml
    /// </summary>
    public partial class CarPage : Page
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static ICarRepository cr = new CarRepository(_connectionString);
        private static IDriverRepository dr = new DriverRepository(_connectionString);

        private static DriverManager d = new DriverManager(dr);
        private static CarManager c = new CarManager(cr);

        public CarPage()
        {
            InitializeComponent();
            LoadPage();
        }

        private void LoadPage()
        {
            CarList.ItemsSource = null;
            SelectedItemContent.IsEnabled = false;
            IReadOnlyList<Car> clist = c.GetAllCars();
            CarList.ItemsSource = new ObservableCollection<Car>(clist);
        }

        private void verwijderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Car selectedCar = (Car)CarList.SelectedItem;
            MessageBoxResult result =
                MessageBox.Show($"Bent u zeker dat u deze auto ({selectedCar}) wilt verwijderen?"
                    , "Confirmatie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (selectedCar.Driver != null)
                {
                    var result2 = MessageBox.Show($"Deze auto heeft een bestuurder " +
                                                  $"({selectedCar.Driver.FirstName} " + $"{selectedCar.Driver.LastName})," +
                                                  $"als u deze auto verwijdert zal deze bestuurder geen auto meer hebben?"
                        , "Confirmatie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result2 == MessageBoxResult.Yes)
                    {
                        Driver oldDriverInfo = selectedCar.Driver;
                        Fuelcard fc = new(oldDriverInfo.AssignedFuelcard.FuelcardId,
                            oldDriverInfo.AssignedFuelcard.Cardnumber, oldDriverInfo.AssignedFuelcard.ExpiryDate);
                        Driver newDriverInfo = new(oldDriverInfo.DriverId, oldDriverInfo.FirstName,
                            oldDriverInfo.LastName, oldDriverInfo.Address, oldDriverInfo.DateOfBirth,
                            oldDriverInfo.NationalIdentificationNumber, oldDriverInfo.Licenses,
                            fc);
                        d.UpdateDriver(oldDriverInfo, newDriverInfo);
                        c.DeleteCar(selectedCar);
                    } else
                    {
                        MessageBox.Show("Verwijderen is gestopt", "Gestopt");
                    }
                } else
                {
                    c.DeleteCar(selectedCar);
                }

                LoadPage();
                MakeTextBoxesEmpty();
            } else
            {
                MessageBox.Show("Verwijderen is gestopt", "Gestopt");
            }
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

        //TODO
        private void FilterButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        //Done
        private void NieuwButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NewCarWindow ncw = new NewCarWindow();
            ncw.Show();
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
