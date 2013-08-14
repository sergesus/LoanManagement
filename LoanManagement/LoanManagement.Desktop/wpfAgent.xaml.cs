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
    /// Interaction logic for wpfAgent.xaml
    /// </summary>
    public partial class wpfAgent : MetroWindow
    {
        public wpfAgent()
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
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return "";
            }
        }

        public void resetGrid()
        {
            using (var ctx = new MyContext())
            {
                var emp = from em in ctx.Agents where em.Active == true select new { em.AgentID, em.FirstName, em.MI, em.LastName, em.Suffix };
                dgEmp.ItemsSource = emp.ToList();
            }
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;
            resetGrid();
        }

        private void dgEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (var ctx = new MyContext())
                {
                    img.Visibility = Visibility.Visible;
                    var emp = ctx.Agents.Find(Convert.ToInt32(getRow(dgEmp, 0)));
                    byte[] imageArr;
                    imageArr = emp.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;
                    lblName.Content = emp.FirstName + " " + emp.MI + ". " + emp.LastName + " " + emp.Suffix;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            wpfAgentInfo frm = new wpfAgentInfo();
            frm.status = "Add";
            frm.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgentInfo frm = new wpfAgentInfo();
                frm.status = "View";
                frm.aId = Convert.ToInt32(getRow(dgEmp, 0));
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            resetGrid();
        }
    }
}
