using System;
using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Views.Pages;

namespace Views.FilterPages
{
    /// <summary>
    /// Interaction logic for DriverFilter.xaml
    /// </summary>
    public partial class CarFilter : Page
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private IReadOnlyList<Car> carList = new List<Car>();
        private static ICarRepository cr = new CarRepository(_connectionString);
        private static CarManager cm = new CarManager(cr);


        public CarFilter()
        {
            InitializeComponent();

        }

        private void returnToCar()
        {
            NavigationService ns = this.NavigationService;
            CarPage cp = new CarPage(carList);
            ns.Navigate(cp);
        }

        private void OpslaanButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Fuel> fuelList;
                if (CreateFueltypeList().Count <= 0)
                {
                    fuelList = null;
                }
                else
                {
                    fuelList = CreateFueltypeList();
                }

                carList = cm.GetCars(null, chassisnummerField.Text, nummerplaatField.Text, merkField.Text,
                    modelField.Text, typeField.Text, fuelList, deurenField.Text, kleurField.Text);
                returnToCar();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Kon de filter niet uitvoerem, {e.Message} - {e.InnerException.Message}");
            }
 
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
