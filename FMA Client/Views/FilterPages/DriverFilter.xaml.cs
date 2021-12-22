using System;
using System.Collections.Generic;
using System.Linq;
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
using Views.Pages;

namespace Views.FilterPages
{
    /// <summary>
    /// Interaction logic for DriverFilter.xaml
    /// </summary>
    public partial class DriverFilter : Page
    {
        private List<Driver> driverList = new List<Driver>();

        public DriverFilter()
        {
            InitializeComponent();
        }

        private void returnToDriver()
        {
            NavigationService ns = this.NavigationService;
            DriverPage dpPage = new DriverPage(driverList);
            ns.Navigate(dpPage);
        }
    }
}
