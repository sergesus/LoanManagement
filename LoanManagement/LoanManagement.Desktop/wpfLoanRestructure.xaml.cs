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

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using System.Diagnostics;

using MahApps.Metro.Controls;
using System.Data.Entity;
using LoanManagement.Domain;
using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanRestructure.xaml
    /// </summary>
    public partial class wpfLoanRestructure : MetroWindow
    {

        public int lId;
        public int myNum;
        public System.Windows.Controls.TextBox[] textarray;
        public bool cont = false;
        public int UserID;
        public wpfLoanRestructure()
        {
            InitializeComponent();
        }

        public void reset()
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    var rmn = from rm in ctx.FPaymentInfo
                              where rm.LoanID == lId && rm.PaymentStatus == "Cleared"
                              select rm;
                    double r = 0;
                    foreach (var item in rmn)
                    {
                        r = r + item.Amount;
                    }
                    double remain = lon.ReleasedLoan.TotalLoan - r;
                    txtTerm.Text = lon.Term.ToString();
                    txtAmt.Text = remain.ToString("N2");
                    txtInt.Text = lon.Service.RestructureInterest.ToString();
                    cmbMode.Text = lon.Mode;
                    refresh();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void refresh()
        {
            try
            {
                if (Convert.ToInt16(txtTerm.Text) < 1)
                {
                    System.Windows.MessageBox.Show("Incorrect Format for term", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                using (var ctx = new finalContext())
                {
                    myNum = 0;
                    ctx.Database.ExecuteSqlCommand("delete from dbo.GenSOAs");
                    ctx.SaveChanges();
                    var lon = ctx.Loans.Find(lId);
                    Double Amt = Convert.ToDouble(txtAmt.Text);
                    lblPrincipal.Content = Amt.ToString("N2");
                    //txtAmt.Text = Amt.ToString("N2");
                    //txtAmt.SelectionStart = txtAmt.Text.Length - 3;
                    Double TotalInt = Convert.ToDouble(txtInt.Text) * Convert.ToInt32(txtTerm.Text);
                    TotalInt = TotalInt / 100;
                    /*double ded = lon.Service.AgentCommission;
                    var dec = from de in ctx.Deductions
                              where de.ServiceID == lon.ServiceID
                              select de;
                    foreach (var item in dec)
                    {
                        ded = ded + item.Percentage;
                    }
                    Double Deduction = ded / 100;
                    Double NetProceed = (Convert.ToDouble(txtAmt.Text)) - (Convert.ToDouble(txtAmt.Text) * Deduction);
                
                     */
                    Double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);
                    //lblProceed.Content = NetProceed.ToString("N2");
                    lblInt.Content = WithInt.ToString("N2");
                    Double Payment = 0;
                    DateTime dt = DateTime.Today.Date;
                    double Interval = 0;
                    DateInterval dInt = DateInterval.Month;
                    Double Remaining = WithInt;
                    ComboBoxItem typeItem = (ComboBoxItem)cmbMode.SelectedItem;
                    string value = typeItem.Content.ToString();
                    if (value == "Monthly")
                    {
                        Interval = 1;
                        dInt = DateInterval.Month;
                        Payment = WithInt / Convert.ToInt32(txtTerm.Text);
                    }
                    else if (value == "Semi-Monthly")
                    {
                        Interval = 15;
                        dInt = DateInterval.Day;
                        Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 2);
                    }
                    else if (value == "Weekly")
                    {
                        Interval = 7;
                        dInt = DateInterval.Day;
                        Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 4);
                    }
                    else if (value == "Daily")
                    {
                        Interval = 1;
                        dInt = DateInterval.Day;
                        Payment = WithInt / ((Convert.ToInt32(txtTerm.Text) * 4) * 7);
                    }
                    /*else if (cmbMode.Text == "One-Time Payment")
                    {
                        NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                        lblProceed.Content = NetProceed.ToString("N2");
                        lblInt.Content = txtAmt.Text;
                        Remaining = Convert.ToDouble(lblPrincipal.Content);
                        Payment = Remaining;
                        Interval = Convert.ToInt32(txtTerm.Text);
                        dInt = DateInterval.Month;
                        lbl4.Content = "Total Payment";
                    }*/

                    dt = DateAndTime.DateAdd(dInt, Interval, dt);
                    int num = 1;
                    while (Remaining > -1)
                    {
                        Remaining = Remaining - Payment;
                        GenSOA soa = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = Remaining.ToString("N2") };
                        ctx.GenSOA.Add(soa);
                        num++;
                        dt = DateAndTime.DateAdd(dInt, Interval, dt);
                        myNum++;
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
                            myNum++;
                            goto a;
                        }

                    }
                a:
                    lblMonthly.Content = Payment.ToString("N2");
                    ctx.SaveChanges();
                    var gen = from ge in ctx.GenSOA
                              select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                    dgSOA.ItemsSource = gen.ToList();
                    textarray = new System.Windows.Controls.TextBox[myNum];
                    System.Windows.Controls.Label[] labelarray = new System.Windows.Controls.Label[myNum];
                    StackPanel[] sp = new StackPanel[myNum];
                    stck.Children.Clear();
                    for (int ctr = 0; ctr < myNum; ctr++)
                    {
                        labelarray[ctr] = new System.Windows.Controls.Label();
                        labelarray[ctr].Height = 30;
                        //labelarray[ctr].Width = 50;
                        labelarray[ctr].FontSize = 16;
                        labelarray[ctr].Content = "Cheque No. " + (ctr + 1).ToString();
                        textarray[ctr] = new System.Windows.Controls.TextBox();
                        textarray[ctr].Height = 25;
                        textarray[ctr].Width = 300;
                        textarray[ctr].FontSize = 16;
                        textarray[ctr].MaxLength = 6;
                        sp[ctr] = new StackPanel();
                        sp[ctr].Width = 300;
                        sp[ctr].Height = 60;
                        sp[ctr].Children.Add(labelarray[ctr]);
                        sp[ctr].Children.Add(textarray[ctr]);
                        stck.Children.Add(sp[ctr]);

                        if (ctr == 0)
                        {
                            textarray[0].LostFocus += new RoutedEventHandler(txt_LostFocus);
                        }

                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int srs = Convert.ToInt32(textarray[0].Text);
                for (int x = 1; x < myNum; x++)
                {
                    srs++;
                    textarray[x].Text = srs.ToString();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
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
                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    if (lon.Service.Department == "Financing")
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "One-Time Payment" });
                    }
                    else if (lon.Service.Department == "Micro Business")
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Weekly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Daily" });
                    }
                    else
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Weekly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Daily" });
                    }

                    cmbBank.Items.Clear();
                    var ban = from ba in ctx.Banks
                              where ba.Active == true
                              select ba;
                    foreach (var item in ban)
                    {
                        cmbBank.Items.Add(item.BankName);
                    }
                    reset();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void lblInt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
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
                refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRestructure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double max = 0;
                double min = 0;
                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    var ser = ctx.Services.Find(lon.ServiceID);
                    max = ser.MaxTerm;
                    min = ser.MinTerm;


                    if (Convert.ToDouble(txtTerm.Text) > max || Convert.ToDouble(txtTerm.Text) < min)
                    {
                        System.Windows.MessageBox.Show("Term must not be greater than the maximum term(" + ser.MaxTerm + " mo.) OR less than the minimum term(" + ser.MinTerm + " mo.)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    max = ser.MaxValue;
                    min = ser.MinValue;

                    //if (Convert.ToDouble(txtAmt.Text) > max || Convert.ToDouble(txtAmt.Text) < min)
                    //{
                    //    System.Windows.MessageBox.Show("Principal amount must not be greater than the maximum loanable amount OR less than the minimum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                }

                if (Convert.ToDouble(txtAmt.Text) > Convert.ToDouble(lblPrincipal.Content))
                {
                    MessageBox.Show("Principal amount must not be greater than the maximum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var i in textarray)
                {
                    if (i.Text.Length != 6)
                    {
                        System.Windows.MessageBox.Show("Please input all cheque numbers", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    bool err;
                    int res;
                    String str = i.Text;
                    err = int.TryParse(str, out res);
                    if (err == false)
                    {
                        System.Windows.MessageBox.Show("Please input the correct format for cheque numbers(Strictly numbers only.)");
                        return;
                    }
                }

                for (int x = 0; x < textarray.Length; x++)
                {
                    for (int y = x + 1; y < textarray.Length; y++)
                    {
                        if (textarray[x].Text == textarray[y].Text)
                        {
                            System.Windows.MessageBox.Show("No duplications of cheque numbers", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }

                MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new finalContext())
                    {
                        wpfCheckout frm = new wpfCheckout();
                        var lon = ctx.Loans.Find(lId);
                        frm.lId = lId;
                        frm.status = "Restructure";
                        frm.lbl2.Content = (Convert.ToDouble(txtAmt.Text) * (lon.Service.RestructureFee / 100)).ToString("N2");
                        if(lon.Status == "Closed Account")
                        {
                            var cc = ctx.ClosedAccounts.Where(x=> x.LoanID==lId && x.isPaid == false).First();
                            frm.lbl2.Content = (Double.Parse(frm.lbl2.Content.ToString()) + cc.Fee).ToString("N2");
                        }
                        frm.ShowDialog();
                    }
                    if (cont != true)
                    {
                        MessageBox.Show("Please pay restructure fee and/or closed account fee first", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    using (var ctx = new finalContext())
                    {
                        var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                        int bId = bk.BankID;
                        var lon = ctx.Loans.Find(lId);
                        if (lon.Status == "Closed Account")
                        {
                            var cc = ctx.ClosedAccounts.Where(x => x.LoanID == lId && x.isPaid == false).First();
                            cc.isPaid = true;
                        }
                        lon.Status = "Resturctured";
                        Loan l = new Loan { AgentID = lon.AgentID, CI = 0, Term = Convert.ToInt32(txtTerm.Text), Status = "Released", ServiceID = lon.ServiceID, Mode = cmbMode.Text, CoBorrower = lon.CoBorrower, ClientID = lon.ClientID, BankID = bId };
                        ReleasedLoan rl = new ReleasedLoan { LoanID = l.LoanID, AgentsCommission = 0, DateReleased = DateTime.Today.Date, MonthlyPayment = Convert.ToDouble(lblMonthly.Content), NetProceed = 0, Principal = 0, TotalLoan = Convert.ToDouble(lblInt.Content) };
                        //RestructuredLoan rln = new RestructuredLoan { LoanID = lId, NewLoanID = l.LoanID, DateRestructured = DateTime.Today, Fee = Convert.ToDouble(txtAmt.Text) * (lon.Service.RestructureFee / 100) };
                        var fp = from f in ctx.FPaymentInfo
                                 where f.PaymentStatus != "Cleared" && f.LoanID == lId
                                 select f;
                        foreach (var item in fp)
                        {
                            item.PaymentStatus = "Void";
                        }

                        var lo = from ly in ctx.GenSOA
                                 select ly;
                        int y = 0;
                        foreach (var item in lo)
                        {
                            FPaymentInfo fpy = new FPaymentInfo { PaymentNumber = item.PaymentNumber, Amount = Convert.ToDouble(item.Amount), ChequeInfo = textarray[y].Text, LoanID = l.LoanID, ChequeDueDate = item.PaymentDate, PaymentDate = item.PaymentDate, PaymentStatus = "Pending", RemainingBalance = Convert.ToDouble(item.RemainingBalance) };
                            ctx.FPaymentInfo.Add(fpy);
                            y++;
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Processed Restructure for Loan " + lId + "" };
                        ctx.AuditTrails.Add(at);

                        ctx.Loans.Add(l);
                        ctx.ReleasedLoans.Add(rl);
                        ctx.SaveChanges();
                        RestructuredLoan rln = new RestructuredLoan { LoanID = lId, NewLoanID = l.LoanID, DateRestructured = DateTime.Today, Fee = Convert.ToDouble(txtAmt.Text) * (lon.Service.RestructureFee / 100) };
                        ctx.RestructuredLoans.Add(rln);

                        

                        ctx.SaveChanges();
                        printSOA();
                        MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }

        private void txtTerm_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void printSOP()
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
                using (var ctx = new finalContext())
                {
                    font = new XFont("Verdana", 10, XFontStyle.Italic);
                    var lon = ctx.Loans.Find(lId);
                    //System.Windows.MessageBox.Show(str);
                    //gfx.DrawString(str, font, XBrushes.Black, new XRect(100,100,1000,248), XStringFormats.Center);
                    gfx.DrawString("Client: : " + lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix, font, XBrushes.Black, new XRect(0, 0, 269, 250), XStringFormats.Center);
                    gfx.DrawString("Type Of Loan : " + lon.Service.Name, font, XBrushes.Black, new XRect(0, 0, 247, 280), XStringFormats.Center);
                    gfx.DrawString("Service Type : " + lon.Service.Type, font, XBrushes.Black, new XRect(0, 0, 241, 310), XStringFormats.Center);
                    gfx.DrawString("Principal Amount : " + txtAmt.Text, font, XBrushes.Black, new XRect(0, 0, 250, 340), XStringFormats.Center);
                    gfx.DrawString("Monthly Payment : " + lblMonthly.Content, font, XBrushes.Black, new XRect(0, 0, 240, 370), XStringFormats.Center);

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
                using (var ctx = new finalContext())
                {
                    var clt = from cl in ctx.GenSOA
                              select cl;
                    int iNum = 0;
                    foreach (var i in clt)
                    {
                        font = new XFont("Verdana", 10, XFontStyle.Regular);
                        gfx.DrawString((iNum + 1).ToString(), font, XBrushes.Black, new XRect(0, 0, 200, n), XStringFormats.Center);
                        gfx.DrawString(textarray[iNum].Text, font, XBrushes.Black, new XRect(0, 0, 420, n), XStringFormats.Center);
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
                using (var ctx = new finalContext())
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

        private void printSOA()
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

                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lId);
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

                sheet.get_Range("A11", "K14").Font.Italic = true;
                sheet.get_Range("A11", "K14").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.Cells[16, 2] = "Payment No.";
                sheet.Cells[16, 4] = "Cheque No.";
                sheet.Cells[16, 6] = "Amount";
                sheet.Cells[16, 8] = "Due Date";
                sheet.Cells[16, 10] = "Remaining Balance";
                sheet.get_Range("B16", "J16").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("B16", "J16").Font.Underline = true;

                int y = 17;

                using (var ctx = new finalContext())
                {
                    var ser = from se in ctx.GenSOA
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;

                    int iNum = 0;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 2] = i.PaymentNumber;
                        sheet.Cells[y, 4] = textarray[iNum].Text;
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

                //sheet.Range["A16", "I" + y].AutoFit();
                sheet.get_Range("B16", "J" + y).EntireColumn.AutoFit();

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

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iPaymentSchedule.xls";
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
                sheet.Name = "Preview of Payment Schedule";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "K8"].MergeCells = true;
                sheet.Range["A8", "K8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "Preview of Payment Schedule";
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

                sheet.Cells[12, 3] = "Payment No.";
                sheet.Cells[12, 5] = "Cheque No.";
                sheet.Cells[12, 7] = "Amount";
                sheet.Cells[12, 9] = "Due Date";
                sheet.Cells[12, 11] = "Remaining Balance";
                sheet.get_Range("A12", "K12").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A12", "K12").Font.Underline = true;

                int y = 13;

                using (var ctx = new finalContext())
                {
                    var ser = from se in ctx.GenSOA
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;

                    int iNum = 0;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 3] = i.PaymentNumber;
                        sheet.Cells[y, 5] = textarray[iNum].Text;
                        sheet.Cells[y, 7] = i.Amount;
                        sheet.Cells[y, 9] = i.PaymentDate.ToString().Split(' ')[0];
                        sheet.Cells[y, 11] = i.RemainingBalance;

                        iNum++;
                        y++;
                    }

                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("I13", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iPaymentSchedule.pdf";
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
    }
}
