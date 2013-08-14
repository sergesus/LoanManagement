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
    /// Interaction logic for wpfClient.xaml
    /// </summary>
    public partial class wpfClient : MetroWindow
    {
        public wpfClient()
        {
            InitializeComponent();
        }

        private static byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitMap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitMap.RawFormat;
            var imageToConvert = System.Drawing.Image.FromFile(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, bmpFormat);
                return ms.ToArray();
            }
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
                //System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return "";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            wpfClientInfo frm = new wpfClientInfo();
            frm.status = "Add";
            frm.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientInfo frm = new wpfClientInfo();
                frm.status = "View";
                frm.cId = Convert.ToInt32(getRow(dgClient, 0));
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

        private void resetGrid()
        {
            using (var ctx = new MyContext())
            {
                var clt = from cl in ctx.Clients
                          where cl.Active == true
                          select new { ClientID=cl.ClientID, FirstName=cl.FirstName, MiddleName=cl.MiddleName, LastName=cl.LastName, Suffix=cl.Suffix, Birthday=cl.Birthday };
                dgClient.ItemsSource = clt.ToList();
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            resetGrid();
            lblName.Content = "";
            img.Visibility = Visibility.Hidden;
        }

        private void dgClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (var ctx = new MyContext())
                {
                    img.Visibility = Visibility.Visible;
                    var clt = ctx.Clients.Find(Convert.ToInt32(getRow(dgClient, 0)));
                    byte[] imageArr;
                    imageArr = clt.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;
                    lblName.Content = clt.FirstName + " " + clt.MiddleName + " " + clt.LastName + " " + clt.Suffix;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
