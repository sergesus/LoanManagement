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
    /// Interaction logic for wpfMPayment.xaml
    /// </summary>
    public partial class wpfMPayment : MetroWindow
    {
        public int UserID;


        public wpfMPayment()
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Grid_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));
            }
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            int lID;
            try
            {
                lID = Convert.ToInt32(txtID.Text);
            }
            catch (Exception)
            {
                lID = 0;
            }
            try
            {
                using (var ctx = new iContext())
                {
                    var ctr = ctx.Loans.Where(x => x.LoanID == lID && x.Service.Department == "Micro Business" && x.Status=="Released").Count();
                    if (ctr > 0)
                    {
                        txtAmt.IsEnabled = !false;
                        btnRecord.IsEnabled = !false;

                        var lon =ctx.Loans.Find(lID);

                        lblClient.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix;
                        
                        lblTOL.Content = lon.Service.Name;

                        var mp = ctx.MPaymentInfoes.Where(x => x.LoanID == lID && x.PaymentStatus == "Pending").First();

                        var c = ctx.MPaymentInfoes.Where(x => x.LoanID == lID && x.PaymentStatus == "Paid").Count();
                        if (c > 0)
                        {
                            var py = from p in ctx.MPaymentInfoes
                                     where p.LoanID == lID && p.PaymentStatus == "Paid"
                                     select p;
                            double tP = 0;
                            int c2 = 0;
                            foreach (var itm in py)
                            {
                                c2++;
                                if (c2 == c)
                                {
                                    lblLastPayment.Content = itm.TotalPayment.ToString("N2");
                                }
                            }
                        }
                        else
                        {
                            lblLastPayment.Content = "0.00";
                        }

                        lblCurrentPayment.Content = mp.Amount.ToString("N2");
                        lblExcessive.Content = mp.ExcessBalance.ToString("N2");
                        lblTotalLoan.Content = mp.RemainingLoanBalance.ToString("N2");
                        lblLateInterest.Content = mp.BalanceInterest.ToString("N2");
                        lblPrevBalance.Content = mp.PreviousBalance.ToString("N2");
                        lblTotalBalance.Content = mp.TotalBalance.ToString("N2");
                        lblTotal.Content = mp.TotalAmount.ToString("N2");
                        
                        
                    }
                    else
                    {
                        txtAmt.IsEnabled = false;
                        btnRecord.IsEnabled = false;
                        txtAmt.Text = "";
                        lblClient.Content = "";
                        lblCurrentPayment.Content = "";
                        lblExcessive.Content = "";
                        lblLastPayment.Content = "";
                        lblLateInterest.Content = "";
                        lblPrevBalance.Content = "";
                        lblTOL.Content = "";
                        lblTotal.Content = "";
                        lblTotalBalance.Content = "";
                        lblTotalLoan.Content = "";
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                double amt = 0;
                try
                {
                    amt = Convert.ToDouble(txtAmt.Text);
                }
                catch(Exception)
                {
                    amt = 0;
                }
                if (amt > Convert.ToDouble(lblTotalLoan.Content))
                {
                    System.Windows.MessageBox.Show("Amount must not be greater that the remaining amoung", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (amt < 1)
                {
                    System.Windows.MessageBox.Show("Amount must be greater that 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (amt <= Convert.ToDouble(lblTotal.Content))
                {
                    using (var ctx = new iContext())
                    {
                        int lID = Convert.ToInt32(txtID.Text);
                        var py = ctx.MPaymentInfoes.Where(x => x.LoanID == lID && x.PaymentStatus == "Pending").First();
                        DateInterval dInt = new DateInterval();
                        int Interval = 0;

                        String value = py.Loan.Mode;
                        if (value == "Semi-Monthly")
                        {
                            Interval = 15;
                            dInt = DateInterval.Day;
                        }
                        else if (value == "Weekly")
                        {
                            Interval = 7;
                            dInt = DateInterval.Day;
                        }
                        else if (value == "Daily")
                        {
                            Interval = 1;
                            dInt = DateInterval.Day;
                        }

                        
                        DateTime dt = py.DueDate;
                        dt = DateAndTime.DateAdd(dInt, Interval, dt);

                        bool isHoliday = true;
                        while (isHoliday == true || dt.Date.DayOfWeek.ToString() == "Saturday" || dt.Date.DayOfWeek.ToString() == "Sunday")
                        {
                            if (dt.Date.DayOfWeek.ToString() == "Saturday")
                            {
                                dt = DateAndTime.DateAdd(DateInterval.Day, 2, dt);
                            }
                            else if (dt.Date.DayOfWeek.ToString() == "Sunday")
                            {
                                dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                            }
                            var myC = ctx.Holidays.Where(x => x.Date.Month == dt.Date.Month && x.Date.Day == dt.Date.Day && x.isYearly == true).Count();
                            if (myC > 0)
                            {
                                dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                isHoliday = true;
                            }
                            else
                            {
                                myC = ctx.Holidays.Where(x => x.Date.Month == dt.Date.Month && x.Date.Day == dt.Date.Day && x.Date.Year == dt.Date.Year && x.isYearly == !true).Count();
                                if (myC > 0)
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                    isHoliday = true;
                                }
                                else
                                {
                                    isHoliday = false;
                                }
                            }
                        }
                        var ser = ctx.Services.Find(py.Loan.ServiceID);
                        double interest = ser.LatePaymentPenalty / 100;

                        double payment = Convert.ToDouble(amt.ToString("N2"));
                        double pbal = Convert.ToDouble((py.TotalAmount - payment).ToString("N2"));
                        double balInt = Convert.ToDouble((pbal * interest).ToString("N2"));
                        double totalBal = Convert.ToDouble((pbal + balInt).ToString("N2"));
                        double total = Convert.ToDouble((totalBal + py.Amount).ToString("N2"));
                        double rem = Convert.ToDouble((py.RemainingLoanBalance - payment).ToString("N2"));

                        py.TotalPayment = amt;
                        py.PaymentDate = DateTime.Now.Date;
                        py.PaymentStatus = "Paid";

                        int n = py.PaymentNumber + 1;

                        MPaymentInfo mp = new MPaymentInfo { Amount = py.Amount, BalanceInterest = balInt, DueDate = dt, ExcessBalance = 0, LoanID = lID, PaymentNumber = n, PaymentStatus = "Pending", PreviousBalance = pbal, RemainingLoanBalance = rem, TotalAmount = total, TotalBalance = totalBal };
                        ctx.MPaymentInfoes.Add(mp);

                        ctx.SaveChanges();
                        
                        txtID.Text = "";
                        txtID.Focus();

                    }
                }
                else
                {
                    using (var ctx = new iContext())
                    {
                        int lID = Convert.ToInt32(txtID.Text);
                        var py = ctx.MPaymentInfoes.Where(x => x.LoanID == lID && x.PaymentStatus == "Pending").First();
                        DateInterval dInt = new DateInterval();

                        double ex = amt - py.TotalAmount;

                        while (ex > 0)
                        {

                            int Interval = 0;

                            String value = py.Loan.Mode;
                            if (value == "Semi-Monthly")
                            {
                                Interval = 15;
                                dInt = DateInterval.Day;
                            }
                            else if (value == "Weekly")
                            {
                                Interval = 7;
                                dInt = DateInterval.Day;
                            }
                            else if (value == "Daily")
                            {
                                Interval = 1;
                                dInt = DateInterval.Day;
                            }


                            DateTime dt = py.DueDate;
                            dt = DateAndTime.DateAdd(dInt, Interval, dt);

                            bool isHoliday = true;
                            while (isHoliday == true || dt.Date.DayOfWeek.ToString() == "Saturday" || dt.Date.DayOfWeek.ToString() == "Sunday")
                            {
                                if (dt.Date.DayOfWeek.ToString() == "Saturday")
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 2, dt);
                                }
                                else if (dt.Date.DayOfWeek.ToString() == "Sunday")
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                }
                                var myC = ctx.Holidays.Where(x => x.Date.Month == dt.Date.Month && x.Date.Day == dt.Date.Day && x.isYearly == true).Count();
                                if (myC > 0)
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                    isHoliday = true;
                                }
                                else
                                {
                                    myC = ctx.Holidays.Where(x => x.Date.Month == dt.Date.Month && x.Date.Day == dt.Date.Day && x.Date.Year == dt.Date.Year && x.isYearly == !true).Count();
                                    if (myC > 0)
                                    {
                                        dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                        isHoliday = true;
                                    }
                                    else
                                    {
                                        isHoliday = false;
                                    }
                                }
                            }

                            double payment = Convert.ToDouble(py.Amount.ToString("N2"));
                            double rem = Convert.ToDouble((py.RemainingLoanBalance - py.TotalAmount).ToString("N2"));
                            double total = Convert.ToDouble((py.Amount - ex).ToString("N2"));
                            double ex2 = ex - py.Amount;
                            string st = "";
                            int n = py.PaymentNumber + 1;
                            MPaymentInfo mp = new MPaymentInfo();
                            if (total < 0)
                            {
                                total = py.Amount;
                                st = "Pending";
                                mp = new MPaymentInfo { Amount = py.Amount, BalanceInterest = 0, DueDate = dt, ExcessBalance = ex, LoanID = lID, PaymentNumber = n, PaymentStatus = st, PreviousBalance = 0, RemainingLoanBalance = rem, TotalAmount = total, TotalBalance = 0, PaymentDate=DateTime.Now.Date };
                            }
                            else
                            {
                                st = "Pending";
                                mp = new MPaymentInfo { Amount = py.Amount, BalanceInterest = 0, DueDate = dt, ExcessBalance = ex, LoanID = lID, PaymentNumber = n, PaymentStatus = st, PreviousBalance = 0, RemainingLoanBalance = rem, TotalAmount = total, TotalBalance = 0 };
                            }


                            py.TotalPayment = amt;
                            py.PaymentDate = DateTime.Now.Date;
                            py.PaymentStatus = "Paid";

                            amt = ex;
                            

                            
                            ctx.MPaymentInfoes.Add(mp);
                            ctx.SaveChanges();

                            txtID.Text = "";
                            txtID.Focus();
                            py = ctx.MPaymentInfoes.Where(x => x.LoanID == lID && x.PaymentStatus == "Pending").First();
                            ex = ex2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error:" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
