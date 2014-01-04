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
//addition
using LoanManagement.Domain;
using System.Data.Entity;
using MahApps.Metro.Controls;

using System.IO;
using System.Windows.Forms;
//using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

using Microsoft.VisualBasic;


namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLogin.xaml
    /// </summary>
    public partial class wpfLogin : MetroWindow
    {
        public wpfLogin()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            
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

        private void remind()
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    DateTime dt = DateTime.Today.Date.AddDays(7);
                    var lons = from lo in ctx.FPaymentInfo
                               where lo.ChequeDueDate == dt && lo.PaymentStatus=="Pending"
                               select lo;
                    string contact = "";
                    foreach (var item in lons)
                    {
                        var ctr = ctx.iTexts.Where(x => x.FPaymentInfoID == item.FPaymentInfoID).Count();
                        if (ctr == 0)
                        {
                            
                            var c1 = ctx.ClientContacts.Where(x => x.ClientID == item.Loan.ClientID).Count();
                            if (c1 > 0)
                            {
                                var c2 = ctx.ClientContacts.Where(x => x.ClientID == item.Loan.ClientID && x.Primary == true).Count();
                                if (c2 > 0)
                                {
                                    var con = ctx.ClientContacts.Where(x => x.ClientID == item.Loan.ClientID && x.Primary == true).First();
                                    contact = con.Contact;
                                }
                                else
                                {
                                    var con = ctx.ClientContacts.Where(x => x.ClientID == item.Loan.ClientID).First();
                                    contact = con.Contact;
                                }

                                MailMessage msg = new MailMessage();
                                msg.To.Add(contact + "@m2m.ph");
                                msg.From = new MailAddress("aldrinarciga@gmail.com"); //See the note afterwards...
                                msg.Body = "We would like to inform you that your next payment with an amount of (Php " + item.Amount.ToString("N2") + ") is due next week, " + dt.ToString().Split(' ')[0] + ". \nKindly settle your accounts BEFORE the due date or you can request for a hold provided paying the holding fee of (Php " + (item.Amount * (item.Loan.Service.Holding / 100)).ToString("N2") + ") ON or BEFORE " + item.ChequeDueDate.AddDays(-3).ToString().Split(' ')[0] + ", thank you.\nFrom: Financing Department";
                                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                                smtp.EnableSsl = true;
                                smtp.Port = 587;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.Credentials = new NetworkCredential("aldrinarciga@gmail.com", "312231212131");
                                smtp.Send(msg);
                                //MessageBox.Show("Message successfuly sent.");
                                iText it = new iText { FPaymentInfoID = item.FPaymentInfoID };
                                ctx.iTexts.Add(it);
                            }
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                 System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void checkState()
        {
            using (var ctx = new newerContext())
            {
                var st = ctx.State.Find(1);
                if (st.iState > 2)
                {
                    System.Windows.MessageBox.Show("System is blocked temporarily. Please contact the administrator.");
                    txtPassword.IsEnabled = false;
                    txtUsername.IsEnabled = false;
                    btnLogIn.IsEnabled = false;
                    this.Focus();
                }
                else
                {
                    txtPassword.IsEnabled = !false;
                    txtUsername.IsEnabled = !false;
                    btnLogIn.IsEnabled = !false;
                }
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
                        double iAmt = itm.TotalAmount;

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
        


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //System.Windows.MessageBox.Show("Okay");
                checkDue();
                checkExpiration();
                remind();
                /*
                using (var ctx = new newerContext())
                {
                    Domain.Position pos = new Domain.Position { PositionName = "Administrator", Description = "ForAdmins" };
                    ctx.SaveChanges();
                    Employee emp = new Employee { FirstName = "Aldrin", MI = "A", LastName = "Arciga", Email = "aldrinarciga@gmail.com", Active = true,PositionID = pos.PositionID };
                    User usr = new User();
                    usr.Username = "aldrin";
                    usr.Password = "123";
                    ctx.Users.Add(usr);
                    ctx.Employees.Add(emp);
                    ctx.SaveChanges();
                    var ctr = ctx.Users.Count();
                    if (ctr < 1)
                    { 
                        //
                    }
                }*/
                String selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                bitmap.EndInit();
                img.Source = bitmap;


                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;
                checkState();

            /*}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hi");
            //pr1.IsActive = true;
            try
            {

                if (txtUsername.Text == "")
                {
                    //MessageBox.Show("Please enter your username", "Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    FocusManager.SetFocusedElement(this, txtUsername);
                    return;
                }

                if (txtPassword.Password == "")
                {
                    //MessageBox.Show("Please enter your password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    FocusManager.SetFocusedElement(this, txtPassword);
                    return;
                }



                using (var ctx = new newerContext())
                {
                    var count = ctx.Users.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password).Count();
                    if (count > 0)
                    {
                        var em = ctx.Users.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password).First();
                        System.Windows.MessageBox.Show("Login Successful", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        wpfMain wnd = new wpfMain();

                        wnd.UserID = em.EmployeeID;

                        if (em.Employee.Department == "Financing")
                        {
                            //wnd.tbFinancing.IsEnabled = true;
                            wnd.grdM1.IsEnabled = true;
                            //wnd.tbMicro.IsEnabled = !true;
                            wnd.grdLo1.IsEnabled = !true;
                        }
                        else if (em.Employee.Department == "Micro Business")
                        {
                            //wnd.tbFinancing.IsEnabled = !true;
                            wnd.grdM1.IsEnabled = !true;
                            //wnd.tbMicro.IsEnabled = true;
                            wnd.grdLo1.IsEnabled = true;
                        }
                        else
                        {
                            //wnd.tbFinancing.IsEnabled = true;
                            wnd.grdM1.IsEnabled = true;
                            //wnd.tbMicro.IsEnabled = true;
                            wnd.grdLo1.IsEnabled = true;
                        }


                        var sc = ctx.Scopes.Find(em.EmployeeID);
                        wnd.btnMClients.IsEnabled = sc.ClientM;
                        wnd.btnMAgents.IsEnabled = sc.AgentM;
                        wnd.btnMServ.IsEnabled = sc.ServiceM;
                        wnd.btnMBank.IsEnabled = sc.BankM;
                        wnd.btnMEmployee.IsEnabled = sc.EmployeeM;
                        wnd.btnFApplication.IsEnabled = sc.Application;
                        wnd.btnMLoanApplication.IsEnabled = sc.Application;
                        wnd.btnFApproval.IsEnabled = sc.Approval;
                        wnd.btnMApproval.IsEnabled = sc.Approval;
                        wnd.btnFReleasing.IsEnabled = sc.Releasing;
                        wnd.btnMReleasing.IsEnabled = sc.Releasing;
                        wnd.btnFPayments.IsEnabled = sc.Payments;
                        wnd.btnMPayments.IsEnabled = sc.Payments;
                        wnd.btnFManage.IsEnabled = sc.ManageCLosed ;
                        wnd.btnFRestructure.IsEnabled = sc.Resturcture;
                        wnd.btnFAdjustment.IsEnabled = sc.PaymentAdjustment;
                        wnd.btnPosition.IsEnabled = sc.PositionM;

                        wnd.grdArchive.IsEnabled=sc.Archive;
                        wnd.btnUBackUp.IsEnabled=sc.BackUp;
                        wnd.btnUUser.IsEnabled = sc.UserAccounts;
                        //sc.Reports = Convert.ToBoolean(cReports.IsChecked);
                        wnd.grdStatistic.IsEnabled = sc.Statistics;
                        //sc.Scopes = Convert.ToBoolean(cUserScopes.IsChecked);

                        var st = ctx.State.Find(1);
                        st.iState = 0;

                        AuditTrail at = new AuditTrail { EmployeeID = em.EmployeeID, DateAndTime = DateTime.Now, Action = "Accessed the system" };
                        ctx.AuditTrails.Add(at);

                        ctx.SaveChanges();
                        
                        this.Close();
                        wnd.Show();
                        
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Username/Password is incorrect", "Error");
                        txtPassword.Password = "";
                        txtUsername.Text = "";
                        var st = ctx.State.Find(1);
                        st.iState = st.iState + 1;
                        ctx.SaveChanges();
                        checkState();
                        pr1.IsActive = !true;
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //return;
            }
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    var ctr = ctx.Users.Where(x => x.Username == txtUsername.Text).Count();
                    if (ctr > 0)
                    {
                        var usr = ctx.Users.Where(x => x.Username == txtUsername.Text).First();
                        byte[] imageArr;
                        imageArr = usr.Employee.Photo;
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CreateOptions = BitmapCreateOptions.None;
                        bi.CacheOption = BitmapCacheOption.Default;
                        bi.StreamSource = new MemoryStream(imageArr);
                        bi.EndInit();
                        img.Source = bi;
                    }
                    else
                    {
                        String selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                        bitmap.EndInit();
                        img.Source = bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int ascii = Convert.ToInt16(e.Key);
            if (ascii == 2)
            {
                using (var ctx = new newerContext())
                {
                    var st = ctx.State.Find(1);
                    if (st.iState > 2)
                    {
                        wpfActivate frm = new wpfActivate();
                        frm.Show();
                        this.Close();
                    }
                }
               
            }
        }
    }

    
}
