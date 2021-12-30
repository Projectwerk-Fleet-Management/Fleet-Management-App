using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Views.NewWindows
{
    /// <summary>
    /// Interaction logic for NewDriverWindow.xaml
    /// </summary>
    public partial class NewCarWindow : Window
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static IDriverRepository driverRepository = new DriverRepository(_connectionString);
        private static ICarRepository carRepository = new CarRepository(_connectionString);

        private DriverManager dm = new DriverManager(driverRepository);
        private CarManager cm = new CarManager(carRepository);


        public NewCarWindow()
        {
            InitializeComponent();
        }

        private void menubardrag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpslaanButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateCar();
        }

        private void CreateCar()
        {
            List<Fuel> fuelList = CreateFueltypeList();
            string vin = chassisnummerField.Text;
            string nummerplaat = nummerplaatField.Text;
            string merk = merkField.Text;
            string model = modelField.Text;
            string wagenType = typeField.Text;
            string deuren = deurenField.Text;
            string kleur = kleurField.Text;

            if (string.IsNullOrWhiteSpace(vin) || string.IsNullOrWhiteSpace(nummerplaat) ||
                string.IsNullOrWhiteSpace(merk)
                || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(wagenType) || fuelList == null)
            {
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld, opslaan is gestopt");
            } else
            {
                cm.InsertCar(vin, nummerplaat, merk, model, wagenType, fuelList, deuren, kleur);
                if (!cm.Exists(null, vin, nummerplaat, merk, model, wagenType, fuelList, deuren, kleur))
                {
                    throw new UserInterfaceException("Failed to create car in newcarwindow");
                } else
                {
                    MessageBox.Show("Nieuwe Car is aangemaakt en toegevoegd");
                    this.Close();
                }
            }


        }

        //Waarschijnlijk ook een methode voor de driver direct te koppelen aan een auto?

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

            if (!fuelList.Any())
            {
                MessageBox.Show("Er moet minstens één soort brandstof aangeduid zijn");
            }

            return fuelList;
        }


        private void driverZoekBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string x = bestuurderField.Text;
            var list = dm.GetDrivers(null, null, null, null, x, null);
            bestuurderlist.ItemsSource = new ObservableCollection<Driver>(list);
        }
    }
}
