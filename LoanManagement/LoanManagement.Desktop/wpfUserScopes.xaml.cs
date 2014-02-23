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
    /// Interaction logic for wpfUserScopes.xaml
    /// </summary>
    public partial class wpfUserScopes : MetroWindow
    {
        public int ID;
        public int UserID;
        public wpfUserScopes()
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

                using (var ctx = new newContext())
                {
                    var sc = ctx.Scopes.Find(ID);
                    cClient.IsChecked = sc.ClientM;
                    cAgents.IsChecked = sc.AgentM;
                    cServices.IsChecked = sc.ServiceM;
                    cBanks.IsChecked = sc.BankM;
                    cEmployee.IsChecked = sc.EmployeeM;
                    cPosition.IsChecked = sc.PositionM;

                    cApplication.IsChecked = sc.Application;
                    cApproval.IsChecked = sc.Approval;
                    cReleasing.IsChecked = sc.Releasing;
                    cPayments.IsChecked = sc.Payments;
                    cManageClosed.IsChecked = sc.ManageCLosed;
                    cRestructure.IsChecked = sc.Resturcture;
                    cAdjustment.IsChecked = sc.PaymentAdjustment;

                    cArchive.IsChecked = sc.Archive;
                    cBackup.IsChecked = sc.BackUp;
                    cUsers.IsChecked = sc.UserAccounts;
                    cReports.IsChecked = sc.Reports;
                    cStatistics.IsChecked = sc.Statistics;
                    cUserScopes.IsChecked = sc.Scopes;
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
            MessageBoxResult mr = MessageBox.Show("Are you sure you want to save this record", "Question", MessageBoxButton.YesNo);
            if (mr == MessageBoxResult.Yes)
            {
                using (var ctx = new newContext())
                {
                    var sc = ctx.Scopes.Find(ID);
                    sc.ClientM = Convert.ToBoolean(cClient.IsChecked);
                    sc.AgentM = Convert.ToBoolean(cAgents.IsChecked);
                    sc.ServiceM = Convert.ToBoolean(cServices.IsChecked);
                    sc.BankM = Convert.ToBoolean(cBanks.IsChecked);
                    sc.EmployeeM = Convert.ToBoolean(cEmployee.IsChecked);
                    sc.PositionM = Convert.ToBoolean(cEmployee.IsChecked);

                    sc.Application = Convert.ToBoolean(cApplication.IsChecked);
                    sc.Approval = Convert.ToBoolean(cApproval.IsChecked);
                    sc.Releasing = Convert.ToBoolean(cReleasing.IsChecked);
                    sc.Payments = Convert.ToBoolean(cPayments.IsChecked);
                    sc.ManageCLosed = Convert.ToBoolean(cManageClosed.IsChecked);
                    sc.Resturcture = Convert.ToBoolean(cRestructure.IsChecked);
                    sc.PaymentAdjustment = Convert.ToBoolean(cAdjustment.IsChecked);

                    sc.Archive = Convert.ToBoolean(cArchive.IsChecked);
                    sc.BackUp = Convert.ToBoolean(cBackup.IsChecked);
                    sc.UserAccounts = Convert.ToBoolean(cUsers.IsChecked);
                    sc.Reports = Convert.ToBoolean(cReports.IsChecked);
                    sc.Statistics = Convert.ToBoolean(cStatistics.IsChecked);
                    sc.Scopes = Convert.ToBoolean(cUserScopes.IsChecked);

                    ctx.SaveChanges();
                    MessageBox.Show("Receord has been successfully saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }
    }
}
