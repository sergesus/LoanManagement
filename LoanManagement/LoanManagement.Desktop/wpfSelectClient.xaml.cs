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
using LoanManagement.Domain;
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfSelectClient.xaml
    /// </summary>
    public partial class wpfSelectClient : MetroWindow
    {
        public string status;
        public string iDept;
        public int UserID;

        public wpfSelectClient()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //toEdit
                status = "Application";
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "Client";
                frm.UserID = UserID;
                frm.cId = 0;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientInfo frm = new wpfClientInfo();
                frm.UserID = UserID;
                frm.status = "Add";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new newContext())
                {
                    int cid = Convert.ToInt32(txtID.Text);
                    var ctr = ctx.Clients.Where(x => x.ClientID == cid).Count();
                    if (ctr == 0)
                    {
                        MessageBox.Show("Client doesn't exists");
                        return;
                    }

                    var ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Released").Count();
                    if (ictr > 0)
                    {
                        ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Released" && x.Service.Department!= iDept).Count();
                        if (ictr > 0)
                        {
                            System.Windows.MessageBox.Show("Client has an existing loan on other department", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Released" && x.Service.Type == "Non Collateral" && x.Service.Department == iDept).Count();
                            if (ictr > 0)
                            {
                                System.Windows.MessageBox.Show("Client has an existing Non-Collateral loan. The client can only apply for collateral loan.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }

                    ictr = ctx.Loans.Where(x => x.ClientID == cid && (x.Status == "Applied" || x.Status == "Approved")).Count();
                    if (ictr > 0)
                    {
                        System.Windows.MessageBox.Show("Client cannot have multiple applications at the same time", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status=="Under Collection").Count();
                    if (ictr > 0)
                    {
                        System.Windows.MessageBox.Show("Client cannot have another application while having a loan under collection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    wpfLoanApplication frm = new wpfLoanApplication();
                    frm.cId = Convert.ToInt32(txtID.Text);
                    frm.status = "Add";
                    frm.btnContinue.Content = "Continue";
                    frm.iDept = iDept;
                    frm.UserID = UserID;
                    this.Close();
                    frm.ShowDialog();
                    //this.Close();
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
