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
    /// Interaction logic for wpfFReleasing.xaml
    /// </summary>
    public partial class wpfFReleasing : MetroWindow
    {
        public string status;
        public int lId;
        public int bId;
        public int myNum;
        public System.Windows.Controls.TextBox[] textarray = new System.Windows.Controls.TextBox[0];
        public int UserID;
        public wpfFReleasing()
        {
            InitializeComponent();
        }

        private void refresh()
        {
            try
            {
                if (status == "UReleasing")
                {
                    using (var ctx = new newContext())
                    {
                        var lons = from ge in ctx.FPaymentInfo
                                   where ge.LoanID == lId
                                   select ge;
                        myNum = ctx.FPaymentInfo.Where(x => x.LoanID == lId).Count();
                    }
                }

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
                    if (status == "Releasing")
                    {
                        if (ctr == 0)
                        {
                            textarray[0].LostFocus += new RoutedEventHandler(txt_LostFocus);   
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

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int srs = Convert.ToInt32(textarray[0].Text);
                for (int x = 1; x < myNum;x++ )
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

        private void refr()
        {
            try
            {
                if (Convert.ToInt16(txtTerm.Text) < 1)
                {
                    System.Windows.MessageBox.Show("Incorrect Format for term", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (status == "Releasing")
                {
                    using (var ctx = new newContext())
                    {
                        myNum = 0;
                        ctx.Database.ExecuteSqlCommand("delete from dbo.GenSOAs");
                        ctx.SaveChanges();
                        var lon = ctx.Loans.Find(lId);
                        Double Amt = Convert.ToDouble(txtAmt.Text);
                        //lblPrincipal.Content = Amt.ToString("N2");
                        txtAmt.Text = Amt.ToString("N2");
                        txtAmt.SelectionStart = txtAmt.Text.Length - 3;
                        Double TotalInt = lon.Service.Interest * Convert.ToInt32(txtTerm.Text);
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
                        lblProceed.Content = NetProceed.ToString("N2");
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
                            lbl4.Content = "Monthly Payment";
                        }
                        else if (value == "Semi-Monthly")
                        {
                            Interval = 15;
                            dInt = DateInterval.Day;
                            Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 2);
                            lbl4.Content = "Semi-Monthly Payment";
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
                        else if (value == "One-Time Payment")
                        {
                            NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                            lblProceed.Content = NetProceed.ToString("N2");
                            lblInt.Content = txtAmt.Text;
                            Remaining = Convert.ToDouble(lblPrincipal.Content);
                            Payment = Remaining;
                            Interval = Convert.ToInt32(txtTerm.Text);
                            dInt = DateInterval.Month;
                            lbl4.Content = "Total Payment";
                        }

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
                        refresh();
                        return;
                    }
                }
                else if (status == "UReleasing")
                {
                    using (var ctx = new newContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var lons = from ge in ctx.FPaymentInfo
                                   where ge.LoanID == lId
                                   select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                        dgSOA.ItemsSource = lons.ToList();
                        txtAmt.Text = lon.ReleasedLoan.TotalLoan.ToString("N2");
                        lblMonthly.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                        lblPrincipal.Content = txtAmt.Text;
                        lblProceed.Content = lon.ReleasedLoan.NetProceed.ToString("N2");
                        myNum = ctx.FPaymentInfo.Where(x => x.LoanID == lId).Count();
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.SelectedItem = lon.Mode;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Incorrect Format on some Fields / Incomplete Input(s)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            //printSOA();
            try
            {
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;

                //num = 0;
                if (status == "Releasing")
                {
                    using (var ctx = new newContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var ser = ctx.Services.Find(lon.ServiceID);
                        cmbMode.Items.Clear();
                        Double Amt = Convert.ToDouble(lon.ApprovedLoan.AmountApproved);
                        lblPrincipal.Content = Amt.ToString("N2");
                        txtAmt.Text = Amt.ToString("N2");
                        if (ser.Department == "Financing")
                        {
                            cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                            cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                            cmbMode.Items.Add(new ComboBoxItem { Content = "One-Time Payment" });
                        }
                        else if (ser.Department == "Micro Business")
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
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.Text = lon.Mode;
                        refr();
                        cmbBank.Items.Clear();
                        var ban = from ba in ctx.Banks
                                  where ba.Active == true
                                  select ba;
                        foreach (var item in ban)
                        {
                            cmbBank.Items.Add(item.BankName);
                        }
                        //MessageBox.Show(myNum.ToString());
                        refresh();
                    }
                }
                else if (status == "UReleasing")
                {

                    txtAmt.IsEnabled = false;
                    txtTerm.IsEnabled = false;
                    cmbMode.IsEnabled = false;
                    btnRef.IsEnabled = false;
                    btnRelease.Content = "Update";
                    refresh();
                    refr();
                    int myCtr = 0;
                    using (var ctx = new newContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var bnk = ctx.Banks.Find(lon.BankID);
                        cmbBank.Items.Clear();
                        var ban = from ba in ctx.Banks
                                  where ba.Active == true
                                  select ba;
                        foreach (var item in ban)
                        {
                            cmbBank.Items.Add(item.BankName);
                        }
                        //cmbBank.SelectedIndex = 0;
                        cmbBank.Text = bnk.BankName;
                        cmbMode.Items.Clear();
                        cmbMode.Items.Add(lon.Mode);
                        cmbMode.Text = lon.Mode;
                        var lons = from lo in ctx.FPaymentInfo
                                   where lo.LoanID == lId
                                   select lo;
                        foreach (var item in lons)
                        {
                            //MessageBox.Show(myCtr.ToString());
                            textarray[myCtr].Text = item.ChequeInfo;
                            myCtr++;
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

        private void cmbBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*using (var ctx = new newContext())
            { 
                ComboBoxItem typeItem = (ComboBoxItem)cmbBank.SelectedItem;
                string value = typeItem.Content.ToString();
                var ban = ctx.Banks.Where(x => x.BankName == value).First();
                bId = ban.BankID;
            }*/
        }

        private void cmbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void cmbMode_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cmbMode_DropDownClosed(object sender, EventArgs e)
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

        private void txtTerm_LostFocus(object sender, RoutedEventArgs e)
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

        private void printSOA() 
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
                gfx.DrawString("Statement Of Account", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                using (var ctx = new newContext())
                {
                    font = new XFont("Verdana", 10, XFontStyle.Italic);
                    var lon = ctx.Loans.Find(lId);
                    string str = "Client: : " + lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix
                        + "\nType Of Loan : " + lon.Service.Name + "\nService Type : " + lon.Service.Type + "\nPrincipal Amount : " + lon.ReleasedLoan.Principal.ToString("N2")
                        + "\nMonthly Payment : " + lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                    //System.Windows.MessageBox.Show(str);
                    //gfx.DrawString(str, font, XBrushes.Black, new XRect(100,100,1000,248), XStringFormats.Center);
                    gfx.DrawString("Client: : " + lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix, font, XBrushes.Black, new XRect(0, 0, 269, 250), XStringFormats.Center);
                    gfx.DrawString("Type Of Loan : "+ lon.Service.Name, font, XBrushes.Black, new XRect(0, 0, 247, 280), XStringFormats.Center);
                    gfx.DrawString("Service Type : " + lon.Service.Type, font, XBrushes.Black, new XRect(0, 0, 241, 310), XStringFormats.Center);
                    gfx.DrawString("Principal Amount : " + lon.ReleasedLoan.Principal.ToString("N2"), font, XBrushes.Black, new XRect(0, 0, 250, 340), XStringFormats.Center);
                    gfx.DrawString("Monthly Payment : " + lon.ReleasedLoan.MonthlyPayment.ToString("N2"), font, XBrushes.Black, new XRect(0, 0, 240, 370), XStringFormats.Center);
                    
                    gfx.DrawString("Term : " + lon.Term, font, XBrushes.Black, new XRect(0, 0, 1183, 250), XStringFormats.Center);
                    gfx.DrawString("Mode of Payment : " + lon.Mode, font, XBrushes.Black, new XRect(0, 0, 1300, 280), XStringFormats.Center);
                    var bk = ctx.Banks.Find(lon.BankID);
                    gfx.DrawString("Bank : " + bk.BankName, font, XBrushes.Black, new XRect(0, 0, 1237, 310), XStringFormats.Center);
                    var fl = ctx.FPaymentInfo.Where(x => x.LoanID == lId).First();
                    gfx.DrawString("Payment Start : " + fl.PaymentDate.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, 1270, 340), XStringFormats.Center);
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
                using (var ctx = new newContext())
                {
                    var clt = from cl in ctx.FPaymentInfo
                              where cl.LoanID == lId
                              select cl;

                    foreach (var i in clt)
                    {
                        font = new XFont("Verdana", 10, XFontStyle.Regular);
                        gfx.DrawString(i.PaymentNumber.ToString(), font, XBrushes.Black, new XRect(0, 0, 200, n), XStringFormats.Center);
                        gfx.DrawString(i.ChequeInfo, font, XBrushes.Black, new XRect(0, 0, 420, n), XStringFormats.Center);
                        gfx.DrawString(i.Amount.ToString("N2"), font, XBrushes.Black, new XRect(0, 0, 620, n), XStringFormats.Center);
                        gfx.DrawString(i.RemainingBalance.ToString("N2"), font, XBrushes.Black, new XRect(0, 0, 850, n), XStringFormats.Center);
                        gfx.DrawString(i.ChequeDueDate.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, 1050, n), XStringFormats.Center);

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
                using (var ctx = new newContext())
                {
                    var usr = ctx.Employees.Find(UserID);
                    user = usr.LastName + ", " + usr.FirstName + " " + usr.MI + " " + usr.Suffix;
                }
                gfx.DrawString("Prepared By: " + user, font, XBrushes.Black, new XRect(0, 0, 200, 1150), XStringFormats.Center);
                //Footer End


                const string filename = "SOA.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Releasing")
                {
                    double max = 0;
                    double min = 0;
                    using (var ctx = new newContext())
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

                        if (Convert.ToDouble(txtAmt.Text) > max || Convert.ToDouble(txtAmt.Text) < min)
                        {
                            System.Windows.MessageBox.Show("Principal amount must not be greater than the maximum loanable amount OR less than the minimum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
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
                            System.Windows.MessageBox.Show("Please input the correct format for cheque numbers(Strictly numbers only.)", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new newContext())
                        {
                            var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                            int bId = bk.BankID;
                            var lon = ctx.Loans.Find(lId);
                            lon.Status = "Released";
                            lon.BankID = bId;
                            lon.Mode = cmbMode.Text;
                            lon.Term = Convert.ToInt32(txtTerm.Text);
                            var cn = ctx.Services.Find(lon.ServiceID);
                            double co = cn.AgentCommission / 100;
                            double cm = Convert.ToDouble(txtAmt.Text) * co;
                            //MessageBox.Show(cm.ToString());
                            ReleasedLoan rl = new ReleasedLoan { AgentsCommission = cm, DateReleased = DateTime.Today.Date, LoanID = lId, MonthlyPayment = Convert.ToDouble(lblMonthly.Content), NetProceed = Convert.ToDouble(lblProceed.Content), Principal = Convert.ToDouble(txtAmt.Text), TotalLoan = Convert.ToDouble(lblInt.Content) };
                            lon.ReleasedLoan = rl;
                            var lo = from l in ctx.GenSOA
                                     select l;
                            int y = 0;
                            foreach (var item in lo)
                            {
                                FPaymentInfo fp = new FPaymentInfo { PaymentNumber = item.PaymentNumber, Amount = Convert.ToDouble(item.Amount), ChequeInfo = textarray[y].Text, LoanID = lId, ChequeDueDate = item.PaymentDate, PaymentDate = item.PaymentDate, PaymentStatus = "Pending", RemainingBalance = Convert.ToDouble(item.RemainingBalance) };
                                ctx.FPaymentInfo.Add(fp);
                                y++;
                            }
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Released loan (" + lon.Service.Name + ") for client " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.LastName + " " + lon.Client.Suffix };
                            ctx.AuditTrails.Add(at);
                            ctx.SaveChanges();
                            printSOA();
                            MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                    }
                }
                else if (status == "UReleasing")
                {
                    int myCtr = 0;
                    using (var ctx = new newContext())
                    {
                        var lons = from lo in ctx.FPaymentInfo
                                   where lo.LoanID == lId
                                   select lo;
                        foreach (var item in lons)
                        {
                            item.ChequeInfo = textarray[myCtr].Text;
                            myCtr++;
                        }
                        var lon = ctx.Loans.Find(lId);
                        var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                        int bId = bk.BankID;
                        lon.BankID = bId;

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Released loan (" + lon.Service.Name + ") for client " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.LastName + " " + lon.Client.Suffix };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        MessageBox.Show("Transaction has been successfully updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void txtAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            refr();
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

                using (var ctx = new newContext())
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
