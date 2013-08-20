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

//
using System.IO;
using LoanManagement.Domain;
using System.Windows.Forms;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfServices.xaml
    /// </summary>
    public partial class wpfServices : MetroWindow
    {
        public wpfServices()
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            wpfServiceInfo frm = new wpfServiceInfo();
            frm.status = "Add";
            frm.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfServiceInfo frm = new wpfServiceInfo();
                frm.status = "View";
                frm.sId = Convert.ToInt32(getRow(dgServ, 0));
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            resetGrid();
        }

        private void resetGrid()
        {
            using (var ctx = new MyLoanContext())
            {
                var servs = from sr in ctx.Services
                            select new { ServiceNumber = sr.ServiceID, Name = sr.Name , Description=sr.Description};
                dgServ.ItemsSource = servs.ToList();
            }
        }
    }
}
