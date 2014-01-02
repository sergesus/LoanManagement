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

using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

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
    /// Interaction logic for wpfReleasedLoanInfo.xaml
    /// </summary>
    public partial class wpfReleasedLoanInfo : MetroWindow
    {
        public int lId;
        public string status;
        public int UserID;
        public string iDept;

        public wpfReleasedLoanInfo()
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
                                    foreach (var i in m1)
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

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgentInfo frm = new wpfAgentInfo();
                frm.status = "View";
                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    frm.aId = lon.AgentID;
                }
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void reset()
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);

                    byte[] imageArr;
                    imageArr = lon.Client.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;

                    lblAmt.Content = lon.ReleasedLoan.Principal.ToString("N2");
                    string[] mdt = lon.ReleasedLoan.DateReleased.ToString().Split(' ');
                    string d = mdt[0];
                    lblDt.Content = d;
                    lblMode.Content = lon.Mode;
                    lblTOL.Content = lon.Service.Name;
                    lblType.Content = lon.Service.Type;
                    var ctr = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "Due/Pending")).Count();
                    if (ctr > 0)
                    {
                        var l = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "Due/Pending")).First();
                        lblNextP.Content = l.PaymentDate.ToString().Split(' ')[0];
                        lblPayment.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                        double hf = lon.ReleasedLoan.MonthlyPayment * (lon.Service.Holding / 100);
                        lblHF.Content = hf.ToString("N2");
                        if (status == "Holding")
                        {
                            lbl1.Content = "Hold next cheque";
                            lbl2.Content = "Unhold held cheque";
                            var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                            var dt = dts.PaymentDate.AddDays(-14);
                            var dt1 = dt.AddDays(11);
                            if (dts.PaymentStatus == "On Hold")
                            {
                                dt = dts.PaymentDate.AddDays(-5);
                                dt1 = dt.AddDays(4);
                            }
                            mdt = dt.ToString().Split(' ');
                            lblSDt.Content = mdt[0];
                            mdt = dt1.ToString().Split(' ');
                            lblEDt.Content = mdt[0];
                        }
                        else if (status == "Adjustment")
                        {
                            lbl1.Content = "Adjust Payment";
                            lbl2.Content = "Void Adjustment";
                            var ctr1 = ctx.AdjustedLoans.Where(x => x.LoanID == lId).Count();
                            if (ctr1 > 0)
                            {
                                btnUpdate.Visibility = Visibility.Hidden;
                                btnVoid.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                btnUpdate.Visibility = Visibility.Visible;
                                btnVoid.Visibility = Visibility.Hidden;
                            }

                        }
                        else if (status == "Restructure")
                        {
                            btnUpdate.Content = "Restructure Loan";
                            btnVoid.Visibility = Visibility.Hidden;
                            /*btnVoid.Content = "Void Adjustment";
                            var ctr1 = ctx.AdjustedLoans.Where(x => x.LoanID == lId).Count();
                            if (ctr1 > 0)
                            {
                                btnUpdate.Visibility = Visibility.Hidden;
                                btnVoid.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                btnUpdate.Visibility = Visibility.Visible;
                                btnVoid.Visibility = Visibility.Hidden;
                            }*/

                        }
                        else
                        {
                            lblS.Visibility = Visibility.Hidden;
                            lblE.Visibility = Visibility.Hidden;
                            lblSDt.Content = "";
                            lblEDt.Content = "";
                        }
                    }
                    else
                    {
                        lblNextP.Content = "-";
                        lblPayment.Content = "-";
                        lblHF.Content = "-";
                    }

                    var rmn = from rm in ctx.FPaymentInfo
                              where rm.LoanID == lId && rm.PaymentStatus == "Cleared"
                              select rm;
                    double r = 0;
                    foreach (var item in rmn)
                    {
                        r = r + item.Amount;
                    }
                    double remain = lon.ReleasedLoan.TotalLoan - r;
                    lblRemaining.Content = remain.ToString("N2");

                    

                    if (status != "Holding")
                    {
                        lblS.Visibility = Visibility.Hidden;
                        lblE.Visibility = Visibility.Hidden;
                        lblSDt.Content = "";
                        lblEDt.Content = "";
                    }

                    var c2 = ctx.RestructuredLoans.Where(x => x.NewLoanID == lon.LoanID).Count();
                    if (c2 > 0)
                    {
                        var lon2 = ctx.RestructuredLoans.Where(x => x.NewLoanID == lon.LoanID).First();
                        lblRes2.Content = "Amount Restructured: ";
                        lblAmt.Content = lon2.Loan.ReleasedLoan.TotalLoan;
                    }


                    checkDue();
                    var chqs = from ge in ctx.FPaymentInfo
                               where ge.LoanID == lId
                               select new { No = ge.PaymentNumber, ChequeID = ge.ChequeInfo, TotalPayment = ge.Amount, ChequeDueDate = ge.ChequeDueDate, PaymentDate = ge.PaymentDate, Status = ge.PaymentStatus, DateCleared = ge.ClearCheque.DateCleared };

                    ctx.Database.ExecuteSqlCommand("delete from dbo.ViewLoans");

                    if (iDept == "Micro Business")
                    {
                        try
                        {
                            var re1 = ctx.MPaymentInfoes.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                            double remain1 = re1.RemainingLoanBalance;
                            lblRemaining.Content = remain1.ToString("N2");
                        }
                        catch(Exception)
                        {
                            lblRemaining.Content = "0.00";
                        }

                        var pys = from p in ctx.MPaymentInfoes
                                  where p.LoanID == lId
                                  select new { No = p.PaymentNumber, Amount = p.Amount, PrevBalance = p.PreviousBalance, PrevBalanceInterest = p.BalanceInterest, TotalBalance = p.TotalBalance, ExcessiveBalance = p.ExcessBalance, TotalAmount = p.TotalAmount, DueDate = p.DueDate, RemaingBalance = p.RemainingLoanBalance, TotalPayment = p.TotalPayment, PaymentDate = p.PaymentDate, Status = p.PaymentStatus };
                        dg.ItemsSource = pys.ToList();
                    }
                    else
                    {
                        foreach (var i in chqs)
                        {
                            string s;
                            string[] dtCleared = null;
                            string myStr;
                            if (i.DateCleared != null)
                            {
                                s = i.DateCleared.ToString();
                                dtCleared = s.Split(' ');
                                myStr = dtCleared[0];
                            }
                            else
                            {
                                myStr = "";
                            }
                            s = i.PaymentDate.ToString();
                            string[] dtPayment = s.Split(' ');
                            s = i.ChequeDueDate.ToString();
                            string[] dtDue = s.Split(' ');
                            ViewLoan vl = new ViewLoan { DateCleared = myStr, DueDate = dtDue[0], PaymentDate = dtPayment[0], PaymentInfo = i.ChequeID, PaymentNumber = i.No, Status = i.Status, TotalPayment = i.TotalPayment.ToString("N2") };
                            ctx.ViewLoans.Add(vl);
                        }
                        ctx.SaveChanges();
                        var chqs1 = from ge in ctx.ViewLoans
                                    select new { No = ge.PaymentNumber, ChequeID = ge.PaymentInfo, TotalPayment = ge.TotalPayment, ChequeDueDate = ge.DueDate, PaymentDate = ge.PaymentDate, Status = ge.Status, DateCleared = ge.DateCleared };
                        dg.ItemsSource = chqs1.ToList();
                    }

                    if (status == "View")
                    {
                        btnFull.Visibility = Visibility.Hidden;
                        btnUpdate.Visibility = Visibility.Hidden;
                        btnVoid.Visibility = Visibility.Hidden;
                        
                    }

                    if (lon.Status == "Under Collection")
                    {
                        remain = lon.PassedToCollector.RemainingBalance;
                        lblRemaining.Content = remain.ToString("N2");
                        lblmd.Content = "Finished Payments: ";
                        lblMode.Content = lon.PassedToCollector.TotalPaidBeforePassing.ToString("N2");
                        lblh.Content = "Total Collection: ";
                        lblHF.Content = "-";
                        var ctr1 = ctx.CollectionInfoes.Where(x => x.LoanID == lId).Count();
                        if (ctr1 > 0)
                        {
                            var pys = from p in ctx.CollectionInfoes
                                      where p.LoanID == lId
                                      select new { TotalCollection = p.TotalCollection, DateCollected = p.DateCollected };

                            double tL = 0;
                            foreach (var itm in pys)
                            {
                                tL = tL + itm.TotalCollection;
                            }

                            lblHF.Content = tL.ToString("N2");
                            
                            dg.ItemsSource = pys.ToList();
                        }
                    }

                    if (lon.Status == "Released" || lon.Status == "Paid" || lon.Status == "Closed Account"
                        || lon.Status == "Under Collection")
                        btnSOA.Visibility = Visibility.Visible;
                    else
                        btnSOA.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
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
                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    lblName.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName;

                    iDept = lon.Service.Department;
                }
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "UReleasing")
                {
                    if (iDept == "Financing")
                    {
                        wpfFReleasing frm = new wpfFReleasing();
                        frm.status = "UReleasing";
                        frm.UserID = UserID;
                        frm.lId = lId;
                        this.Close();
                        frm.ShowDialog();
                    }
                    else
                    {
                        wpfMReleasing frm = new wpfMReleasing();
                        frm.status = "UReleasing";
                        frm.UserID = UserID;
                        frm.lId = lId;
                        this.Close();
                        frm.ShowDialog();
                    }
                }
                else if (status == "Holding")
                {
                    using (var ctx = new newerContext())
                    {
                        //var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                        if (DateTime.Today.Date > Convert.ToDateTime(lblEDt.Content) || DateTime.Today.Date < Convert.ToDateTime(lblSDt.Content))
                        {
                            System.Windows.MessageBox.Show("Unable to hold next cheque", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        /*MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                            reset();

                            System.Windows.MessageBox.Show("Okay");


                        }*/
                        wpfCheckout frm = new wpfCheckout();
                        frm.lId = lId;
                        frm.UserID = UserID;
                        frm.status = "Holding";
                        frm.ShowDialog();

                    }
                }
                else if (status == "Adjustment")
                {
                    wpfPaymentAdjustment frm = new wpfPaymentAdjustment();
                    frm.UserID = UserID;
                    frm.lId = lId;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "Restructure")
                {
                    wpfLoanRestructure frm = new wpfLoanRestructure();
                    frm.lId = lId;
                    frm.UserID = UserID;
                    this.Close();
                    frm.ShowDialog();
                }
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
                if (status == "UReleasing")
                {
                    MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to process this tranction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        if (iDept == "Financing")
                        {
                            using (var ctx = new newerContext())
                            {
                                var lon = ctx.Loans.Find(lId);
                                lon.Status = "Approved";
                                ctx.ReleasedLoans.Remove(lon.ReleasedLoan);
                                var lons = from lo in ctx.FPaymentInfo
                                           where lo.LoanID == lId
                                           select lo;
                                foreach (var item in lons)
                                {
                                    ctx.FPaymentInfo.Remove(item);
                                }
                                ctx.SaveChanges();
                                this.Close();
                                System.Windows.MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                        else
                        {
                            using (var ctx = new newerContext())
                            {
                                var lon = ctx.Loans.Find(lId);
                                lon.Status = "Approved";
                                ctx.ReleasedLoans.Remove(lon.ReleasedLoan);
                                var lons = from lo in ctx.MPaymentInfoes
                                           where lo.LoanID == lId
                                           select lo;
                                foreach (var item in lons)
                                {
                                    ctx.MPaymentInfoes.Remove(item);
                                }
                                ctx.SaveChanges();
                                this.Close();
                                System.Windows.MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                    }
                }
                else if (status == "Holding")
                {
                    using (var ctx = new newerContext())
                    {
                        var ctrs = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "On Hold").Count();
                        if (ctrs < 1)
                        {
                            System.Windows.MessageBox.Show("No cheque to unhold", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        //var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                        MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                            var dt = ctx.HeldCheques.Where(x => x.LoanID == lId && x.PaymentNumber == dts.PaymentNumber && x.NewPaymentDate == dts.PaymentDate).First();
                            dts.PaymentDate = dt.OriginalPaymentDate;
                            dts.PaymentStatus = "Pending";
                            ctx.HeldCheques.Remove(dt);
                            ctx.SaveChanges();
                            reset();

                            System.Windows.MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);


                        }

                    }
                }
                else if (status == "Adjustment")
                {
                    MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new newerContext())
                        {
                            AdjustedLoan al = ctx.AdjustedLoans.Find(lId);
                            var py = from p in ctx.FPaymentInfo
                                     where p.PaymentStatus != "Cleared" && p.LoanID == lId
                                     select p;
                            double num = al.Days * (-1);
                            foreach (var item in py)
                            {
                                item.PaymentDate = item.PaymentDate.AddDays(num);
                            }
                            ctx.AdjustedLoans.Remove(al);
                            ctx.SaveChanges();
                            System.Windows.MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            wpfViewClientInfo frm = new wpfViewClientInfo();
            frm.status = "View2";
            frm.Height = 600;
            using (var ctx = new newerContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.cID = lon.ClientID;
            }
            frm.ShowDialog();
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            reset();
        }

        private void btnSOA_Click(object sender, RoutedEventArgs e)
        {

            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iOfficialSchedule.xls";
            Microsoft.Office.Interop.Excel._Application xl = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet2 = null;
            bool SaveChanges = false;

            try
            {
                if (File.Exists(FileName)) { File.Delete(FileName); }

                GC.Collect();

                // Create a new instance of Excel from scratch

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.Visible = false;
                wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
                //sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[1]);
                sheet = wb.Worksheets.Add();
                //sheet2 = wb.Worksheets.Add();

                // set come column heading names
                sheet.Name = "Official Payment Schedule";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.PageSetup.RightMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "J8"].MergeCells = true;
                sheet.Range["A8", "J8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "Official Payment Schedule";
                sheet.get_Range("A8", "J8").Font.Bold = true;
                sheet.get_Range("A8", "J8").Font.Size = 18;
                sheet.Range["A9", "J9"].MergeCells = true;
                sheet.Range["A9", "J9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
                sheet.Range["A1", "Z1"].Columns.AutoFit();
                sheet.Range["A2", "Z2"].Columns.AutoFit();
                //sheet.Cells["1:100"].Rows.AutoFit(); 
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 40, 0, 600, 100);
                sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

                sheet.Range["A10", "D10"].MergeCells = true;

                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);

                    if (lon.Service.Department == "Financing")
                    {
                        sheet.Cells[11, 1] = "Client Name: " + lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix;
                        try
                        {
                            var agt = ctx.Agents.Find(lon.AgentID);
                            sheet.Cells[12, 1] = "Agent Name: " + agt.LastName + ", " + agt.FirstName + " " + agt.MI + " " + agt.Suffix;
                        }
                        catch (Exception) { sheet.Cells[12, 1] = "Agent Name: -"; }
                        sheet.Cells[13, 1] = "Type of Loan: " + lon.Service.Name;
                        sheet.Cells[14, 1] = "Principal Loan: " + lon.ReleasedLoan.Principal.ToString("N2");

                        var py = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentNumber == 1).First();

                        sheet.Cells[11, 10] = "First Payment: " + py.ChequeDueDate.ToString().Split(' ')[0];
                        sheet.Cells[12, 10] = "Amount: " + py.Amount.ToString("N2");
                    }
                    else
                    {
                        sheet.Cells[11, 1] = "Client Name: " + lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix;
                        try
                        {
                            var agt = ctx.Agents.Find(lon.AgentID);
                            sheet.Cells[12, 1] = "Agent Name: " + agt.LastName + ", " + agt.FirstName + " " + agt.MI + " " + agt.Suffix;
                        }
                        catch (Exception) { sheet.Cells[12, 1] = "Agent Name: -"; }
                        var py = ctx.MPaymentInfoes.Where(x => x.LoanID == lId && x.PaymentNumber == 1).First();

                        try
                        {
                            var cb = ctx.Clients.Find(lon.CoBorrower);
                            sheet.Cells[13, 1] = "Co-Borrower: " + cb.LastName + ", " + cb.FirstName + " " + cb.MiddleName + " " + cb.Suffix;
                        }
                        catch (Exception) { sheet.Cells[13, 1] = "Co-Borrower: -"; }

                        var cl = ctx.Employees.Find(lon.CollectortID);
                        sheet.Cells[14, 1] = "Collector: " + cl.LastName + " , " + cl.FirstName + " " + cl.MI + " " + cl.Suffix;
                        sheet.Cells[11, 10] = "Type of Loan: " + lon.Service.Name;
                        sheet.Cells[12, 10] = "Principal Loan: " + lon.ReleasedLoan.Principal.ToString("N2");
                        sheet.Cells[13, 10] = "First Payment: " + py.DueDate.ToString().Split(' ')[0];
                        sheet.Cells[14, 10] = "Amount: " + py.Amount.ToString("N2");
                    }

                    if (lon.Service.Department == "Financing")
                    {
                        sheet.get_Range("A11", "K14").Font.Italic = true;
                        sheet.get_Range("A11", "K14").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        sheet.Cells[16, 2] = "Payment No.";
                        sheet.Cells[16, 4] = "Cheque No.";
                        sheet.Cells[16, 6] = "Amount";
                        sheet.Cells[16, 8] = "Due Date";
                        sheet.Cells[16, 10] = "Remaining Balance";
                        sheet.get_Range("B16", "J16").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        sheet.get_Range("B16", "J16").Font.Underline = true;
                    }
                    else
                    {
                        sheet.get_Range("A11", "K14").Font.Italic = true;
                        sheet.get_Range("A11", "K14").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        sheet.Cells[16, 3] = "Payment No.";
                        sheet.Cells[16, 5] = "Amount";
                        sheet.Cells[16, 7] = "Due Date";
                        sheet.Cells[16, 9] = "Remaining Balance";
                        sheet.get_Range("B16", "J16").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        sheet.get_Range("B16", "J16").Font.Underline = true;
                    }
                }

                int y = 17;

                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);


                    var emp2 = ctx.Employees.Find(UserID);
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    if (lon.Service.Department == "Financing")
                    {
                        var ser = from se in ctx.FPaymentInfo
                                  where se.LoanID == lId
                                  select se;
                        //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                        

                        int iNum = 0;
                        foreach (var i in ser)
                        {
                            sheet.Cells[y, 2] = i.PaymentNumber;
                            sheet.Cells[y, 4] = i.ChequeInfo;
                            sheet.Cells[y, 6] = i.Amount;
                            sheet.Cells[y, 8] = i.PaymentDate.ToString().Split(' ')[0];
                            sheet.Cells[y, 10] = i.RemainingBalance;

                            iNum++;
                            y++;
                        }

                        sheet.get_Range("B17", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        sheet.get_Range("D17", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        sheet.get_Range("F17", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        sheet.get_Range("H17", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        sheet.get_Range("J17", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    }
                    else
                    {
                        var ser = from se in ctx.GenSOA
                                  select se;
                        int iNum = 0;
                        foreach (var i in ser)
                        {
                            sheet.Cells[y, 3] = i.PaymentNumber;
                            sheet.Cells[y, 5] = i.Amount;
                            sheet.Cells[y, 7] = i.PaymentDate.ToString().Split(' ')[0];
                            sheet.Cells[y, 9] = i.RemainingBalance;

                            iNum++;
                            y++;
                        }

                        sheet.get_Range("C17", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        sheet.get_Range("E17", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        sheet.get_Range("G17", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        sheet.get_Range("I17", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                        sheet.get_Range("B16", "J" + y).EntireColumn.AutoFit();
                    }

                    //sheet.Range["A16", "I" + y].AutoFit();
                    sheet.get_Range("B16", "J" + y).EntireColumn.AutoFit();

                    // Let loose control of the Excel instance
                }
                xl.Visible = false;
                xl.UserControl = false;
                xl.StandardFont = "Segoe UI";
                xl.StandardFontSize = 12;



                // Set a flag saying that all is well and it is ok to save our changes to a file.

                SaveChanges = true;

                //  Save the file to disk
                sheet.Protect();

                wb.SaveAs(FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                          null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                          false, false, null, null, null);
                object paramMissing = Type.Missing;

                wb = xl.Workbooks.Open(FileName,
                    paramMissing, paramMissing, paramMissing, paramMissing,
                    paramMissing, paramMissing, paramMissing, paramMissing,
                    paramMissing, paramMissing, paramMissing, paramMissing,
                    paramMissing, paramMissing);

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iOfficialSchedule.pdf";
                XlFixedFormatType paramExportFormat = XlFixedFormatType.xlTypePDF;
                XlFixedFormatQuality paramExportQuality =
                XlFixedFormatQuality.xlQualityStandard;
                bool paramOpenAfterPublish = false;
                bool paramIncludeDocProps = true;
                bool paramIgnorePrintAreas = true;
                object paramFromPage = Type.Missing;
                object paramToPage = Type.Missing;

                if (wb != null)
                    wb.ExportAsFixedFormat(paramExportFormat,
                        paramExportFilePath, paramExportQuality,
                        paramIncludeDocProps, paramIgnorePrintAreas, paramFromPage,
                        paramToPage, paramOpenAfterPublish,
                        paramMissing);

                Process xlProcess = Process.Start(paramExportFilePath);
            }
            catch (Exception err)
            {
                String msg;
                msg = "Error: ";
                msg = String.Concat(msg, err.Message);
                msg = String.Concat(msg, " Line: ");
                msg = String.Concat(msg, err.Source);
                System.Windows.MessageBox.Show(msg);
            }
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string folderName = @"F:\Loan Files";
                string pathString = System.IO.Path.Combine(folderName, "Loan " + lId.ToString());
                if (!Directory.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(pathString);
                    Process.Start(@"F:\Loan Files\Loan " + lId.ToString());
                }
                else
                {
                    Process.Start(@"F:\Loan Files\Loan " + lId.ToString());
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
