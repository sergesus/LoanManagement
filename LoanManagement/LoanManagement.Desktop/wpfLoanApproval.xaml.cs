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

using System.Net;
using System.Net.Mail;

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
        public string status;
        public wpfLoanApproval()
        {
            InitializeComponent();
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dtDate.SelectedDate = DateTime.Today.Date;
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;

                using (var ctx = new MyLoanContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    lblDesAmt.Content = "Php " + lon.LoanApplication.AmountApplied.ToString("N2");
                    lblDesTerm.Content = lon.Term.ToString();
                    txtAmt.Text = lon.LoanApplication.AmountApplied.ToString("N2");
                    if (status == "UApproval")
                    {
                        dtDate.SelectedDate = lon.ApprovedLoan.ReleaseDate;
                        txtAmt.Text = lon.ApprovedLoan.AmountApproved.ToString("N2");
                        btnApprove.Content = "Update Approval";
                    }
                    lblName.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName;
                }
                refr();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == System.Windows.MessageBoxResult.Yes)
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        if (status == "Approval")
                        {

                            lon.Status = "Approved";
                            ApprovedLoan al = new ApprovedLoan { AmountApproved = Convert.ToDouble(txtAmt.Text), DateApproved = DateTime.Today.Date, ReleaseDate = dtDate.SelectedDate.Value.Date };
                            lon.ApprovedLoan = al;
                            ctx.SaveChanges();
                            System.Windows.MessageBox.Show("Loan Approved");
                        }
                        else
                        {
                            lon.ApprovedLoan.AmountApproved = Convert.ToDouble(txtAmt.Text);
                            lon.ApprovedLoan.DateApproved = DateTime.Today.Date;
                            lon.ApprovedLoan.ReleaseDate = dtDate.SelectedDate.Value.Date;
                            ctx.SaveChanges();
                            System.Windows.MessageBox.Show("Loan Updated");
                        }
                        try
                        {
                            mr = System.Windows.MessageBox.Show("Do you want to send a message to the client regarding this loan approval?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mr == System.Windows.MessageBoxResult.Yes)
                            {
                                var cont = ctx.ClientContacts.Where(x => x.ClientID == lon.ClientID && x.Primary == true).First();

                                string con = cont.Contact;
                                MailMessage msg = new MailMessage();
                                msg.To.Add(con + "@m2m.ph");
                                msg.From = new MailAddress("aldrinarciga@gmail.com");
                                msg.Body = "We are glad to inform you that your loan application(" + lon.Service.Name + ") has been approved with the following details: \n\n Maximum Loanable Amount : " + txtAmt.Text + " \n Release Date : " + dtDate.SelectedDate.Value.Date.Date.Date.ToString() + "";
                                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                                smtp.EnableSsl = true;
                                smtp.Port = 587;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.Credentials = new NetworkCredential("aldrinarciga@gmail.com", "312231212131");
                                smtp.Send(msg);
                                System.Windows.MessageBox.Show("Message successfuly sent.");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message.ToString());
                        }
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

        private void txtAmt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    

                    var lon = ctx.Loans.Find(lId);

                    Double Amt = Convert.ToDouble(txtAmt.Text);
                    lblPrincipal.Content = Amt.ToString("N2");
                    txtAmt.Text = Amt.ToString("N2");
                    txtAmt.SelectionStart = txtAmt.Text.Length - 3;
                    
                    Double TotalInt = lon.Service.Interest * lon.Term;
                    TotalInt = TotalInt / 100;
                    double ded = lon.Service.AgentCommission;
                    var dec = from de in ctx.Deductions
                              where de.ServiceID == lon.ServiceID
                              select de;
                    foreach (var item in dec)
                    {
                        ded = ded + item.Percentage;
                    }
                    Double Deduction = ded / 100;
                    Double NetProceed = (Convert.ToDouble(txtAmt.Text)) - (Convert.ToDouble(txtAmt.Text) * Deduction);
                    Double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);
                    if (lon.Mode == "One-Time Payment")
                    {
                        NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                        lblProceed.Content = NetProceed.ToString("N2");
                        lblInt.Content = txtAmt.Text;
                    }
                    lblProceed.Content = "Php " + NetProceed.ToString("N2");
                    lblInt.Content = "Php" + WithInt.ToString("N2");
                    if (lon.Mode == "One-Time Payment")
                    {
                        refr();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void refr()
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete from dbo.GenSOAs");
                    ctx.SaveChanges();
                    var lon = ctx.Loans.Find(lId);
                    Double Amt = Convert.ToDouble(txtAmt.Text);
                    lblPrincipal.Content = Amt.ToString("N2");
                    txtAmt.Text = Amt.ToString("N2");
                    txtAmt.SelectionStart = txtAmt.Text.Length - 3;
                    Double TotalInt = lon.Service.Interest * lon.Term;
                    TotalInt = TotalInt / 100;
                    double ded = lon.Service.AgentCommission;
                    var dec = from de in ctx.Deductions
                              where de.ServiceID == lon.ServiceID
                              select de;
                    foreach (var item in dec)
                    {
                        ded = ded + item.Percentage;
                    }
                    Double Deduction = ded / 100;
                    Double NetProceed = (Convert.ToDouble(txtAmt.Text)) - (Convert.ToDouble(txtAmt.Text) * Deduction);
                    Double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);
                    lblProceed.Content = "Php " + NetProceed.ToString("N2");
                    lblInt.Content = "Php" + WithInt.ToString("N2");
                    Double Payment = 0;
                    DateTime dt = dtDate.SelectedDate.Value.Date;
                    double Interval = 0;
                    DateInterval dInt = DateInterval.Month;
                    Double Remaining = WithInt;
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
                        Payment = WithInt / (lon.Term * 2);
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
                    else if (lon.Mode == "One-Time Payment")
                    { 
                        NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                        lblProceed.Content = NetProceed.ToString("N2");
                        lblInt.Content = txtAmt.Text;
                        Remaining = Convert.ToDouble(lblPrincipal.Content);
                        Payment = Remaining;
                        Interval = Convert.ToInt32(lblDesTerm.Content);
                        dInt = DateInterval.Month;
                    }
                    
                    dt = DateAndTime.DateAdd(dInt, Interval, dt);
                    //Double Remaining = WithInt;
                    int num = 1;
                    while (Remaining > 1)
                    {
                        //System.Windows.MessageBox.Show(num.ToString());
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
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refr();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                refr();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dtDate_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                refr();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void dtDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                refr();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            wpfClientInfo frm = new wpfClientInfo();
            frm.status = "View";
            using (var ctx = new MyLoanContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.cId = lon.ClientID;
            }
            frm.ShowDialog();
        }
    }
}
