﻿using System;
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
            resetGrid();
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;
        }

        public void resetGrid()
        {
            if (status == "Client")
            {
                using (var ctx = new SystemContext())
                {
                    var clt = from cl in ctx.Clients
                              where cl.Active == true && cl.ClientID != cId
                              select new { ClientID = cl.ClientID, FirstName = cl.FirstName, MiddleName = cl.MiddleName, LastName = cl.LastName, Suffix = cl.Suffix };
                    dgClient.ItemsSource = clt.ToList();
                }
            }
            else if (status == "Agent")
            {
                using (var ctx = new SystemContext())
                {
                    var agt = from ag in ctx.Agents
                              where ag.Active == true
                              select new { AgentID = ag.AgentID, FirstName = ag.FirstName, MiddleName = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                    dgClient.ItemsSource = agt.ToList();
                }
            }
            else if (status == "CI")
            {
                using (var ctx = new SystemContext())
                {
                    var agt = from ag in ctx.Employees
                              where ag.Active == true
                              select new { AgentID = ag.EmployeeID, FirstName = ag.FirstName, MI = ag.MI, LastName = ag.LastName, Suffix = ag.Suffix };
                    dgClient.ItemsSource = agt.ToList();
                }
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Client")
            {
                var ctr = Application.Current.Windows.Count;
                if (status2 == "LoanApplication")
                {
                    var frm = Application.Current.Windows[ctr - 2] as wpfLoanApplication;
                    int cbId = Convert.ToInt32(getRow(dgClient, 0));
                    frm.cbId = Convert.ToInt32(getRow(dgClient, 0));
                    using (var ctx = new SystemContext())
                    {
                        var agt = ctx.Clients.Find(cbId);
                        String str = "(" + cbId + ")" + agt.FirstName + " " + agt.MiddleName + " " + agt.LastName;
                        frm.txtID.Text = str;
                    }
                }
                else
                {
                    var frm = Application.Current.Windows[ctr - 2] as wpfSelectClient;
                    frm.txtID.Text = getRow(dgClient, 0);
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
                var frm = Application.Current.Windows[ctr-2] as wpfLoanApplication;
                int num=Convert.ToInt32(getRow(dgClient, 0));
                frm.agentId = num;
                this.Close();
            }
            else if (status == "CI")
            {
                var ctr = Application.Current.Windows.Count;
                var frm = Application.Current.Windows[ctr - 2] as wpfLoanApplication;
                int num = Convert.ToInt32(getRow(dgClient, 0));
                frm.ciId = num;
                using (var ctx = new SystemContext())
                {
                    var agt = ctx.Employees.Find(num);
                    String str = "(" + num + ")" + agt.FirstName + " " + agt.MI + " " + agt.LastName;
                    frm.txtCI.Text = str;
                }
                this.Close();
            }
        }
    }
}