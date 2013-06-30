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
using System.Windows.Shapes;

using LoanManagement.Domain;
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfClientSearch.xaml
    /// </summary>
    public partial class wpfClientSearch : Window
    {
        public wpfClientSearch()
        {
            InitializeComponent();
        }

        public string getRow(System.Windows.Controls.DataGrid dg, int row)
        {
            try
            {
                object item = dg.SelectedItem;
                string str = (dg.SelectedCells[row].Column.GetCellContent(item) as TextBlock).Text;
                return str;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                var clt = from cl in ctx.Clients
                          where cl.Active == true
                          select new { ClientID = cl.ClientID, FirstName = cl.FirstName, MiddleName = cl.MiddleName, LastName = cl.LastName, Suffix = cl.Suffix };
                dgClient.ItemsSource = clt.ToList();
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var frm = Application.Current.Windows[0] as wpfSelectClient;
            frm.txtID.Text = getRow(dgClient, 0);
            if (frm.txtID.Text == "")
            {
                return;
            }
            this.Close();
        }
    }
}
