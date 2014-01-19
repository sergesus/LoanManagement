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
    /// Interaction logic for wpfCheckout.xaml
    /// </summary>
    public partial class wpfCheckout : MetroWindow
    {
        public string status;
        public int fId;
        public int lId;
        public double days;
        public int UserID;
        public wpfCheckout()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "PBC")
                {

                }
                else if (status == "Holding")
                {
                    if (Convert.ToDouble(txtCash.Text) < Convert.ToDouble(lbl2.Content))
                    {
                        MessageBox.Show("Invalid Cash Amount");
                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                            double hFee = dts.Amount * (dts.Loan.Service.Holding / 100);
                            hFee = Convert.ToDouble(hFee.ToString("N2"));
                            HeldCheque hc = new HeldCheque { DateHeld = DateTime.Today.Date, LoanID = lId, NewPaymentDate = dts.PaymentDate.AddDays(7), OriginalPaymentDate = dts.PaymentDate, PaymentNumber = dts.PaymentNumber, HoldingFee = hFee };
                            dts.PaymentDate = dts.PaymentDate.AddDays(7);
                            dts.PaymentStatus = "On Hold";
                            ctx.HeldCheques.Add(hc);
                            ctx.SaveChanges();
                            //reset();
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Processed Holding for cheque "+ dts.ChequeInfo +""};
                            ctx.AuditTrails.Add(at);
                            System.Windows.MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                            this.Close();
                        }
                    }
                }
                else if (status == "Adjustment")
                {
                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new newerContext())
                        {
                            AdjustedLoan al = new AdjustedLoan { DateAdjusted = DateTime.Today.Date, Days = Convert.ToInt32(days), Fee = Convert.ToDouble(lbl2.Content), LoanID = lId };
                            var py = from p in ctx.FPaymentInfo
                                     where p.PaymentStatus != "Cleared" && p.LoanID == lId
                                     select p;
                            foreach (var item in py)
                            {
                                item.PaymentDate = item.PaymentDate.AddDays(days);
                            }
                            ctx.AdjustedLoans.Add(al);
                            ctx.SaveChanges();
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Processed Adjustment for Loan " + lId + "" };
                            ctx.AuditTrails.Add(at);

                            System.Windows.MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                    }
                }
                else if (status == "Restructure")
                {
                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {
                        var ctr = Application.Current.Windows.Count;
                        var frm = Application.Current.Windows[ctr - 2] as wpfLoanRestructure;
                        frm.cont=true;
                        MessageBox.Show("Transaction has been successfully processed. Restructure will now be processed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else if (status == "Daif")
                {
                    var ctr = Application.Current.Windows.Count;
                    var frm = Application.Current.Windows[ctr - 2] as wpfNewCheque;
                    frm.cont = true;
                    MessageBox.Show("Transaction has been successfully processed. Renewal of Cheque will now be processed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else if (status == "RenewClosed")
                {
                    var ctr = Application.Current.Windows.Count;
                    var frm = Application.Current.Windows[ctr - 2] as wpfRenewClosed;
                    frm.cont = true;
                    MessageBox.Show("Transaction has been successfully processed. Renewal of Account will now be processed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
                //Grid grid = new Grid();
                wdw1.Background = myBrush;

                if (status == "PBC")
                { 
                    
                }
                else if (status == "Holding")
                {
                    lbl1.Content = "Cheque Holding";
                    using (var ctx = new newerContext())
                    { 
                       var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                       double hFee = dts.Amount * (dts.Loan.Service.Holding / 100);
                       hFee = Convert.ToDouble(hFee.ToString("N2"));
                       lbl2.Content = hFee.ToString("N2");
                    }
                }
                else if (status == "Adjustment")
                {
                    lbl1.Content = "Payment Adjustment";
                }
                else if (status == "Restructure")
                {
                    lbl1.Content = "Loan Restructure";
                }
                else if (status == "Daif")
                {
                    lbl1.Content = "DAIF Fee";
                    using (var ctx = new newerContext())
                    {
                        var fee = ctx.ReturnedCheques.Find(fId);
                        double hFee = Convert.ToDouble(fee.Fee.ToString("N2"));
                        lbl2.Content = hFee.ToString("N2");
                    }
                }
                else if (status == "RenewClosed")
                {
                    lbl1.Content = "Closed Account Fee";
                    using (var ctx = new newerContext())
                    {
                        var fee = ctx.ClosedAccounts.Where(x => x.isPaid == false && x.LoanID == lId).First();
                        double hFee = Convert.ToDouble(fee.Fee.ToString("N2"));
                        lbl2.Content = hFee.ToString("N2");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Double n = Convert.ToDouble(txtCash.Text) - Convert.ToDouble(lbl2.Content);
                lblChange.Content = n.ToString("N2");
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
