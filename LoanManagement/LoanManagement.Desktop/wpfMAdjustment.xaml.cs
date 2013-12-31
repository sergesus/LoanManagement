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
    /// Interaction logic for wpfMAdjustment.xaml
    /// </summary>
    public partial class wpfMAdjustment : MetroWindow
    {
        public int UserID;

        public wpfMAdjustment()
        {
            InitializeComponent();
        }

        private void btnAdjust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtTo.SelectedDate.Value.Date < dtFrom.SelectedDate.Value.Date)
                {
                    System.Windows.MessageBox.Show("TO date must be breater than or equal to FROM date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                using (var ctx = new newerContext())
                {
                    var py = from p in ctx.MPaymentInfoes
                             where (p.DueDate <= dtTo.SelectedDate.Value && p.DueDate >= dtFrom.SelectedDate.Value) && (p.PaymentStatus == "Unpaid" || p.PaymentStatus=="Pending")
                             select p;
                    var ctr = ctx.MPaymentInfoes.Where(p=> (p.DueDate <= dtTo.SelectedDate.Value && p.DueDate >= dtFrom.SelectedDate.Value) && (p.PaymentStatus == "Unpaid" || p.PaymentStatus == "Pending")).Count();
                    if (ctr > 0)
                    {
                        var ps = ctx.MPaymentInfoes.Where(p=> (p.DueDate <= dtTo.SelectedDate.Value && p.DueDate >= dtFrom.SelectedDate.Value) && (p.PaymentStatus == "Unpaid" || p.PaymentStatus == "Pending")).First();
                        double rem = ps.RemainingLoanBalance;
                        foreach (var itm in py)
                        {
                            itm.TotalAmount = itm.Amount + itm.PreviousBalance;
                            itm.TotalBalance = itm.TotalBalance - itm.BalanceInterest;
                            itm.RemainingLoanBalance = rem;
                            //if (itm.BalanceInterest != 0)
                            //{
                            //itm.PreviousBalance = itm.PreviousBalance - itm.BalanceInterest;
                            //itm.TotalAmount = itm.Amount + itm.PreviousBalance;
                            //}
                            if (itm.PaymentStatus == "Unpaid")
                            {
                                itm.PaymentStatus = "Unpaid(No Interest)";
                            }
                        }
                        foreach (var itm in py)
                        {
                            itm.BalanceInterest = 0;
                        }
                        MicroAdjusment ma = new MicroAdjusment { ToDate = dtTo.SelectedDate.Value.Date, FromDate = dtFrom.SelectedDate.Value.Date, ReasonOfAdjustment = txtReason.Text };
                        ctx.MicroAdjusments.Add(ma);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Transaction has been  successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
