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

using LoanManagement.Domain;
using System.Data.Entity;
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfClientSearch.xaml
    /// </summary>
    public partial class wpfClientSearch : MetroWindow
    {
        public string status;
        public int cId;
        public string status2;
        public int UserID;

        public wpfClientSearch()
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
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                resetGrid();
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void resetGrid()
        {
            try
            {
                int n = 0;
                try
                {
                    n = Convert.ToInt32(txtSearch.Text);
                }
                catch (Exception)
                {
                    n = 0;
                }
                if (status == "Client")
                {
                    if (txtSearch.Text == "")
                    {
                        using (var ctx = new newContext())
                        {
                            var clt = from cl in ctx.Clients
                                      where cl.Active == true && cl.ClientID != cId 
                                      select new { ClientID = cl.ClientID, Name = cl.FirstName + " " + cl.MiddleName + " " + cl.LastName + " " + cl.Suffix };
                            dgClient.ItemsSource = clt.ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new newContext())
                        {
                            var clt = from cl in ctx.Clients
                                      where cl.Active == true && cl.ClientID != cId && (cl.ClientID == n || (cl.FirstName + cl.MiddleName + cl.LastName).Contains(txtSearch.Text.Replace(" ", "")))
                                      select new { ClientID = cl.ClientID, Name = cl.FirstName + " " + cl.MiddleName + " " + cl.LastName + " " + cl.Suffix };
                            dgClient.ItemsSource = clt.ToList();
                        }
                    }
                }
                else if (status == "Agent")
                {
                    if (txtSearch.Text == "")
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Agents
                                      where ag.Active == true
                                      select new { AgentID = ag.AgentID, FirstName = ag.FirstName, MiddleName = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Agents
                                      where ag.Active == true && (ag.FirstName + " " + ag.MI + " " + ag.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", ""))
                                      select new { AgentID = ag.AgentID, FirstName = ag.FirstName, MiddleName = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                }
                else if (status == "CI")
                {
                    if (txtSearch.Text == "")
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true && (ag.FirstName + " " + ag.MI + " " + ag.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", ""))
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                }
                else if (status == "Collector")
                {
                    if (txtSearch.Text == "")
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true && ag.Position.PositionName.Contains("Collector")
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true && (ag.FirstName + " " + ag.MI + " " + ag.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")) && ag.Position.PositionName.Contains("Collector")
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                }
                else if (status == "Collector1")
                {
                    if (txtSearch.Text == "")
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true && ag.Position.PositionName.Contains("Collector")
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true && (ag.FirstName + " " + ag.MI + " " + ag.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")) && ag.Position.PositionName.Contains("Collector")
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                }
                else if (status == "Employee")
                {
                    if (txtSearch.Text == "")
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new newContext())
                        {
                            var agt = from ag in ctx.Employees
                                      where ag.Active == true && (ag.FirstName + " " + ag.MI + " " + ag.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", ""))
                                      select new { EmployeeID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                            dgClient.ItemsSource = agt.ToList();
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

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Client")
                {
                    var ctr = Application.Current.Windows.Count;
                    if (status2 == "LoanApplication")
                    {
                        var frm = Application.Current.Windows[ctr - 2] as wpfLoanApplication;
                        int cbId = Convert.ToInt32(getRow(dgClient, 0));
                        frm.cbId = Convert.ToInt32(getRow(dgClient, 0));
                        frm.UserID = UserID;
                        using (var ctx = new newContext())
                        {
                            var agt = ctx.Clients.Find(cbId);
                            String str = "(" + cbId + ")" + agt.FirstName + " " + agt.MiddleName + " " + agt.LastName;
                            frm.txtID.Text = str;
                        }
                    }
                    else if (status2 == "View1")
                    {
                        wpfViewClientInfo frm = new wpfViewClientInfo();
                        frm.status = status2;
                        int myCID = Convert.ToInt32(getRow(dgClient, 0));
                        frm.cID = myCID;
                        frm.UserID = UserID;
                        this.Close();
                        frm.ShowDialog();
                    }
                    else
                    {
                        var frm = Application.Current.Windows[ctr - 2] as wpfSelectClient;
                        frm.txtID.Text = getRow(dgClient, 0);
                        frm.UserID = UserID;
                        if (frm.txtID.Text == "")
                        {
                            return;
                        }
                    }
                    this.Close();
                }
                else if (status == "Agent")
                {
                    var ctr = Application.Current.Windows.Count;
                    var frm = Application.Current.Windows[ctr - 2] as wpfLoanApplication;
                    frm.UserID = UserID;
                    int num = Convert.ToInt32(getRow(dgClient, 0));
                    frm.agentId = num;
                    this.Close();
                }
                else if (status == "CI")
                {
                    var ctr = Application.Current.Windows.Count;
                    var frm = Application.Current.Windows[ctr - 2] as wpfLoanApplication;
                    frm.UserID = UserID;
                    int num = Convert.ToInt32(getRow(dgClient, 0));
                    frm.ciId = num;
                    using (var ctx = new newContext())
                    {
                        var agt = ctx.Employees.Find(num);
                        String str = "(" + num + ")" + agt.FirstName + " " + agt.MI + " " + agt.LastName;
                        frm.txtCI.Text = str;
                    }
                    this.Close();
                }
                else if (status == "Collector")
                {
                    var ctr = Application.Current.Windows.Count;
                    var frm = Application.Current.Windows[ctr - 2] as wpfMReleasing;
                    frm.UserID = UserID;
                    int num = Convert.ToInt32(getRow(dgClient, 0));
                    frm.ciId = num;
                    using (var ctx = new newContext())
                    {
                        var agt = ctx.Employees.Find(num);
                        String str = "(" + num + ")" + agt.FirstName + " " + agt.MI + " " + agt.LastName;
                        frm.txtCI.Text = str;
                    }
                    this.Close();
                }
                else if (status == "Collector1")
                {
                    var ctr = Application.Current.Windows.Count;
                    var frm = Application.Current.Windows[ctr - 2] as wpfPassToCollectors;
                    frm.UserID = UserID;
                    int num = Convert.ToInt32(getRow(dgClient, 0));
                    frm.cID = num;
                    using (var ctx = new newContext())
                    {
                        var agt = ctx.Employees.Find(num);
                        String str = "(" + num + ")" + agt.FirstName + " " + agt.MI + " " + agt.LastName;
                        frm.txtCI.Text = str;
                    }
                    this.Close();
                }
                else if (status == "Employee")
                {
                    int num = Convert.ToInt32(getRow(dgClient, 0));
                    wpfUserInfo frm = new wpfUserInfo();
                    frm.UserID = UserID;
                    frm.eId = num;
                    this.Close();
                    frm.ShowDialog();
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

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                resetGrid();
            }
            catch (Exception)
            {
                return;
            }
        }

    }
}
