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
    /// Interaction logic for wpfChequeReturning.xaml
    /// </summary>
    public partial class wpfChequeReturning : MetroWindow
    {
        public wpfChequeReturning()
        {
            InitializeComponent();
        }

        public int fId;
        public double DaifFee;
        public double ClosedFee;

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

                using (var ctx = new SystemContext())
                {
                    FPaymentInfo fp = ctx.FPaymentInfo.Find(fId);
                    double n = 0;
                    n = fp.Amount * (fp.Loan.Service.DaifPenalty / 100);
                    lblDaif.Content = "(Fee: " + n.ToString("N2") + ")";
                    DaifFee = n;
                    n = fp.Amount * (fp.Loan.Service.ClosedAccountPenalty / 100);
                    lblClosed.Content = "(Fee: " + n.ToString("N2") + ")";
                    ClosedFee = n;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == MessageBoxResult.Yes)
                {
                    if (rdDaif.IsChecked == true)
                    {
                        using (var ctx = new SystemContext())
                        {
                            FPaymentInfo fp = ctx.FPaymentInfo.Find(fId);
                            ReturnedCheque rc = new ReturnedCheque { DateReturned = DateTime.Today.Date, Fee = DaifFee, FPaymentInfoID = fId, Remarks = "DAIF", isPaid = false };
                            fp.PaymentStatus = "Returned";
                            ctx.ReturnedCheques.Add(rc);
                            ctx.SaveChanges();
                            MessageBox.Show("Okay");
                            this.Close();
                        }
                    }
                    else
                    {
                        using (var ctx = new SystemContext())
                        {
                            FPaymentInfo fp = ctx.FPaymentInfo.Find(fId);
                            ClosedAccount cc = new ClosedAccount { DateClosed = DateTime.Today.Date, Fee = ClosedFee, LoanID = fp.LoanID, isPaid = false };
                            //fp.PaymentStatus = "Returned";
                            fp.Loan.Status = "Closed Account";
                            ctx.ClosedAccounts.Add(cc);

                            var chq = from c in ctx.FPaymentInfo
                                      where c.PaymentStatus != "Cleared" && c.LoanID==fp.LoanID
                                      select c;

                            foreach (var item in chq)
                            {
                                item.PaymentStatus = "Void";
                            }

                            ctx.SaveChanges();
                            MessageBox.Show("Okay");
                            this.Close();
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
