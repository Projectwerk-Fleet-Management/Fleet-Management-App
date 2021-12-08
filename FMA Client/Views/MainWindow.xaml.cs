using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLayer;
using BusinessLayer.Model;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private string _activePage;

        public MainWindow() {
            DataContext = this;
            InitializeComponent();
            SetActive("driverPage");

        }

        private void SetActive(string toset)
        {
            _activePage = toset;
            var converter = new BrushConverter();

            if (_activePage == "driverPage")
            {
                DriverBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#adadad"));
                CarBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbdbdb"));
                FuelcardBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbdbdb"));
                pageName.Text = "Driver";
            }
            else if (_activePage == "carPage")
            {
                DriverBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbdbdb"));
                CarBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#adadad"));
                FuelcardBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbdbdb"));
                pageName.Text = "Car";
            }
            else if (_activePage == "fuelcardPage")
            {
                DriverBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbdbdb"));
                CarBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbdbdb"));
                FuelcardBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#adadad"));
                pageName.Text = "Fuelcard";
            }

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void menubardrag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left) this.DragMove();
        }



        private void SearchButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FilterButton_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            filterscreen popFilterscreen = new filterscreen();
            popFilterscreen.Show();
        }
        private void DriverBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetActive("driverPage");
        }

        private void FuelcardBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetActive("fuelcardPage");
        }

        
        private void CarBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetActive("carPage");
        }
    }
}
