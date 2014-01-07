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
using System.IO;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;


using System.Data.Entity;
using LoanManagement.Domain;
using Microsoft.VisualBasic;

using System.Net.Mail;
using System.Net;

using MahApps.Metro.Controls;
namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfMain.xaml
    /// </summary>
    public partial class wpfMain : MetroWindow
    {
        public int UserID;
        public DateTime dt1 = DateTime.Now;
        public DateTime dt2 = DateTime.Now;

        public wpfMain()
        {
            InitializeComponent();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("asd");
        }

        private void autoCancel()
        {
            using (var ctx = new newerContext())
            {
                DateTime dt = DateTime.Now.AddMonths(-1);
                var lon = from l in ctx.ApprovedLoans
                          where l.DateApproved <= dt && l.Loan.Status == "Applied"
                          select l;

                foreach (var itm in lon)
                {
                    itm.Loan.Status = "Canceled";
                }
                ctx.SaveChanges();
            }
        }

        private void checkExpiration()
        {
            try
            {
                using (var iCtx = new newerContext())
                {
                    var l = from x in iCtx.TemporaryLoanApplications
                            where x.ExpirationDate <= DateTime.Today.Date && x.Client.isConfirmed == true
                            select x;
                    foreach (var i in l)
                    {
                        string message = "Your online loan application has been removed because you fail to confirm it. You can rapply again online but please be sure to confirm it next time. Thank you. \n\n\n\n\n -Guahan Financing Corporation Management.";
                        string email = i.Client.Email;
                        MailMessage msg = new MailMessage();
                        msg.To.Add(email);
                        msg.From = new MailAddress("aldrinarciga@gmail.com"); //See the note afterwards...
                        msg.Body = message;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential("aldrinarciga@gmail.com", "312231212131");
                        smtp.Send(msg);
                        iCtx.TemporaryLoanApplications.Remove(i);
                    }
                    iCtx.SaveChanges();
                }



                using (var ctx = new newerContext())
                {
                    var clt = from x in ctx.Clients
                              where x.isConfirmed == false && x.iClientExpiration.ExpirationDate <= DateTime.Today.Date
                              select x;

                    foreach (var itm in clt)
                    {
                        var c = ctx.TemporaryLoanApplications.Where(x => x.ClientID == itm.ClientID).Count();
                        if (c > 0)
                        {
                            var ln = ctx.TemporaryLoanApplications.Where(x => x.ClientID == itm.ClientID).First();
                            ctx.TemporaryLoanApplications.Remove(ln);
                        }
                        string message = "Your account together with your loan application(s) have been removed because you fail to confirm them. You can register again an account but please be sure to confirm it next time. Thank you. \n\n\n\n\n -Guahan Financing Corporation Management.";
                        string email = itm.Email;
                        MailMessage msg = new MailMessage();
                        msg.To.Add(email);
                        msg.From = new MailAddress("aldrinarciga@gmail.com"); //See the note afterwards...
                        msg.Body = message;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential("aldrinarciga@gmail.com", "312231212131");
                        smtp.Send(msg);
                        ctx.iClientExpirations.Remove(itm.iClientExpiration);
                        ctx.Clients.Remove(itm);
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

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                reset();
                if (lb1.SelectedIndex == 0)
                {
                    tb1.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 1)
                {
                    tb2.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 2)
                {
                    tb3.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 3)
                {
                    tb4.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 4)
                {
                    tb5.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MetroWindow_Activated_1(object sender, EventArgs e)
        {
            try
            {
                //lbM.UnselectAll();
                reset();
                checkDue();
                checkExpiration();
                autoCancel();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void reset()
        {
            try
            {
                itm1.IsSelected = false;
                itm2.IsSelected = false;
                itm3.IsSelected = false;
                itm4.IsSelected = false;
                itm5.IsSelected = false;
                itm6.IsSelected = false;
                itm7.IsSelected = false;
                itm8.IsSelected = false;
                itm9.IsSelected = false;
                itm10.IsSelected = false;
                itm11.IsSelected = false;
                itm12.IsSelected = false;
                itm13.IsSelected = false;
                itm14.IsSelected = false;
                itm15.IsSelected = false;
                itm16.IsSelected = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLogin frm = new wpfLogin();
                frm.Show();
                this.Close();
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
                //grdLo.Background = myBrush;
                //grdM.Background = myBrush;
                //grdBG.Background = myBrush;

                //System.Windows.MessageBox.Show(DateTime.Now.DayOfWeek.ToString());
                //string GuidePath = AppDomain.CurrentDomain.BaseDirectory + @"iAgents.pdf"; 
                //Uri GuideURI = new Uri(GuidePath, UriKind.Absolute);
                //wb1.Navigate(GuideURI);
                checkServices();
                checkYear();
                cmbM1.SelectedIndex = 0;
                cmbM2.SelectedIndex = 0;
                cmbY1.SelectedIndex = 0;
                cmbY2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void checkYear()
        {
            int yr = DateTime.Now.Date.Year;
            cmbY1.Items.Clear();
            cmbY2.Items.Clear();
            for (int x = 2011; x <= yr; x++)
            { 
                cmbY1.Items.Add(new ComboBoxItem{ Content = x.ToString() });
                cmbY2.Items.Add(new ComboBoxItem { Content = x.ToString() });
            }
        }

        private void ListBoxItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfBank frm = new wpfBank();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_4(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_5(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm8_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm9_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm9_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm10_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectPayment frm = new wpfSelectPayment();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm12_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectClosed frm = new wpfSelectClosed();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm13_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.iDept = "Financing";
                frm.status = "Adjustment";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm11_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Restructure";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm14_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnBank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfBank frm = new wpfBank();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAgents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoanAppllication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectPayment frm = new wpfSelectPayment();
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectClosed frm = new wpfSelectClosed();
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.UserID = UserID;
                frm.status = "Restructure";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.iDept = "Financing";
                frm.UserID = UserID;
                frm.status = "Adjustment";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoanAppllication_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Micro Business";
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnClients_Copy10_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnBank_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfBank frm = new wpfBank();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAgents_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy11_Click(object sender, RoutedEventArgs e)
        {
            wpfUsers frm = new wpfUsers();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iClients.xls";
            Microsoft.Office.Interop.Excel._Application xl = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            bool SaveChanges = false;

            try
            {
                if (File.Exists(FileName)) { File.Delete(FileName); }

                GC.Collect();

                // Create a new instance of Excel from scratch

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.Visible = false;
                wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
                sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[1]);

                // set come column heading names
                sheet.Name = "List Of Clients";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "K8"].MergeCells = true;
                sheet.Range["A8", "K8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "List of Clients";
                sheet.get_Range("A8", "K8").Font.Bold = true;
                sheet.get_Range("A8", "K8").Font.Size = 18;
                sheet.Range["A9", "K9"].MergeCells = true;
                sheet.Range["A9", "K9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
                sheet.Range["A1", "Z1"].Columns.AutoFit();
                sheet.Range["A2", "Z2"].Columns.AutoFit();
                //sheet.Cells["1:100"].Rows.AutoFit(); 
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 60, 0, 600, 100);
                sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

                sheet.Range["A10", "D10"].MergeCells = true;

                sheet.Cells[12, 1] = "Client ID";
                sheet.Cells[12, 3] = "Client Name";
                sheet.Cells[12, 5] = "Gender";
                sheet.Cells[12, 7] = "Age";
                sheet.Cells[12, 9] = "Contact";
                sheet.Cells[12, 11] = "Province";
                sheet.get_Range("A12", "K12").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A12", "K12").Font.Underline = true;

                int y = 13;

                using (var ctx = new newerContext())
                {
                    var ser = from se in ctx.Clients
                              where se.Active == true
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 1] = i.ClientID;
                        string myString = i.LastName + ", " + i.FirstName + " " + i.MiddleName + " " + i.Suffix;
                        try
                        {
                            myString = myString.Substring(0, 20);
                        }
                        catch (Exception) { myString = i.LastName + ", " + i.FirstName + " " + i.MiddleName + " " + i.Suffix; }
                        sheet.Cells[y, 3] = myString;
                        sheet.Cells[y, 5] = i.Sex;
                        string age = "0";
                        try
                        {
                            DateTime dt = Convert.ToDateTime(i.Birthday);
                            int years = DateTime.Now.Year - dt.Year;
                            if (dt.AddYears(years) > DateTime.Now);
                            years--;
                            age = years.ToString();
                        }
                        catch
                        { 
                    
                        }
                        sheet.Cells[y, 7] = age;
                        try
                        {
                            var c1 = ctx.ClientContacts.Where(x => x.ClientID == i.ClientID && x.Primary == true).Count();
                            if (c1 > 0)
                            {
                                var con = ctx.ClientContacts.Where(x => x.ClientID == i.ClientID && x.Primary == true).First();
                                sheet.Cells[y, 9] = con.Contact;
                            }
                            else
                            {
                                var con = ctx.ClientContacts.Where(x => x.ClientID == i.ClientID).First();
                                sheet.Cells[y, 9] = con.Contact;
                            }
                        }
                        catch (Exception)
                        { sheet.Cells[y, 9] = "";  }

                        try
                        {
                            var c1 = ctx.HomeAddresses.Where(x => x.ClientID == i.ClientID).First();
                            sheet.Cells[y, 11] = c1.Province;
                        }
                        catch (Exception)
                        { sheet.Cells[y, 11] = ""; }

                        
                        y++;
                    }
                    sheet.get_Range("A13", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("I13", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("K13", "K" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                }

                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)(sheet.UsedRange.Columns);
                range.AutoFit();


                // Let loose control of the Excel instance

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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iClients.pdf";
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
                MessageBox.Show(msg);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"Services.xls";
            Microsoft.Office.Interop.Excel._Application xl = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            bool SaveChanges = false;

            try
            {
                if (File.Exists(FileName)) { File.Delete(FileName); }

                GC.Collect();

                // Create a new instance of Excel from scratch

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.Visible = false;
                wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
                sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[1]);
                
                // set come column heading names
                sheet.Name = "List Of Services";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "K8"].MergeCells = true;
                sheet.Range["A8", "K8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "List of Services";
                sheet.get_Range("A8", "K8").Font.Bold = true;
                sheet.get_Range("A8", "K8").Font.Size = 18;
                sheet.Range["A9", "K9"].MergeCells = true;
                sheet.Range["A9", "K9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
                sheet.Range["A1", "Z1"].Columns.AutoFit();
                sheet.Range["A2", "Z2"].Columns.AutoFit();
                //sheet.Cells["1:100"].Rows.AutoFit(); 
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 60, 0, 600, 100);
                sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

                sheet.Range["A10", "D10"].MergeCells = true;

                sheet.Cells[12, 1] = "Service ID";
                sheet.Cells[12, 3] = "Service Name";
                sheet.Cells[12, 5] = "Type";
                sheet.Cells[12, 7] = "Department";
                sheet.Cells[12, 9] = "Interest";
                sheet.Cells[12, 11] = "Commision";
                sheet.get_Range("A12", "K12").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A12", "K12").Font.Underline = true;

                int y = 13;

                using (var ctx = new newerContext())
                {
                    var ser = from se in ctx.Services
                              where se.Active == true
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 1] = i.ServiceID;
                        sheet.Cells[y, 3] = i.Name;
                        sheet.Cells[y, 5] = i.Type;
                        sheet.Cells[y, 7] = i.Department;
                        sheet.Cells[y, 9] = i.Interest + "%";
                        sheet.Cells[y, 11] = i.AgentCommission + "%";
                        y++;
                    }
                    sheet.get_Range("A13", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("I13", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("K13", "K" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                  
                }

                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)(sheet.UsedRange.Columns);
                range.AutoFit();


                // Let loose control of the Excel instance

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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iServices.pdf";
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
                MessageBox.Show(msg);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iEmployees.xls";
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
                sheet.Name = "List Of Employees";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "K8"].MergeCells = true;
                sheet.Range["A8", "K8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "List of Employees";
                sheet.get_Range("A8", "K8").Font.Bold = true;
                sheet.get_Range("A8", "K8").Font.Size = 18;
                sheet.Range["A9", "K9"].MergeCells = true;
                sheet.Range["A9", "K9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
                sheet.Range["A1", "Z1"].Columns.AutoFit();
                sheet.Range["A2", "Z2"].Columns.AutoFit();
                //sheet.Cells["1:100"].Rows.AutoFit(); 
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 60, 0, 600, 100);
                sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

                sheet.Range["A10", "D10"].MergeCells = true;

                sheet.Cells[12, 1] = "Employee ID";
                sheet.Cells[12, 3] = "Employee Name";
                sheet.Cells[12, 5] = "Position";
                sheet.Cells[12, 7] = "Department";
                sheet.Cells[12, 9] = "Contact";
                sheet.Cells[12, 11] = "Email";
                sheet.get_Range("A12", "K12").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A12", "K12").Font.Underline = true;

                int y = 13;

                using (var ctx = new newerContext())
                {
                    var ser = from se in ctx.Employees
                              where se.Active == true
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 1] = i.EmployeeID;
                        string myString = i.LastName + ", " + i.FirstName + " " + i.MI + " " + i.Suffix;
                        try
                        {
                            myString = myString.Substring(0, 20);
                        }
                        catch (Exception) { myString = i.LastName + ", " + i.FirstName + " " + i.MI + " " + i.Suffix; }
                        sheet.Cells[y, 3] = myString;
                        sheet.Cells[y, 5] = i.Position.PositionName;
                        sheet.Cells[y, 7] = i.Department;
                        try
                        {
                            var c1 = ctx.EmployeeContacts.Where(x => x.EmployeeID == i.EmployeeID).First();
                            sheet.Cells[y, 9] = c1.Contact;
                        }
                        catch (Exception)
                        { sheet.Cells[y, 9] = ""; }

                        sheet.Cells[y, 11] = i.Email; 


                        y++;
                    }
                    sheet.get_Range("A13", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("I13", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("K13", "K" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                }

                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)(sheet.UsedRange.Columns);
                range.AutoFit();


                // Let loose control of the Excel instance

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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iEmployees.pdf";
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
                MessageBox.Show(msg);
            }
        }

        private void btnAgents_Copy1_Click(object sender, RoutedEventArgs e)
        {
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iAgents.xls";
            Microsoft.Office.Interop.Excel._Application xl = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            bool SaveChanges = false;

            try
            {
                if (File.Exists(FileName)) { File.Delete(FileName); }

                GC.Collect();

                // Create a new instance of Excel from scratch

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.Visible = false;
                wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
                sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[1]);

                // set come column heading names
                sheet.Name = "List Of Agents";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "J8"].MergeCells = true;
                sheet.Range["A8", "J8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "List of Agents";
                sheet.get_Range("A8", "J8").Font.Bold = true;
                sheet.get_Range("A8", "J8").Font.Size = 18;
                sheet.Range["A9", "J9"].MergeCells = true;
                sheet.Range["A9", "J9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
                sheet.Range["A1", "Z1"].Columns.AutoFit();
                sheet.Range["A2", "Z2"].Columns.AutoFit();
                //sheet.Cells["1:100"].Rows.AutoFit(); 
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 80, 0, 600, 100);
                sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

                sheet.Range["A10", "D10"].MergeCells = true;

                sheet.Cells[12, 1] = "Agent ID";
                sheet.Cells[12, 3] = "Agent Name";
                sheet.Cells[12, 5] = "Province";
                sheet.Cells[12, 7] = "Contact";
                sheet.Cells[12, 9] = "Email";
                sheet.get_Range("A12", "K12").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A12", "K12").Font.Underline = true;

                int y = 13;

                using (var ctx = new newerContext())
                {
                    var ser = from se in ctx.Agents
                              where se.Active == true
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 1] = i.AgentID;
                        string myString = i.LastName + ", " + i.FirstName + " " + i.MI + " " + i.Suffix;
                        try
                        {
                            myString = myString.Substring(0, 20);
                        }
                        catch (Exception) { myString = i.LastName + ", " + i.FirstName + " " + i.MI + " " + i.Suffix; }
                        sheet.Cells[y, 3] = myString;
                        try
                        {
                            var c1 = ctx.AgentAddresses.Where(x => x.AgentID == i.AgentID).First();
                            sheet.Cells[y, 5] = c1.Province;
                        }
                        catch (Exception)
                        { sheet.Cells[y, 5] = "";}

                        try
                        {
                            var c1 = ctx.AgentContacts.Where(x => x.AgentID == i.AgentID).First();
                            sheet.Cells[y, 7] = c1.Contact;
                        }
                        catch (Exception)
                        { sheet.Cells[y, 7] = ""; }

                        sheet.Cells[y, 9] = i.Email;


                        y++;
                    }
                    sheet.get_Range("A13", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("I13", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    

                }

                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)(sheet.UsedRange.Columns);
                range.AutoFit();


                // Let loose control of the Excel instance

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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iAgents.pdf";
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
                MessageBox.Show(msg);
            }
        }

        private void btnPosition_Click(object sender, RoutedEventArgs e)
        {
            wpfPosition frm = new wpfPosition();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnVLoan_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.status = "View";
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnVClient_Click(object sender, RoutedEventArgs e)
        {
            wpfClientSearch frm = new wpfClientSearch();
            frm.status = "Client";
            frm.UserID = UserID;
            frm.status2 = "View1";
            frm.ShowDialog();
        }

        private void btnClients_Copy9_Click(object sender, RoutedEventArgs e)
        {
            wpfAudtiTrail frm = new wpfAudtiTrail();
            frm.ShowDialog();
        }

        private void btnVAging_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.status = "Aging";
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnMReleasing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.UserID = UserID;
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnHoliday_Click(object sender, RoutedEventArgs e)
        {
            wpfHoliday frm = new wpfHoliday();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnMPayments_Click(object sender, RoutedEventArgs e)
        {
            wpfSelectMPayment frm = new wpfSelectMPayment();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnCPayments_Click(object sender, RoutedEventArgs e)
        {
            wpfSelectCPayments frm = new wpfSelectCPayments();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnRLoan_Click(object sender, RoutedEventArgs e)
        {
            wpfReportForLoans frm = new wpfReportForLoans();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                ComboBoxItem typeItem = (ComboBoxItem)cmbY1.SelectedItem;
                string value = typeItem.Content.ToString();
                ComboBoxItem typeItem2 = (ComboBoxItem)cmbY2.SelectedItem;
                string value2 = typeItem2.Content.ToString();
                int year1 = Convert.ToInt32(value);
                int year2 = Convert.ToInt32(value2);
                dt1 = new DateTime(year1, (cmbM1.SelectedIndex + 1), 1);
                dt2 = new DateTime(year2, (cmbM2.SelectedIndex + 1), DateTime.DaysInMonth(year2, (cmbM1.SelectedIndex + 1)));
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Select appropriate date values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dt1 > dt2)
            {
                System.Windows.MessageBox.Show("TO date must be greater than to FROM date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbDept.Text == "" || cmbTOL.Text == "")
            {
                System.Windows.MessageBox.Show("Please complete all information and select at least one data to include", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            generateStatistics();
        }

        private void cmbDept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkServices();
        }


        private void checkServices()
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    string dept = "";
                    ComboBoxItem typeItem = (ComboBoxItem)cmbDept.SelectedItem;
                    string value = typeItem.Content.ToString();

                    if (value == "Both")
                        dept = "";
                    else
                        dept = value;

                    var ser = from s in ctx.Services
                              where s.Active == true && s.Department.Contains(dept)
                              select s;
                    cmbTOL.Items.Clear();
                    cmbTOL.Items.Add(new ComboBoxItem { Content = "All" });
                    foreach (var i in ser)
                    {
                        ComboBoxItem cb = new ComboBoxItem { Content = i.Name };
                        cmbTOL.Items.Add(cb);
                    }
                }
            }
            catch (Exception)
            { return; }
        }

        private void TabItem_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            checkYear();
        }


        private void generateStatistics()
        {
            //wb1.Refresh();
            //wb1 = new WebBrowser();
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iStatistics.xls";
            Microsoft.Office.Interop.Excel._Application xl = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet2 = null;
            bool SaveChanges = false;

            //try
            //{
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
            sheet.Name = "Business Statistics";
            sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //sheet.PageSetup.RightFooter = "Page &P of &N";
            sheet.PageSetup.TopMargin = 0.5;
            sheet.PageSetup.RightMargin = 0.5;
            //sheet.Range["A2", "F2"].MergeCells = true;
            //sheet.Range["A7", "E7"].MergeCells = true;
            sheet.Range["A1", "J1"].MergeCells = true;
            sheet.Range["A1", "J1"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            sheet.Cells[1, 1] = "Business Statistics Report";
            sheet.get_Range("A1", "J1").Font.Bold = true;
            sheet.get_Range("A1", "J1").Font.Size = 18;
            //sheet.Range["A9", "J9"].MergeCells = true;
            //sheet.Range["A9", "J9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
            //sheet.Range["A1", "Z1"].Columns.AutoFit();
            //sheet.Range["A2", "Z2"].Columns.AutoFit();
            //sheet.Cells["1:100"].Rows.AutoFit(); 
            //String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
            //sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 40, 0, 600, 100);
            //sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

            //sheet.Range["A10", "D10"].MergeCells = true;
            sheet.Cells[2, 1] = "Department:  " + cmbDept.Text;
            sheet.Cells[3, 1] = "Type Of Loan  " + cmbTOL.Text;
            sheet.Cells[4, 1] = "From: " + dt1.ToString().Split(' ')[0];
            sheet.Cells[5, 1] = "To: " + dt2.ToString().Split(' ')[0];

            sheet.Cells[7, 2] = "Releases";
            sheet.Cells[7, 3] = "Collection";
            sheet.Cells[7, 4] = "Unpaid Payments";
            sheet.Cells[7, 5] = "Expected Payments";

            int y = 8;
            DateTime dt = dt1;
            ComboBoxItem typeItem2 = (ComboBoxItem)cmbDept.SelectedItem;
            string dp = typeItem2.Content.ToString();
            ComboBoxItem typeItem3 = (ComboBoxItem)cmbTOL.SelectedItem;
            string tp = typeItem3.Content.ToString();

            if (dp == "Both")
                dp = "";
            if (tp == "All")
                tp = "";
            while (dt < dt2)
            {
                sheet.Cells[y, 1] = dt.ToString("MMM") +"  " + dt.Year;
               

                double rel = 0;
                double col = 0;
                double unp = 0;
                double exp = 0;

                using (var ctx = new newerContext())
                { 
                    DateTime endOfMonth = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
                    var lns = from ln in ctx.ReleasedLoans
                              where ln.DateReleased >= dt && ln.DateReleased <= endOfMonth && (ln.Loan.Service.Department.Contains(dp) && ln.Loan.Service.Name.Contains(tp))
                              select ln;
                    foreach (var itm in lns)
                    {
                        rel = rel + itm.Principal;
                    }

                    var lns2 = from ln in ctx.ClearedCheques
                               where ln.DateCleared >= dt && ln.DateCleared <= endOfMonth && (ln.FPaymentInfo.Loan.Service.Department.Contains(dp) && ln.FPaymentInfo.Loan.Service.Name.Contains(tp))
                          select ln;
                    foreach (var itm in lns2)
                    {
                        col = col + itm.FPaymentInfo.Amount;
                    }

                    var lns3 = from ln in ctx.MPaymentInfoes
                               where ln.PaymentStatus == "Paid" && (ln.Loan.Service.Department.Contains(dp) && ln.Loan.Service.Name.Contains(tp))
                               select ln;
                    foreach (var itm in lns3)
                    {
                        if (itm.PaymentDate >= dt && itm.PaymentDate<= endOfMonth)
                            col = col + itm.TotalPayment;
                    }

                    var lns4 = from ln in ctx.MPaymentInfoes
                               where ln.PaymentStatus == "Unpaid" && (ln.Loan.Service.Department.Contains(dp) && ln.Loan.Service.Name.Contains(tp))
                               select ln;
                    foreach (var itm in lns4)
                    {
                        if (itm.DueDate >= dt && itm.DueDate <= endOfMonth)
                            unp = unp + itm.TotalAmount;
                    }

                    var lns5 = from ln in ctx.FPaymentInfo
                               where (ln.ChequeDueDate >= dt && ln.ChequeDueDate <= endOfMonth) && (ln.PaymentStatus == "Pending/Due" || ln.PaymentStatus == "Returned") && (ln.Loan.Service.Department.Contains(dp) && ln.Loan.Service.Name.Contains(tp))
                               select ln;
                    foreach (var itm in lns5)
                    {
                        unp = unp + itm.Amount;
                    }

                    var lns6 = from ln in ctx.FPaymentInfo
                               where (ln.ChequeDueDate >= dt && ln.ChequeDueDate <= endOfMonth) && (ln.PaymentStatus == "Pending" || ln.PaymentStatus == "Due" || ln.PaymentStatus == "Deposited") && (ln.Loan.Service.Department.Contains(dp) && ln.Loan.Service.Name.Contains(tp))
                               select ln;
                    foreach (var itm in lns6)
                    {
                        exp = exp + itm.Amount;
                    }

                    var lns7 = from ln in ctx.MPaymentInfoes
                               where ln.PaymentStatus == "Pending" && (ln.Loan.Service.Department.Contains(dp) && ln.Loan.Service.Name.Contains(tp))
                               select ln;
                    foreach (var itm in lns7)
                    {
                        if (itm.PaymentDate >= dt && itm.PaymentDate <= endOfMonth)
                            exp = exp + itm.TotalPayment;
                    }
                    

                }

                sheet.Cells[y, 2] = rel.ToString("N2");
                sheet.Cells[y, 3] = col.ToString("N2");
                sheet.Cells[y, 4] = unp.ToString("N2");
                sheet.Cells[y, 5] = exp.ToString("N2");
                y++;
                dt = dt.AddMonths(1);
            }
            y--;
            

            sheet.get_Range("A1", "E" + y).EntireColumn.AutoFit();
            sheet.get_Range("A7", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            sheet.get_Range("A7", "E" + y).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            Microsoft.Office.Interop.Excel.ChartObjects xlCharts = (Microsoft.Office.Interop.Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
            Microsoft.Office.Interop.Excel.ChartObject myChart = (Microsoft.Office.Interop.Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
            Microsoft.Office.Interop.Excel.Chart chartPage = myChart.Chart;
            myChart.Name = "myChart";
            Microsoft.Office.Interop.Excel.Range chartRange; 
            chartRange = sheet.get_Range("A7", "E" + y);
            object misValue = System.Reflection.Missing.Value;
            chartPage.SetSourceData(chartRange, misValue);
            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlLineMarkers;

            myChart.Left = 450;

            // Let loose control of the Excel instance

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

            

            string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iStatistics.pdf";
            XlFixedFormatType paramExportFormat = XlFixedFormatType.xlTypePDF;
            XlFixedFormatQuality paramExportQuality =
            XlFixedFormatQuality.xlQualityStandard;
            bool paramOpenAfterPublish = false;
            bool paramIncludeDocProps = true;
            bool paramIgnorePrintAreas = true;
            object paramFromPage = Type.Missing;
            object paramToPage = Type.Missing;
            //var asd = File.Create(paramExportFilePath);
            if (File.Exists(paramExportFilePath))
            {
                wb1.Navigate(new Uri("about:blank"));
                FindAndKillProcess("AcroRd32.exe");
                //asd.Close();
                //Uri GuideURI1 = new Uri(FileName, UriKind.Absolute);
                //wb1.Navigate(GuideURI1);
                try
                {
                    File.Delete(paramExportFilePath);
                }
                catch (Exception)
                {
                    wb1.Navigate(new Uri("about:blank"));
                    FindAndKillProcess("AcroRd32.exe");
                    generateStatistics();
                }
                
            }
            

            if (wb != null)
                wb.ExportAsFixedFormat(paramExportFormat,
                    paramExportFilePath, paramExportQuality,
                    paramIncludeDocProps, paramIgnorePrintAreas, paramFromPage,
                    paramToPage, paramOpenAfterPublish,
                    paramMissing);

            Uri GuideURI = new Uri(paramExportFilePath, UriKind.Absolute);
            wb1.Navigate(GuideURI);
            //asd.Flush();
            //asd.Close();
            
            GC.Collect();
            //try
            //{
                
            //}
            //catch (Exception)
            //{
            //    wb.Close(false,misValue,misValue);
            //    xl.Quit();
            //    releaseObject(wb);
            //    releaseObject(xl);
            //    
            //    generateStatistics();
            //    //wb1.Navigate(null);
            //}
            
           

            //Process xlProcess = Process.Start(paramExportFilePath);
            //}
            //catch (Exception err)
            //{
            //    String msg;
            //    msg = "Error: ";
            //    msg = String.Concat(msg, err.Message);
            //    msg = String.Concat(msg, " Line: ");
            //    msg = String.Concat(msg, err.Source);
            //    System.Windows.MessageBox.Show(msg);
            //}
        }


        public bool FindAndKillProcess(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes by using the StartsWith Method,
                //this prevents us from incluing the .EXE for the process we're looking for.
                //. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    //since we found the proccess we now need to use the
                    //Kill Method to kill the process. Remember, if you have
                    //the process running more than once, say IE open 4
                    //times the loop thr way it is now will close all 4,
                    //if you want it to just close the first one it finds
                    //then add a return; after the Kill
                    clsProcess.Kill();
                    //process killed, return true
                    return true;
                }
            }
            //process not found, return false
            return false;
        }

        private void releaseObject(object obj)
        {
            try
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnMClients_Confirmation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.UserID = UserID;
                frm.status = true;
                frm.status2 = "Confirmation";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnConfirmApplication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Confirmation";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnMConfirmApplication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Confirmation";
                frm.UserID = UserID;
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnFRenewal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectRenewal frm = new wpfSelectRenewal();
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }



    }
}
