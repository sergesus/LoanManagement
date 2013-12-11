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

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
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
    /// Interaction logic for wpfLoanApproval.xaml
    /// </summary>
    public partial class wpfLoanApproval : MetroWindow
    {

        public int lId;
        public string status;
        public int UserID;
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

                using (var ctx = new iContext())
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
                double max  = 0;
                double min = 0;
                using (var ctx = new iContext())
                { 
                    var lon = ctx.Loans.Find(lId);
                    var ser = ctx.Services.Find(lon.ServiceID);
                    max = ser.MaxValue;
                    min = ser.MinValue;
                }
                if (Convert.ToDouble(txtAmt.Text) > max || Convert.ToDouble(txtAmt.Text) < min)
                {
                    System.Windows.MessageBox.Show("Principal amount must not be greater than the maximum loanable amount OR less than the minimum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                System.Windows.MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == System.Windows.MessageBoxResult.Yes)
                {
                    using (var ctx = new iContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        if (status == "Approval")
                        {

                            lon.Status = "Approved";
                            ApprovedLoan al = new ApprovedLoan { AmountApproved = Convert.ToDouble(txtAmt.Text), DateApproved = DateTime.Today.Date, ReleaseDate = dtDate.SelectedDate.Value.Date };
                            lon.ApprovedLoan = al;
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Processed approval for loan (" + lon.Service.Name + ") for client " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.LastName + " " + lon.Client.Suffix };
                            ctx.AuditTrails.Add(at);
                            ctx.SaveChanges();
                            System.Windows.MessageBox.Show("Loan has been successfully approved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            lon.ApprovedLoan.AmountApproved = Convert.ToDouble(txtAmt.Text);
                            lon.ApprovedLoan.DateApproved = DateTime.Today.Date;
                            lon.ApprovedLoan.ReleaseDate = dtDate.SelectedDate.Value.Date;
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated approval for loan (" + lon.Service.Name + ") for client " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.LastName + " " + lon.Client.Suffix };
                            ctx.AuditTrails.Add(at);
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
                                System.Windows.MessageBox.Show("Message successfuly sent", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                System.Windows.MessageBox.Show("Incorrect Format on some Fields / Incomplete Input(s)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtAmt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
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
                using (var ctx = new iContext())
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
                    while (Remaining > -1)
                    {
                        Remaining = Remaining - Payment;
                        GenSOA soa = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = Remaining.ToString("N2") };
                        ctx.GenSOA.Add(soa);
                        num++;
                        dt = DateAndTime.DateAdd(dInt, Interval, dt);
                        if (Remaining <= Payment)
                        {
                            Payment = Remaining;
                            if (Payment < 1)
                            {
                                goto a;
                            }
                            GenSOA soa1 = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = "0.00" };
                            ctx.GenSOA.Add(soa1);
                            num++;
                            goto a;
                        }

                    }
                a:
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
            wpfViewClientInfo frm = new wpfViewClientInfo();
            frm.status = "View2";
            using (var ctx = new iContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.cID = lon.ClientID;
            }
            frm.ShowDialog();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Statement Of Account";

                PdfPage page = document.AddPage();
                page.Orientation = PageOrientation.Landscape;
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                XImage xImage = XImage.FromFile(imagePath);
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                //Header Start
                //gfx.DrawString("Guahan Financing Corporation", font, XBrushes.Black, new XRect(0, 0, page.Width, 80), XStringFormats.Center);
                //System.Windows.MessageBox.Show(xImage.Width.ToString());
                gfx.DrawImage(xImage, 40, 10, xImage.Width - 260, xImage.Height / 3);
                font = new XFont("Verdana", 18, XFontStyle.Italic);
                gfx.DrawString("Preview of Payment Schedule", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                using (var ctx = new iContext())
                {
                    font = new XFont("Verdana", 10, XFontStyle.Italic);
                    var lon = ctx.Loans.Find(lId);
                    //System.Windows.MessageBox.Show(str);
                    //gfx.DrawString(str, font, XBrushes.Black, new XRect(100,100,1000,248), XStringFormats.Center);
                    gfx.DrawString("Client: : " + lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix, font, XBrushes.Black, new XRect(0, 0, 269, 250), XStringFormats.Center);
                    gfx.DrawString("Type Of Loan : " + lon.Service.Name, font, XBrushes.Black, new XRect(0, 0, 247, 280), XStringFormats.Center);
                    gfx.DrawString("Service Type : " + lon.Service.Type, font, XBrushes.Black, new XRect(0, 0, 241, 310), XStringFormats.Center);
                    gfx.DrawString("Principal Amount : " + txtAmt.Text, font, XBrushes.Black, new XRect(0, 0, 250, 340), XStringFormats.Center);
                   

                    gfx.DrawString("Term : " + lon.Term, font, XBrushes.Black, new XRect(0, 0, 1183, 250), XStringFormats.Center);
                }
                //font = new XFont("Verdana", 10, XFontStyle.Italic);
                //gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                //Header End

                //ColumnHeader Start
                font = new XFont("Verdana", 10, XFontStyle.Bold);
                gfx.DrawString("No.", font, XBrushes.Black, new XRect(0, 0, 200, 430), XStringFormats.Center);
                gfx.DrawString("Cheque Number", font, XBrushes.Black, new XRect(0, 0, 420, 430), XStringFormats.Center);
                gfx.DrawString("Amount", font, XBrushes.Black, new XRect(0, 0, 620, 430), XStringFormats.Center);
                gfx.DrawString("Balance", font, XBrushes.Black, new XRect(0, 0, 850, 430), XStringFormats.Center);
                gfx.DrawString("Payment Date", font, XBrushes.Black, new XRect(0, 0, 1057, 430), XStringFormats.Center);
                //ColumnHeader End

                int n = 460;
                int p = 1;
                font = new XFont("Verdana", 10, XFontStyle.Regular);
                using (var ctx = new iContext())
                {
                    var clt = from cl in ctx.GenSOA
                              select cl;
                    int iNum = 0;
                    foreach (var i in clt)
                    {
                        font = new XFont("Verdana", 10, XFontStyle.Regular);
                        gfx.DrawString((iNum + 1).ToString(), font, XBrushes.Black, new XRect(0, 0, 200, n), XStringFormats.Center);
                        gfx.DrawString("-", font, XBrushes.Black, new XRect(0, 0, 420, n), XStringFormats.Center);
                        gfx.DrawString(i.Amount, font, XBrushes.Black, new XRect(0, 0, 620, n), XStringFormats.Center);
                        gfx.DrawString(i.RemainingBalance, font, XBrushes.Black, new XRect(0, 0, 850, n), XStringFormats.Center);
                        gfx.DrawString(i.PaymentDate.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, 1050, n), XStringFormats.Center);
                        iNum++;
                        n += 30;
                        if (n >= 1000)
                        {
                            gfx.DrawString("Page " + p.ToString(), font, XBrushes.Black, new XRect(0, 0, 1500, 1150), XStringFormats.Center); ;
                            page = document.AddPage();
                            page.Orientation = PageOrientation.Landscape;
                            gfx = XGraphics.FromPdfPage(page);
                            font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                            gfx.DrawImage(xImage, 40, 10, xImage.Width - 260, xImage.Height / 3);
                            font = new XFont("Verdana", 18, XFontStyle.Italic);
                            gfx.DrawString("Statement Of Account", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                            font = new XFont("Verdana", 10, XFontStyle.Italic);
                            //gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                            //ColumnHeader Start
                            font = new XFont("Verdana", 10, XFontStyle.Bold);
                            gfx.DrawString("No.", font, XBrushes.Black, new XRect(0, 0, 200, 270), XStringFormats.Center);
                            gfx.DrawString("Cheque Number", font, XBrushes.Black, new XRect(0, 0, 420, 270), XStringFormats.Center);
                            gfx.DrawString("Amount", font, XBrushes.Black, new XRect(0, 0, 620, 270), XStringFormats.Center);
                            gfx.DrawString("Balance", font, XBrushes.Black, new XRect(0, 0, 850, 270), XStringFormats.Center);
                            gfx.DrawString("Payment Date", font, XBrushes.Black, new XRect(0, 0, 1057, 270), XStringFormats.Center);
                            //ColumnHeader End
                            n = 300;
                            p++;
                        }
                    }
                    if (n < 1000)
                    {
                        gfx.DrawString("Page " + p.ToString(), font, XBrushes.Black, new XRect(0, 0, 1500, 1150), XStringFormats.Center);
                    }
                }

                //Footer Start
                font = new XFont("Verdana", 10, XFontStyle.Italic);
                string user = "";
                using (var ctx = new iContext())
                {
                    var usr = ctx.Employees.Find(UserID);
                    user = usr.LastName + ", " + usr.FirstName + " " + usr.MI + " " + usr.Suffix;
                }
                gfx.DrawString("Prepared By: " + user, font, XBrushes.Black, new XRect(0, 0, 200, 1150), XStringFormats.Center);
                //Footer End


                const string filename = "iSOA.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
