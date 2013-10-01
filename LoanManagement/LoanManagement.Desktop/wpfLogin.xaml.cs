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
//addition
using LoanManagement.Domain;
using System.Data.Entity;
using MahApps.Metro.Controls;

using System.IO;
using System.Windows.Forms;
//using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLogin.xaml
    /// </summary>
    public partial class wpfLogin : MetroWindow
    {
        public wpfLogin()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            
        }

        private void checkDue()
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from lo in ctx.FPaymentInfo
                              where lo.PaymentDate <= DateTime.Today.Date && (lo.PaymentStatus == "Pending" || lo.PaymentStatus == "On Hold")
                              select lo;
                    foreach (var item in lon)
                    {
                        var ctr = ctx.FPaymentInfo.Where(x => (x.PaymentDate <= DateTime.Today.Date && x.LoanID == item.LoanID) && (x.PaymentStatus == "Due" || x.PaymentStatus == "Returned" || x.PaymentStatus == "Due/Pending" || x.PaymentStatus == "Deposited")).Count();
                        if (ctr == 0)
                        {
                            item.PaymentStatus = "Due";
                        }
                        else
                        {
                            item.PaymentStatus = "Due/Pending";
                        }
                    }

                    var dep = from d in ctx.FPaymentInfo
                              where d.PaymentStatus == "Due"
                              select d;
                    foreach (var item in dep)
                    {
                        var ctr = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Deposited").Count();
                        if (ctr != 0)
                        {
                            item.PaymentStatus = "Due/Pending";
                        }
                    }

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //System.Windows.MessageBox.Show("Okay");
                checkDue();
                using (var ctx = new MyLoanContext())
                {
                    /*Employee emp = new Employee { FirstName = "Aldrin", MI = "A", LastName = "Arciga",  Email = "aldrinarciga@gmail.com", Active=true };
                    User usr = new User();
                    usr.Username = "aldrin";
                    usr.Password="123";
                    ctx.Users.Add(usr);
                    ctx.Employees.Add(emp);
                    ctx.SaveChanges();
                    var ctr = ctx.Users.Count();
                    if (ctr < 1)
                    { 
                        //
                    }*/
                }
                String selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                bitmap.EndInit();
                img.Source = bitmap;


                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hi");
            //pr1.IsActive = true;
            try
            {

                if (txtUsername.Text == "")
                {
                    //MessageBox.Show("Please enter your username", "Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    FocusManager.SetFocusedElement(this, txtUsername);
                    return;
                }

                if (txtPassword.Password == "")
                {
                    //MessageBox.Show("Please enter your password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    FocusManager.SetFocusedElement(this, txtPassword);
                    return;
                }



                using (var ctx = new MyLoanContext())
                {
                    var count = ctx.Users.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password).Count();
                    if (count > 0)
                    {
                        System.Windows.MessageBox.Show("Login Successful", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        wpfMain wnd = new wpfMain();
                        wnd.Show();
                        this.Close();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Username/Password is incorrect", "Error");
                        pr1.IsActive = !true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    var ctr = ctx.Users.Where(x => x.Username == txtUsername.Text).Count();
                    if (ctr > 0)
                    {
                        var usr = ctx.Users.Where(x => x.Username == txtUsername.Text).First();
                        byte[] imageArr;
                        imageArr = usr.Employee.Photo;
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CreateOptions = BitmapCreateOptions.None;
                        bi.CacheOption = BitmapCacheOption.Default;
                        bi.StreamSource = new MemoryStream(imageArr);
                        bi.EndInit();
                        img.Source = bi;
                    }
                    else
                    {
                        String selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                        bitmap.EndInit();
                        img.Source = bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }

    
}
