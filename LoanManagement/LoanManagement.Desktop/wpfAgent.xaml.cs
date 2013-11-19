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

        public bool status;
        public int UserID;

        public wpfAgent()
        {
            InitializeComponent();
        }

        private static byte[] ConvertImageToByteArray(string fileName)
        {
            try
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Bitmap bitMap = new Bitmap(fileName);
                ImageFormat bmpFormat = bitMap.RawFormat;
                var imageToConvert = System.Drawing.Image.FromFile(fileName);
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, bmpFormat);
                    return ms.ToArray();
                }
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

        public void resetGrid()
        {
            try
            {
                using (var ctx = new iContext())
                {
                    var emp = from em in ctx.Agents where em.Active == status select new { em.AgentID, em.FirstName, em.MI, em.LastName, em.Suffix };
                    dgEmp.ItemsSource = emp.ToList();
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;
                resetGrid();

                if (status == false)//maintenance
                {
                    btnView.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnRet.Visibility = Visibility.Visible;
                    myLbL.Content = "Agent Retreival";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dgEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
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
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgentInfo frm = new wpfAgentInfo();
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
                wpfAgentInfo frm = new wpfAgentInfo();
                frm.status = "View";
                frm.UserID = UserID;
                frm.aId = Convert.ToInt32(getRow(dgEmp, 0));
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            try
            {
                resetGrid();
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
                int n= Convert.ToInt32(getRow(dgEmp, 0));
                MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure?","Question",MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new iContext())
                    {
                        var agt = ctx.Agents.Find(n);
                        agt.Active = true;
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Retrieved Agent " + agt.FirstName + " " + agt.MI + " " + agt.LastName + " " + agt.Suffix };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Retreived");
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
                {
                    int n;
                    try
                    {
                        n = Convert.ToInt32(txtSearch.Text);
                    }
                    catch (Exception)
                    {
                        n = 0;
                    }
                    var emp = from em in ctx.Agents 
                              where (em.Active == status) && ((em.FirstName + " " + em.MI + " " + em.LastName).Contains(txtSearch.Text) || em.AgentID == n) 
                              select new { em.AgentID, em.FirstName, em.MI, em.LastName, em.Suffix };
                    dgEmp.ItemsSource = emp.ToList();
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
