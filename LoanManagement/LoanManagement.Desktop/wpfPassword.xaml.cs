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

using MahApps.Metro.Controls;
using System.Data.Entity;
using LoanManagement.Domain;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfPassword.xaml
    /// </summary>
    public partial class wpfPassword : MetroWindow
    {
        public string status;
        public int ID;
        public int eID;
        public int UserID;

        public wpfPassword()
        {
            InitializeComponent();
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
                if (status == "view")
                {
                    lbl.Content = "Enter password for this user";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnCont_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "view")
                {
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.Users.Where(x => x.EmployeeID == ID && x.Password == txtPassword.Password).Count();
                        if (ctr > 0)
                        {
                            wpfUserInfo frm = new wpfUserInfo();
                            frm.status = status;
                            frm.eId = ID;
                            this.Close();
                            frm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtPassword.Password = "";
                            return;
                        }
                    }
                }
                else if (status == "void")
                {
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.Users.Where(x => x.EmployeeID == UserID && x.Password == txtPassword.Password).Count();
                        if (ctr > 0)
                        {
                            var ctr1 = Application.Current.Windows.Count;
                            var frm = Application.Current.Windows[ctr1 - 2] as wpfLoanSearch;
                            frm.cont = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtPassword.Password = "";
                            return;
                        }
                    }
                }
                else if (status == "scope")
                {
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.Users.Where(x => x.EmployeeID == ID && x.Password == txtPassword.Password).Count();
                        if (ctr > 0)
                        {
                            wpfUserScopes frm = new wpfUserScopes();
                            frm.ID = eID;
                            this.Close();
                            frm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtPassword.Password = "";
                            return;
                        }
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
