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

        private void remind()
        {
            try
            {
                using (var ctx = new iContext())
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
            using (var ctx = new iContext())
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
                using (var ctx = new iContext())
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
                               where lo.Status == "Released"
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
            try
            {
                //System.Windows.MessageBox.Show("Okay");
                checkDue();
                remind();
                /*
                using (var ctx = new iContext())
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

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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



                using (var ctx = new iContext())
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
                using (var ctx = new iContext())
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
                using (var ctx = new iContext())
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
