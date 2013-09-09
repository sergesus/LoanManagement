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

using System.Data.Entity;
using MahApps.Metro.Controls;
using LoanManagement.Domain;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfUserInfo.xaml
    /// </summary>
    public partial class wpfUserInfo : MetroWindow
    {

        public int eId;
        public string status;
        public wpfUserInfo()
        {
            InitializeComponent();
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

                if (status == "view")
                {
                    using (var ctx = new SystemContext())
                    {
                        var u = ctx.Users.Find(eId);
                        txtUserName.Text = u.Username;
                        txtUserName.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPassword.Password != txtConfirm.Password)
                {
                    MessageBox.Show("Password doesn't match");
                    txtPassword.Password = "";
                    txtConfirm.Password = "";
                    return;
                }
                if (status != "view")
                {
                    using (var ctx = new SystemContext())
                    {
                        var u = ctx.Users.Where(x => x.Username == txtUserName.Text).Count();
                        if (u > 0)
                        {
                            MessageBox.Show("Username already exists");
                            txtUserName.Text = "";
                            return;
                        }

                        User usr = new User { EmployeeID = eId, Username = txtUserName.Text, Password = txtPassword.Password };
                        Scope sc = new Scope { EmployeeID = eId };
                        ctx.Users.Add(usr);
                        ctx.Scopes.Add(sc);
                        ctx.SaveChanges();
                        MessageBox.Show("Okay");
                        this.Close();
                    }
                }
                else
                {
                    using (var ctx = new SystemContext())
                    {
                        var u = ctx.Users.Find(eId);
                        u.Password = txtPassword.Password;
                        ctx.SaveChanges();
                        MessageBox.Show("Okay");
                        this.Close();
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
