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

using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfPassToCollectors.xaml
    /// </summary>
    public partial class wpfPassToCollectors : MetroWindow
    {
        public int UserID;
        public int n;
        public int cID=0;

        public wpfPassToCollectors()
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
                //Grid grid = new Grid();
                wdw1.Background = myBrush;

                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(n);
                    lblClient.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix;

                    lblTOL.Content = lon.Service.Name;
                    lblTotalLoan.Content = lon.ReleasedLoan.TotalLoan;
                    var rmn = from rm in ctx.FPaymentInfo
                              where rm.LoanID == n && rm.PaymentStatus == "Cleared"
                              select rm;
                    double r = 0;
                    foreach (var item in rmn)
                    {
                        r = r + item.Amount;
                    }
                    double remain = lon.ReleasedLoan.TotalLoan - r;

                    lblTotalPayment.Content = r;
                    lblBalance.Content = remain;
                    txtID.Text = n.ToString();
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            if (cID == 0)
            {
                MessageBox.Show("Please assign a collector for this loan", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction? This cannot be undone", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mr == MessageBoxResult.Yes)
            {

                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(n);
                    lon.Status = "Under Collection";//active
                    int num = ctx.ClosedAccounts.Where(x => x.LoanID == n).Count();

                    var rmn = from rm in ctx.FPaymentInfo
                              where rm.LoanID == n && rm.PaymentStatus == "Cleared"
                              select rm;
                    double r = 0;
                    foreach (var item in rmn)
                    {
                        r = r + item.Amount;
                    }
                    double remain = lon.ReleasedLoan.TotalLoan - r;

                    PassedToCollector pc = new PassedToCollector { LoanID = n, DatePassed = DateTime.Today.Date, RemainingBalance = remain, TotalPassedBalance = remain, TotalPaidBeforePassing = r };
                    ctx.PassedToCollectors.Add(pc);
                    lon.CollectortID = cID;
                    AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Voided Closed Account for Loan " + lon.LoanID };
                    ctx.AuditTrails.Add(at);
                    ctx.SaveChanges();
                    MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "Collector1";
                //frm.cId = ciId;
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
