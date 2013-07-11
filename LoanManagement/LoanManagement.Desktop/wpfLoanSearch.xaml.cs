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

using MahApps.Metro.Controls;
using LoanManagement.Domain;
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanSearch.xaml
    /// </summary>
    public partial class wpfLoanSearch : MetroWindow
    {
        public wpfLoanSearch()
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

        public void rg()
        {
            using (var ctx = new SystemContext())
            {
                var lon = from ln in ctx.Loans
                          select new { LoanID = ln.LoanID, TypeOfLoan = ln.TypeOfLoan, Type=ln.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName= ln.Client.LastName };
                dgLoan.ItemsSource = lon.ToList();

            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;
            rg();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanApplication frm = new wpfLoanApplication();
            frm.status = "Edit";
            int id=Convert.ToInt32(getRow(dgLoan, 0));
            frm.lId = id;
            frm.btnContinue.Content = "Update";
            frm.ShowDialog();
        }
    }
}
