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
    /// Interaction logic for wpfPaymentAdjustment.xaml
    /// </summary>
    public partial class wpfPaymentAdjustment : MetroWindow
    {
        public int lId;

        public wpfPaymentAdjustment()
        {
            InitializeComponent();
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            

            
        }

        private void btnAdjust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*MessageBoxResult mr = MessageBox.Show("Sure?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new SystemContext())
                    {
                        AdjustedLoan al = new AdjustedLoan { DateAdjusted = DateTime.Today.Date, Days = Convert.ToInt32(txtDays.Text), Fee = Convert.ToDouble(lblFee.Content), LoanID = lId };
                        var py = from p in ctx.FPaymentInfo
                                 where p.PaymentStatus != "Cleared" && p.LoanID == lId
                                 select p;
                        foreach (var item in py)
                        {
                            item.PaymentDate = item.PaymentDate.AddDays(Convert.ToDouble(txtDays.Text));
                        }
                        ctx.AdjustedLoans.Add(al);
                        ctx.SaveChanges();
                        this.Close();
                    }
                }
                 * */

                if (Convert.ToInt32(txtDays.Text) > 14)
                {
                    MessageBox.Show("Maximum of 14 days only");
                    return;
                }
                wpfCheckout frm = new wpfCheckout();
                frm.status = "Adjustment";
                frm.days = Convert.ToDouble(txtDays.Text);
                frm.lId = lId;
                frm.lbl2.Content = lblFee.Content;
                frm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void wdw1_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new SystemContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    Double fee = lon.ReleasedLoan.MonthlyPayment * (lon.Service.AdjustmentFee / 100);
                    //MessageBox.Show(fee.ToString());
                    lblFee.Content = fee.ToString("N2");
                }
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
