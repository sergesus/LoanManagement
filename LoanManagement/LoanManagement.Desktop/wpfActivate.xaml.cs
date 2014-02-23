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

using LoanManagement.Domain;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfActivate.xaml
    /// </summary>
    public partial class wpfActivate : MetroWindow
    {
        public wpfActivate()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new newContext())
            {
                if (txtUsername.Text == "" || txtPassword.Password == "")
                {
                    MessageBox.Show("Please input username/password");
                    return;
                }
                var ctr = ctx.Users.Where(x => x.Username == txtUsername.Text).Count();
                if (ctr > 0)
                {
                    var usr = ctx.Users.Where(x => x.Username == txtUsername.Text).First();
                    if (usr.Employee.Position.PositionName != "Administrator")
                    {
                        MessageBox.Show("Only the administrator is allowed to activate the system.");
                        return;
                    }
                    else
                    {
                        var c = ctx.Users.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password).Count();
                        if (c > 0)
                        {
                            var st = ctx.State.Find(1);
                            st.iState = 0;
                            ctx.SaveChanges();
                            MessageBox.Show("System has been successfuly activated.");
                            wpfLogin frm = new wpfLogin();
                            frm.ShowDialog();
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect admin information.");
                    return;
                }
            }
        }
    }
}
