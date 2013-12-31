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
    /// Interaction logic for wpfSelectPayment.xaml
    /// </summary>
    public partial class wpfSelectPayment : MetroWindow
    {
        public int UserID;
        public wpfSelectPayment()
        {
            InitializeComponent();
        }

        private void checkDue()
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    var lon = from lo in ctx.FPaymentInfo
                              where lo.PaymentDate <= DateTime.Today.Date && (lo.PaymentStatus == "Pending" || lo.PaymentStatus == "On Hold")
                              select lo;
                    foreach (var item in lon)
                    {
                        var ctr = ctx.FPaymentInfo.Where(x => (x.PaymentDate <= DateTime.Today.Date && x.LoanID == item.LoanID) && (x.PaymentStatus == "Due" || x.PaymentStatus == "Returned" || x.PaymentStatus == "Due/Pending" || x.PaymentStatus == "Deposited")).Count();
                        if (ctr == 0)
                        {
                            item.PaymentStatus = "Due";
                        }
                        else
                        {
                            item.PaymentStatus = "Due/Pending";
                        }
                    }

                    var dep = from d in ctx.FPaymentInfo
                              where d.PaymentStatus == "Due"
                              select d;
                    foreach (var item in dep)
                    {
                        var ctr = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Deposited").Count();
                        if (ctr != 0)
                        {
                            item.PaymentStatus = "Due/Pending";
                        }
                    }

                    var lons = from lo in ctx.Loans
                               where lo.Status == "Released" && lo.Service.Department == "Financing"
                               select lo;

                    foreach (var item in lons)
                    {
                        var ctr1 = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Cleared").Count();
                        var ctr2 = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID).Count();
                        if (ctr1 == ctr2)
                        {
                            item.Status = "Paid";
                            PaidLoan pl = new PaidLoan { LoanID = item.LoanID, DateFinished = DateTime.Today.Date };
                            ctx.PaidLoans.Add(pl);
                        }
                    }

                    //MICRO

                    var mLoans = from m in ctx.MPaymentInfoes
                                 where m.DueDate < DateTime.Today.Date && m.PaymentStatus == "Pending"
                                 select m;

                    DateTime dt;
                    DateTime dt2;
                    int Interval = 0;
                    DateInterval dInt = DateInterval.Day;
                    foreach (var itm in mLoans)
                    {
                        dt = itm.DueDate;
                        itm.PaymentStatus = "Unpaid";
                        itm.TotalPayment = 0;
                        var ser = ctx.Services.Find(itm.Loan.ServiceID);
                        var iAmt = itm.TotalAmount;
                        //var ln = ctx.Loans.Find(itm.LoanID);

                        double cRem = itm.RemainingLoanBalance;

                        /*var rc = ctx.MPaymentInfoes.Where(x => x.LoanID == itm.LoanID && x.PaymentStatus == "Paid").Count();
                        if (rc > 0)
                        {
                            var re = from x in ctx.MPaymentInfoes
                                     where x.LoanID == itm.LoanID && x.PaymentStatus == "Paid"
                                     select x;
                            foreach (var item in re)
                            {
                                cRem = cRem - itm.TotalPayment;
                            }
                        }*/

                        double ciRate = ser.LatePaymentPenalty / 100;
                        double ctRate = itm.TotalAmount * ciRate;
                        double ctBalance = itm.TotalAmount;

                        //System.Windows.MessageBox.Show(ciRate.ToString());
                        //System.Windows.MessageBox.Show(ctRate.ToString());

                        int n = itm.PaymentNumber;
                        while (dt < DateTime.Today.Date)
                        {
                            String value = itm.Loan.Mode;
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

                            String str = "";

                            double tRate = ctRate;
                            str = tRate.ToString("N2");
                            tRate = Convert.ToDouble(str);
                            //System.Windows.MessageBox.Show(tRate.ToString());

                            double tBalance = ctBalance + tRate;
                            str = tBalance.ToString("N2");
                            tBalance = Convert.ToDouble(str);

                            double tAmount = itm.Amount + tBalance;
                            str = tAmount.ToString("N2");
                            tAmount = Convert.ToDouble(str);

                            ctBalance = tAmount;
                            ctRate = ctBalance * ciRate;



                            double tRem = cRem + tRate;
                            str = tRem.ToString("N2");
                            tRem = Convert.ToDouble(str);

                            cRem = tRem;

                            dt2 = DateAndTime.DateAdd(dInt, Interval, dt);
                            String st = "Unpaid";
                            if (dt2 > DateTime.Today.Date)
                                st = "Pending";
                            MPaymentInfo mpi = null;
                            if (tAmount <= tRem)
                            {
                                mpi = new MPaymentInfo { PaymentNumber = n + 1, Amount = itm.Amount, TotalBalance = tBalance, BalanceInterest = tRate, DueDate = dt, ExcessBalance = 0, LoanID = itm.LoanID, PaymentStatus = st, TotalAmount = tAmount, RemainingLoanBalance = tRem, PreviousBalance = iAmt };
                                ctx.MPaymentInfoes.Add(mpi);
                            }
                            else
                            {
                                if (itm.PaymentStatus == "Unpaid")
                                { 
                                    double tPaid = 0;
                                    var m1 = from m in ctx.MPaymentInfoes
                                             where m.LoanID == itm.LoanID && m.PaymentStatus == "Paid"
                                             select m;
                                    foreach(var i in m1)
                                    {
                                        tPaid = tPaid + i.TotalPayment;
                                    }
                                    PassedToCollector pc = new PassedToCollector { DatePassed = DateTime.Today.Date, LoanID = itm.LoanID, RemainingBalance = tRem, TotalPassedBalance = tRem, TotalPaidBeforePassing = tPaid };
                                    var l1 = ctx.Loans.Find(itm.LoanID);
                                    l1.Status = "Under Collection";
                                    ctx.PassedToCollectors.Add(pc);
                                }
                            }
                            iAmt = tAmount;
                            n++;
                            ctx.MPaymentInfoes.Add(mpi);
                        }

                    }


                    ctx.SaveChanges();
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

        private void btnHold_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Holding";
                frm.iDept = "Financing";
                frm.UserID = UserID;
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfDepositCheques frm = new wpfDepositCheques();
                frm.status = "Deposit";
                frm.UserID = UserID;
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfChequeClearing frm = new wpfChequeClearing();
                frm.UserID = UserID;
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfDepositCheques frm = new wpfDepositCheques();
                frm.status = "Returning";
                frm.UserID = UserID;
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Voiding";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            checkDue();
        }


    }
}
