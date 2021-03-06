using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;

namespace Views.UpdateWindows
{
    /// <summary>
    /// Interaction logic for UpdateDriverWindow.xaml
    /// </summary>
    public partial class UpdateFuelcardWindow : Window
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static IFuelcardRepository ifc = new FuelcardRepository(_connectionString);

        private FuelcardManager fcm = new FuelcardManager(ifc);
        private Fuelcard _fuelcard;

        public UpdateFuelcardWindow(Fuelcard fuelcard)
        {
            InitializeComponent();
            _fuelcard = fuelcard;

            kaartnummerField.Text = fuelcard.Cardnumber;
            vervaldatumField.SelectedDate = fuelcard.ExpiryDate;
            pincodeField.Text = fuelcard.Pincode == 0 ? "" : fuelcard.Pincode.ToString();
            if (fuelcard.IsActive)
            {
                Ja.IsChecked = true;
            } else
            {
                Nee.IsChecked = true;
            }

            foreach (var fuel in fuelcard.FueltypeList)
            {
                if (fuel == Fuel.Benzine) Benzine.IsChecked = true;
                if (fuel == Fuel.Diesel) Diesel.IsChecked = true;
                if (fuel == Fuel.Electrisch) Electrisch.IsChecked = true;

            }
        }

        private void updateFuelcard()
        {
            try
            {
                int? pincode = null;
                bool nietCorrect;
                if (!string.IsNullOrWhiteSpace(pincodeField.Text)) pincode = TryParseNullable(pincodeField.Text);
                Fuelcard newFuelcard = new(_fuelcard.FuelcardId, kaartnummerField.Text, vervaldatumField.SelectedDate.Value, pincode,
                    CreateFueltypeList(), GetActief());

                fcm.UpdateFuelcard(_fuelcard, newFuelcard);
                MessageBox.Show("Fuelcard updated");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"failed to update fuelcard, {ex.Message} - {ex.InnerException.Message}");
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
            updateFuelcard();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool GetActief()
        {
            return Ja.IsChecked == true;
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

        public int? TryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }
    }
}
