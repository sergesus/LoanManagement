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

//using LoanManagement.Domain;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfDepositCheques.xaml
    /// </summary>
    public partial class wpfDepositCheques : MetroWindow
    {
        public int UserID;
        public wpfDepositCheques()
        {
            InitializeComponent();
        }

        public string state;
        public string status;

        private void checkDue()
        {
            try
            {
                using (var ctx = new newContext())
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

                            double tAmount = itm.Amount + ctBalance;
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
                            
                            MPaymentInfo mpi = new MPaymentInfo { PaymentNumber = n + 1, Amount = itm.Amount, TotalBalance = tBalance, BalanceInterest = tRate, DueDate = dt, ExcessBalance = 0, LoanID = itm.LoanID, PaymentStatus = st, TotalAmount = tAmount, RemainingLoanBalance = tRem, PreviousBalance = iAmt };
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

        public string getRow(System.Windows.Controls.DataGrid dg, int row)
        {
            try
            {
                object item = dg.SelectedItem;
                string str = (dg.SelectedCells[row].Column.GetCellContent(item) as TextBlock).Text;
                return str;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        private void rg()
        {
            try
            {
                if (rdDue.IsChecked == true)
                {
                    using (var ctx = new newContext())
                    {
                        lblDep.Content = "Deposit";
                        var chq = from ch in ctx.FPaymentInfo
                                  where ch.PaymentStatus == "Due"
                                  select new { LoanID = ch.LoanID, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate };
                        dg.ItemsSource = chq.ToList();
                        var ctr = ctx.FPaymentInfo.Where(x => x.PaymentStatus == "Due").Count();
                        String str = "Number of cheques to Deposit : " + ctr.ToString();

                        //lblTOL.Text = str.ToString();
                        //MessageBox.Show(str);
                        state = "Dep";
                        //lblDep.Content = "Deposit all cheques";
                    }
                    btnCash.Visibility = Visibility.Hidden;
                    btnNew.Visibility = Visibility.Hidden;
                }
                else if (rdDeposited.IsChecked == true)
                {
                    using (var ctx = new newContext())
                    {
                        var chq = from ch in ctx.FPaymentInfo
                                  where ch.PaymentStatus == "Deposited"
                                  select new { LoanID = ch.LoanID, No = ch.PaymentNumber, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate, DateDeposited = ch.DepositedCheque.DepositDate };
                        dg.ItemsSource = chq.ToList();
                        if (status == "Deposit"){
                            lblDep.Content = "Void";}
                        else if (status == "Returning")
                            lblDep.Content = "Mark as Returned";
                        state = "Undep";
                    }
                    btnCash.Visibility = Visibility.Hidden;
                    btnNew.Visibility = Visibility.Hidden;
                }
                else
                {
                    using (var ctx = new newContext())
                    {
                        var chq = from ch in ctx.FPaymentInfo
                                  where ch.PaymentStatus == "Returned"
                                  select new { LoanID = ch.LoanID, No = ch.PaymentNumber, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate, DateDeposited = ch.DepositedCheque.DepositDate };
                        dg.ItemsSource = chq.ToList();
                        if (status == "Deposit")
                            lblDep.Content = "Re-deposit";
                        else if (status == "Returning")
                            lblDep.Content = "Void Returning";
                        state = "Redep";
                        btnCash.Visibility = Visibility.Visible;
                        btnNew.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                rdDue.IsChecked = true;
                if (status == "Returning")
                {
                    rdDue.Visibility = Visibility.Hidden;
                    rdDeposited.IsChecked = true;
                }
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Deposit")
                {
                    if (state == "Dep")
                    {
                        MessageBoxResult mr = MessageBox.Show("Are you sure you want to deposit the selected cheque(s)?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            using (var ctx = new newContext())
                            {
                                var chq = from ch in ctx.FPaymentInfo
                                          where ch.PaymentStatus == "Due"
                                          select ch;
                                foreach (var item in chq)
                                {
                                    /*var ctr = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Due/Pending").Count();
                                    if (ctr != 0)
                                    {
                                        var hq = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Due/Pending").First();
                                        hq.PaymentStatus = "Due";
                                    }*/
                                    var ch = ctx.DepositedCheques.Where(x => x.FPaymentInfoID == item.FPaymentInfoID).Count();
                                    if (ch > 0)
                                    {
                                        item.DepositedCheque.DepositDate = DateTime.Today.Date;
                                        item.PaymentStatus = "Deposited";
                                    }
                                    else
                                    {
                                        item.PaymentStatus = "Deposited";
                                        DepositedCheque dc = new DepositedCheque { DepositDate = DateTime.Today.Date, FPaymentInfoID = item.FPaymentInfoID };
                                        ctx.DepositedCheques.Add(dc);
                                    }
                                }
                                AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Deposited Cheque(s)" };
                                ctx.AuditTrails.Add(at);

                                ctx.SaveChanges();
                                MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                rg();
                            }
                        }
                    }
                    else if (state == "Undep")
                    {
                        using (var ctx = new newContext())
                        {
                            int id = Convert.ToInt32(getRow(dg, 0));
                            int num = Convert.ToInt32(getRow(dg, 1));
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                            DepositedCheque dp = ctx.DepositedCheques.Find(fp.FPaymentInfoID);
                            MessageBoxResult mr = MessageBox.Show("Are you sure you want to void this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mr == MessageBoxResult.Yes)
                            {
                                fp.PaymentStatus = "Due";
                                ctx.DepositedCheques.Remove(dp);
                                ctx.SaveChanges();
                                MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                checkDue();
                                rg();
                                rdDue.IsChecked = true;
                            }
                        }
                    }
                    else if (state == "Redep")
                    {
                        using (var ctx = new newContext())
                        {
                            int id = Convert.ToInt32(getRow(dg, 0));
                            int num = Convert.ToInt32(getRow(dg, 1));
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                            DepositedCheque dp = ctx.DepositedCheques.Find(fp.FPaymentInfoID);
                            MessageBoxResult mr = MessageBox.Show("Are you sure you want to redeposit this cheque?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mr == MessageBoxResult.Yes)
                            {
                                fp.PaymentStatus = "Deposited";
                                dp.DepositDate = DateTime.Today.Date;
                                AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Redeposit cheque " + fp.ChequeInfo };
                                ctx.AuditTrails.Add(at);
                                ctx.SaveChanges();
                                MessageBox.Show("Transaction has been successfully processed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                checkDue();
                                rg();
                                //rdDue.IsChecked = true;
                            }
                        }
                    }
                }
                else if(status == "Returning")
                {
                    if (state == "Undep")//for returning
                    {
                        int id = Convert.ToInt32(getRow(dg, 0));
                        int num = Convert.ToInt32(getRow(dg, 1));
                        using (var ctx = new newContext())
                        {
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();

                            var ctr = ctx.ReturnedCheques.Where(x => x.FPaymentInfoID==fp.FPaymentInfoID).Count();

                            if (ctr > 0)
                            {
                                MessageBox.Show("Cheque has already been returned once", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }

                            wpfChequeReturning frm = new wpfChequeReturning();
                            frm.fId = fp.FPaymentInfoID;
                            frm.UserID = UserID;
                            frm.ShowDialog();
                        }
                    }
                    else if (state == "Redep")//for voiding
                    {
                        int id = Convert.ToInt32(getRow(dg, 0));
                        int num = Convert.ToInt32(getRow(dg, 1));
                        using (var ctx = new newContext())
                        {
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                            ReturnedCheque rc = ctx.ReturnedCheques.Find(fp.FPaymentInfoID);
                            fp.PaymentStatus = "Deposited";
                            ctx.ReturnedCheques.Remove(rc);
                            ctx.SaveChanges();
                            MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void rdDue_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void rdDeposited_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            try
            {
                rg();
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
                using (var ctx = new newContext())
                {
                    int id = Convert.ToInt32(getRow(dg, 0));
                    int num = Convert.ToInt32(getRow(dg, 1));
                    FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                    DepositedCheque dp = ctx.DepositedCheques.Find(fp.FPaymentInfoID);
                    wpfNewCheque frm = new wpfNewCheque();
                    frm.UserID = UserID;
                    frm.fId = fp.FPaymentInfoID;
                    frm.ShowDialog();
                    //frm.dId = dp.FPaymentInfoID;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnCash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new newContext())
                {
                    int id = Convert.ToInt32(getRow(dg, 0));
                    int num = Convert.ToInt32(getRow(dg, 1));
                    FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                    DepositedCheque dp = ctx.DepositedCheques.Find(fp.FPaymentInfoID);
                    wpfCheckout frm = new wpfCheckout();
                    frm.status = "PBC";//pay by cash
                    frm.fId = fp.FPaymentInfoID;
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
