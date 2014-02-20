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
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfNewCheque.xaml
    /// </summary>
    public partial class wpfNewCheque : MetroWindow
    {

        public int fId;
        public int lID;
        public string status;
        public int UserID;
        public bool cont = false;

        public wpfNewCheque()
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
                //Grid grid = new Grid();
                wdw1.Background = myBrush;

                if (status == "Full")
                {
                    lblDaif.Visibility = Visibility.Hidden;
                    chDaif.Visibility = Visibility.Hidden;
                    using (var ctx = new finalContext())
                    {
                        var fp = from f in ctx.FPaymentInfo
                                 where f.LoanID == lID && f.PaymentStatus!= "Cleared"
                                 select f;
                        double tot = 0;
                        foreach (var itm in fp)
                        {
                            tot = tot + itm.Amount;
                        }
                        txtAmt.Text = tot.ToString("N2");
                    }
                    return;
                }

                using (var ctx = new finalContext())
                {
                    var fp = ctx.FPaymentInfo.Find(fId);
                    txtAmt.Text = fp.Amount.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    FPaymentInfo fp = ctx.FPaymentInfo.Find(fId);
                    DepositedCheque dp = ctx.DepositedCheques.Find(fId);
                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {

                        if (status == "Full")
                        {
                            var fp2 = from f in ctx.FPaymentInfo
                                     where f.LoanID == lID && f.PaymentStatus != "Cleared"
                                     select f;
                            double tot = 0;
                            foreach (var itm in fp2)
                            {
                                itm.PaymentStatus = "Cleared";
                                ClearedCheque cc = new ClearedCheque { FPaymentInfoID = itm.FPaymentInfoID, DateCleared = DateTime.Now };
                            }

                            FPaymentInfo f2 = new FPaymentInfo { Amount = double.Parse(txtAmt.Text), ChequeDueDate = DateTime.Now.Date, ChequeInfo = txtId.Text, LoanID = lID, PaymentNumber = fp2.Count() + 1, PaymentStatus = "Cleared", PaymentDate = DateTime.Now, RemainingBalance = 0 };
                            ctx.FPaymentInfo.Add(f2);
                            var lon = ctx.Loans.Find(lID);
                            lon.Status = "Paid";
                            PaidLoan pl = new PaidLoan { DateFinished = DateTime.Now, LoanID = lID };
                            ctx.PaidLoans.Add(pl);
                            ctx.SaveChanges();
                            ClearedCheque cc2 = new ClearedCheque { DateCleared = DateTime.Now, FPaymentInfoID = f2.FPaymentInfoID };
                            MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                            return;
                        }


                        if (chDaif.IsChecked == false)
                        {
                            wpfCheckout frm = new wpfCheckout();
                            frm.status = "Daif";
                            frm.fId = fId;
                            frm.ShowDialog();

                            if (cont == false)
                            {
                                System.Windows.MessageBox.Show("Please pay the DAIF fee first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Changed Cheque " + fp.ChequeInfo + " to " + txtId.Text + "" };
                        ctx.AuditTrails.Add(at);
                        fp.PaymentStatus = "Deposited";
                        fp.ChequeInfo = txtId.Text;
                        dp.DepositDate = DateTime.Today.Date;
                        

                        fp.ReturnedCheque.isPaid = true;


                        if (chDaif.IsChecked == true)
                        {
                            fp.Amount = fp.Amount + fp.ReturnedCheque.Fee;
                        }
                        ctx.SaveChanges();
                        MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chDaif_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var fp = ctx.FPaymentInfo.Find(fId);
                    double tot = fp.Amount + fp.ReturnedCheque.Fee;
                    txtAmt.Text = tot.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void chDaif_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var fp = ctx.FPaymentInfo.Find(fId);
                    txtAmt.Text = fp.Amount.ToString("N2");
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
