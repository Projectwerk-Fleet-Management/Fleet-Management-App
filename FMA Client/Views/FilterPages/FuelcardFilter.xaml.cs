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
    public partial class FuelcardFilter : Page
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private IReadOnlyList<Fuelcard> fuelcardList = new List<Fuelcard>();
        private static IFuelcardRepository fcr = new FuelcardRepository(_connectionString);
        private static FuelcardManager fcm = new FuelcardManager(fcr);


        public FuelcardFilter()
        {
            InitializeComponent();

        }

        private void ReturnToFuelcard()
        {
            NavigationService ns = this.NavigationService;
            FuelcardPage fcp = new FuelcardPage(fuelcardList);
            ns.Navigate(fcp);
        }

        private void OpslaanButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<Fuel> fuelList;
            if (CreateFueltypeList().Count <= 0)
            {
                fuelList = null;
            } else
            {
                fuelList = CreateFueltypeList();
            }

            bool? isActief = null;
            if (ja.IsChecked == true)
            {
                isActief = true;
                nee.IsChecked = false;
                beide.IsChecked = false;
            } else if (nee.IsChecked == true)
            {
                isActief = false;
                nee.IsChecked = true;
                beide.IsChecked = false;
            } else if (beide.IsChecked == true)
            {
                isActief = null;
                ja.IsChecked = false;
                nee.IsChecked = false;
            }

            fuelcardList = fcm.Filter(KaartnummerField.Text, fuelList, isActief);
            ReturnToFuelcard();
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
