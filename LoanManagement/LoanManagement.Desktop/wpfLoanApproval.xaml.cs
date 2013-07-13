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

using System.IO;
using LoanManagement.Domain;
using System.Windows.Forms;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using MahApps.Metro.Controls;
using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanApproval.xaml
    /// </summary>
    public partial class wpfLoanApproval : MetroWindow
    {

        public int lId;
        public wpfLoanApproval()
        {
            InitializeComponent();
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            dtDate.SelectedDate = DateTime.Today.Date;
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;

            using (var ctx = new SystemContext())
            {
                var lon = ctx.Loans.Find(lId);
                lblDesAmt.Content = "Php " + lon.LoanApplication.AmountApplied.ToString("N2");
                lblDesTerm.Content = lon.Term.ToString();
                txtAmt.Text = lon.LoanApplication.AmountApplied.ToString("N2");
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtAmt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var ctx = new SystemContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete from dbo.GenSOAs");
                    var lon = ctx.Loans.Find(lId);
                    Double Amt = Convert.ToDouble(txtAmt.Text);
                    lblPrincipal.Content = Amt.ToString("N2");
                    txtAmt.Text = Amt.ToString("N2");
                    txtAmt.SelectionStart=txtAmt.Text.Length - 3;
                    Double TotalInt = lon.Interest * lon.Term;
                    TotalInt = TotalInt / 100;
                    Double Deduction = lon.Deduction / 100;
                    Double NetProceed = (Convert.ToDouble(txtAmt.Text)) - (Convert.ToDouble(txtAmt.Text) * Deduction);
                    Double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);
                    lblProceed.Content = "Php " + NetProceed.ToString("N2");
                    lblInt.Content = "Php" + WithInt.ToString("N2");
                    Double Payment = 0;
                    DateTime dt = dtDate.SelectedDate.Value.Date;
                    double Interval = 0;
                    DateInterval dInt = DateInterval.Month;
                    if (lon.Mode == "Monthly")
                    {
                        Interval = 1;
                        dInt = DateInterval.Month;
                        Payment = WithInt / lon.Term;
                    }
                    else if (lon.Mode == "Semi-Monthly")
                    {
                        Interval = 15;
                        dInt = DateInterval.Day;
                        Payment = WithInt / (lon.Term *  2);
                    }
                    else if (lon.Mode == "Weekly")
                    {
                        Interval = 7;
                        dInt = DateInterval.Day;
                        Payment = WithInt / (lon.Term * 4);
                    }
                    else if (lon.Mode == "Daily")
                    {
                        Interval = 1;
                        dInt = DateInterval.Day;
                        Payment = WithInt / ((lon.Term * 4) * 7);
                    }

                    dt = DateAndTime.DateAdd(dInt, Interval, dt);
                    Double Remaining = WithInt;
                    int num = 1;
                    while (Remaining > 1)
                    {
                        Remaining = Remaining - Payment;
                        GenSOA soa = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = Remaining.ToString("N2") };
                        ctx.GenSOA.Add(soa);
                        num++;
                        //System.Windows.MessageBox.Show(Remaining.ToString());
                        dt = DateAndTime.DateAdd(dInt, Interval, dt);

                    }
                    ctx.SaveChanges();
                    var gen = from ge in ctx.GenSOA
                              select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                    dgSOA.ItemsSource = gen.ToList();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
