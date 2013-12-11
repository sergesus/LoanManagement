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
        public bool status;
        public int UserID;

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
            try
            {
                wpfClientInfo frm = new wpfClientInfo();
                frm.status = "Add";
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientInfo frm = new wpfClientInfo();
                frm.status = "View";
                frm.UserID = UserID;
                frm.cId = Convert.ToInt32(getRow(dgClient, 0));
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;

                 if (status == false)//archive
                {
                    btnView.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnRet.Visibility = Visibility.Visible;
                    myLbL.Content = "Client Retreival";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void resetGrid()
        {
            try
            {
                using (var ctx = new iContext())
                {
                    var clt = from cl in ctx.Clients
                              where cl.Active == status && cl.isConfirmed == true
                              select new { ClientID = cl.ClientID, FirstName = cl.FirstName, MiddleName = cl.MiddleName, LastName = cl.LastName, Suffix = cl.Suffix, Birthday = cl.Birthday };
                    dgClient.ItemsSource = clt.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            try
            {
                resetGrid();
                lblName.Content = "";
                img.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dgClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
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
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = Convert.ToInt32(getRow(dgClient, 0));
                MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to retreive this record?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new iContext())
                    {
                        var agt = ctx.Clients.Find(n);
                        agt.Active = true;
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Retrieved Client " + agt.FirstName + " " + agt.MiddleName + " " + agt.LastName + " " + agt.Suffix };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Record has been successfully retreived", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        resetGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dgClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
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
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
                {
                    int n;
                    try
                    {
                        n = Convert.ToInt16(txtSearch.Text);
                    }
                    catch (Exception)
                    {
                        n = 0;
                    }
                    var clt = from cl in ctx.Clients
                              where (cl.Active == status) && ((cl.FirstName + cl.LastName + cl.MiddleName + cl.Suffix).Contains(txtSearch.Text) || cl.ClientID==n)
                              select new { ClientID = cl.ClientID, FirstName = cl.FirstName, MiddleName = cl.MiddleName, LastName = cl.LastName, Suffix = cl.Suffix, Birthday = cl.Birthday };
                    dgClient.ItemsSource = clt.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
