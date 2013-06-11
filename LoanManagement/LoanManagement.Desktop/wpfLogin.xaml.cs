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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLogin.xaml
    /// </summary>
    public partial class wpfLogin : Window
    {
        public wpfLogin()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            
        }

        public string getRow(DataGrid dg, int row)
        {
            object item=dg.SelectedItem;
            string str = (dg.SelectedCells[row].Column.GetCellContent(item) as TextBlock).Text;
            return str;
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
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hi");
            try
            {
                if (txtUsername.Text == "")
                {
                    MessageBox.Show("Please enter your username", "Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    FocusManager.SetFocusedElement(this, txtUsername);
                    return;
                }

                if (txtPassword.Password == "")
                {
                    MessageBox.Show("Please enter your password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    FocusManager.SetFocusedElement(this, txtPassword);
                    return;
                }


                
                using (var ctx = new SystemContext())
                {
                    var count = ctx.Users.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password).Count();
                    if (count > 0)
                    {
                        MessageBox.Show("Login Successfull","Information",MessageBoxButton.OK,MessageBoxImage.Information);
                        wpfMain wnd = new wpfMain();
                        wnd.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username/Password is incorrect","Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }

    
}
