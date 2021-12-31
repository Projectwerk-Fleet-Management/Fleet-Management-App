using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace Views.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateDriverWindow.xaml
    /// </summary>
    public partial class UpdateCarWindow : Window
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static ICarRepository icr = new CarRepository(_connectionString);

        private CarManager cm = new CarManager(icr);
        private Car _car;

        public UpdateCarWindow(Car car)
        {
            InitializeComponent();
            _car = car;

            chassisnummerField.Text = car.Vin;
            nummerplaatField.Text = car.Licenseplate;
            merkField.Text = car.Make;
            modelField.Text = car.Model;
            typeField.Text = car.Type;
            deurenField.Text = string.IsNullOrWhiteSpace(car.Doors) ? "" : car.Doors;
            kleurField.Text = string.IsNullOrWhiteSpace(car.Colour) ? "" : car.Colour;
            foreach (var fuel in car.FuelType)
            {
                if (fuel == Fuel.Benzine) Benzine.IsChecked = true;
                if (fuel == Fuel.Diesel) Diesel.IsChecked = true;
                if (fuel == Fuel.Electrisch) Electrisch.IsChecked = true;

            }
        }

        private void updateCar()
        {
            List<Fuel> fuelList = CreateFueltypeList();
            string vin = chassisnummerField.Text;
            string nummerplaat = nummerplaatField.Text;
            string merk = merkField.Text;
            string model = modelField.Text;
            string wagenType = typeField.Text;
            string deuren = deurenField.Text;
            string kleur = kleurField.Text;

            if (string.IsNullOrWhiteSpace(nummerplaat) || fuelList == null)
            {
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld, updaten is gestopt");
            } else
            {
                Car newCar = new(_car.CarId, merk, model, vin, nummerplaat, wagenType, fuelList, kleur, deuren);
                cm.UpdateCar(_car, newCar);

                MessageBox.Show("Car is geupdate");
                this.Close();

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
            updateCar();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private List<Fuel> CreateFueltypeList()
        {
            List<Fuel> fuelList = new List<Fuel>();
            if (Benzine.IsChecked == true)
            {
                fuelList.Add(Fuel.Benzine);
            }
            if (Diesel.IsChecked == true)
            {
                fuelList.Add(Fuel.Diesel);
            }
            if (Electrisch.IsChecked == true)
            {
                fuelList.Add(Fuel.Electrisch);
            }

            return fuelList;
        }
    }
}
