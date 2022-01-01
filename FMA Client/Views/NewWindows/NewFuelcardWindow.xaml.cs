using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace Views.NewWindows
{
    /// <summary>
    /// Interaction logic for NewDriverWindow.xaml
    /// </summary>
    public partial class NewFuelcardWindow : Window
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static IDriverRepository driverRepository = new DriverRepository(_connectionString);
        private static IFuelcardRepository fr = new FuelcardRepository(_connectionString);

        private FuelcardManager fcm = new FuelcardManager(fr);
        private DriverManager dm = new DriverManager(driverRepository);



        public NewFuelcardWindow()
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
            try
            {
                List<Fuel> fuelList = CreateFueltypeList();
                string kaartnummer = kaartnummerField.Text;
                if (kaartnummer.Length != 18)
                {
                    MessageBox.Show("De lengte van een kaartnummer moet 18 zijn");
                }
                DateTime vervaldatum = new DateTime(1888, 01, 01);
                if (vervaldatumField.Text != "")
                {
                    vervaldatum = vervaldatumField.SelectedDate.Value;
                }

                bool isActief = true;
                if (Ja.IsChecked == true)
                {
                    isActief = true;
                } else if (Nee.IsChecked == true)
                {
                    isActief = false;
                }

                int? pincode = null;
                if (!string.IsNullOrWhiteSpace(pincodeField.Text))
                {
                    pincode = int.Parse(pincodeField.Text);
                }

                if (string.IsNullOrWhiteSpace(kaartnummer) || (Ja.IsChecked == false && Nee.IsChecked == false) || vervaldatum == new DateTime(1888, 01, 01))
                {
                    MessageBox.Show("Nieuwe tankkaart aanmaken is gestopt, niet alle verplichte velden zijn ingevuld");
                } else
                {
                    var result = MessageBox.Show("Bent u zeker dat u deze tankkaart wilt toevoegen?", "Confirmatie",
                        MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        MessageBox.Show("Toevoegen gestopt");
                    } else
                    {
                        if (fcm.Exists(kaartnummer))
                        {
                            MessageBox.Show("Er bestaat al een tankkaart met deze nummer, gelieve dit te controleren.");
                        } else
                        {
                            fcm.Insert(kaartnummer, vervaldatum, fuelList, pincode, isActief);
                            MessageBox.Show("Toevoegen is succesvol doorgegaan");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"kon geen tankkaart aanmaken {e.Message} - {e.InnerException.Message}");
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
