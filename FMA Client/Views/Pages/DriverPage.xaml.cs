using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using BusinessLayer.Interfaces;
using BusinessLayer.Managers;
using DAL;
using Views.PageData;


namespace Views.Pages
{
    /// <summary>
    /// Interaction logic for DriverPage.xaml
    /// </summary>
    public partial class DriverPage : Page
    {
        private DriverPageData data = new DriverPageData();

        public DriverPage()
        {
            InitializeComponent();

            IDriverRepository dr = new DriverRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");
           DriverManager d = new DriverManager(dr);

           IReadOnlyList<Driver> drlist = d.GetAllDrivers();
           data.setList(drlist);
           List<string> listfilling = data.getFill();

           foreach (var content in listfilling)
           {

               DriverList.Items.Add(content);
           }
        }

        private void SearchButtonDriver_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DriverList.Items.Clear();
            string x = userInputDriverPage.Text;
            IDriverRepository dr = new DriverRepository(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");
            DriverManager d = new DriverManager(dr);
            var drlist = d.Search(x);
            data.setList(drlist);
            List<string> listfilling = data.getFill();

            foreach (var content in listfilling)
            {

                DriverList.Items.Add(content);
            }

        }

        private void DriverList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DriverList.Items.Count > 0)
            {
                var item = DriverList.SelectedItem;
                Driver x = data.getDriverDetails(item.ToString());

                voornaam.Content = x.FirstName;
                achternaam.Content = x.LastName;
                RRN.Content = x.NationalIdentificationNumber;
            }
            
        }
    }
}
