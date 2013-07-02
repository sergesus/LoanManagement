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



        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                /*Employee emp = new Employee { FName = "Aldrin", MI = "A", LName = "Arciga", Address = "Apalit, Pampanga", Contact = "09066666867", Email = "aldrinarciga@gmail.com" };
                User usr = new User();
                usr.Username = "admin";
                usr.Password="123";
                ctx.Users.Add(usr);
                ctx.Employees.Add(emp);
                ctx.SaveChanges();*/
                var ctr = ctx.Users.Count();
                if (ctr < 1)
                { 
                    //
                }
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

                
                
                using (var ctx = new SystemContext())
                {
                    var count = ctx.Users.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password).Count();
                    if (count > 0)
                    {
                        //MessageBox.Show("Login Successfull","Information",MessageBoxButton.OK,MessageBoxImage.Information);
                        wpfMain wnd = new wpfMain();
                        wnd.Show();
                        this.Close();
                    }
                    else
                    {
                        //MessageBox.Show("Username/Password is incorrect","Error");
                        pr1.IsActive = !true;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
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
    }

    
}
