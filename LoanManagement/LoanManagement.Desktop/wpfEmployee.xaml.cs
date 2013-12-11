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
    /// Interaction logic for wpfEmployee.xaml
    /// </summary>
    public partial class wpfEmployee : MetroWindow
    {

        public bool status;
        public int UserID;

        public wpfEmployee()
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

        public void resetGrid()
        {
            try
            {
                using (var ctx = new iContext())
                {
                    var emp = from em in ctx.Employees where em.Active == status select new { em.EmployeeID, em.FirstName, em.MI, em.LastName, em.Suffix, em.Position.PositionName };
                    dgEmp.ItemsSource = emp.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }



        private void EmpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                EmpWindow.Background = myBrush;
                resetGrid();

                if (status == false)//maintenance
                {
                    btnView.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnRet.Visibility = Visibility.Visible;
                    myLbL.Content = "Employee Retreival";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        
        private void dgEmp_MouseUp(object sender, MouseButtonEventArgs e)
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
                    lblPosition.Content = "Position: " + emp.Position.PositionName;
                    lblDept.Content = "Department: " + emp.Department;
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dgEmp_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }


        private void EmpWindow_Activated(object sender, EventArgs e)
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfEmployeeInfo emp = new wpfEmployeeInfo();
                emp.status = "Add";
                emp.UserID = UserID;
                emp.ShowDialog();
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
                wpfEmployeeInfo emp = new wpfEmployeeInfo();
                emp.status = "View";
                emp.UserID = UserID;
                emp.uId = Convert.ToInt32(getRow(dgEmp, 0));
                emp.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = Convert.ToInt32(getRow(dgEmp, 0));
                MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to retreive this record?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new iContext())
                    {
                        var agt = ctx.Employees.Find(n);
                        agt.Active = true;
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Retrieved Employee " + agt.FirstName + " " + agt.MI + " " + agt.LastName + " " + agt.Suffix };
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
                    var emp = from em in ctx.Employees 
                              where (em.Active == status) && ((em.FirstName + " " + em.MI + " " + em.LastName).Contains(txtSearch.Text) || em.EmployeeID == n) 
                              select new { em.EmployeeID, em.FirstName, em.MI, em.LastName, em.Suffix, em.Position.PositionName };
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
