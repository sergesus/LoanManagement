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
    /// Interaction logic for wpfUsers.xaml
    /// </summary>
    /// 

    public partial class wpfUsers : MetroWindow
    {
        public int UserID;

        public wpfUsers()
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
                    var emp = from em in ctx.Users where em.Employee.Active == true select new { EmployeeID= em.EmployeeID, Name = em.Employee.FirstName + " " + em.Employee.MI + " " + em.Employee.LastName, Username = em.Username };
                    dgEmp.ItemsSource = emp.ToList();
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
                    var emp = ctx.Employees.Find(Convert.ToInt32(getRow(dgEmp, 0)));
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
                    lblPosition.Content = "Position: " + emp.Position;
                    lblDept.Content = "Department: " + emp.Department;
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MetroWindow_Activated_1(object sender, EventArgs e)
        {
            try
            {
                resetGrid();
                lblDept.Content = "";
                lblName.Content = "";
                lblPosition.Content = "";
                img.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
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
                using (var ctx = new iContext())
                { 
                    int n = Convert.ToInt32(getRow(dgEmp,0));
                    var emp = ctx.Employees.Find(n);
                    if (emp.Position.PositionName == "Administrator")
                    {
                        System.Windows.MessageBox.Show("Unable to edit Administrator");
                        return;
                    }
                    else
                    {
                        wpfPassword frm = new wpfPassword();
                        frm.status = "view";
                        frm.ID = n;
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            wpfClientSearch frm = new wpfClientSearch();
            frm.status = "Employee";
            frm.ShowDialog();
        }

        private void btnScope_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
                {
                    int n = Convert.ToInt32(getRow(dgEmp, 0));
                    var emp = ctx.Employees.Find(n);
                    if (emp.Position.PositionName == "Administrator")
                    {
                        System.Windows.MessageBox.Show("Unable to edit Administrator");
                        return;
                    }
                    else
                    {
                        wpfPassword frm = new wpfPassword();
                        frm.status = "scope";
                        frm.ID = UserID;
                        frm.eID = n;
                        frm.ShowDialog();
                    }
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
