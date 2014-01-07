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

                using (var ctx = new newerContext())
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
                using (var ctx = new newerContext())
                { 
                    var lon = ctx.Loans.Find(lId);
                    var ser = ctx.Services.Find(lon.ServiceID);
                    max = ser.MaxValue;
                    min = ser.MinValue;
                    double val = Convert.ToDouble(txtAmt.Text);
                    if (status == "Renewal")
                    {
                        if (val < lon.ReleasedLoan.TotalLoan)
                        {
                            System.Windows.MessageBox.Show("Amount must be greater than or equal the Total Loan Amount of Previous loan(" + lon.ReleasedLoan.TotalLoan.ToString("N2") + ")", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }
                if (Convert.ToDouble(txtAmt.Text) > max || Convert.ToDouble(txtAmt.Text) < min)
                {
                    System.Windows.MessageBox.Show("Principal amount must not be greater than the maximum loanable amount OR less than the minimum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string qst = "Are you sure you want to process this transaction?";

                using (var ctx = new newerContext())
                {
                    var c1 = ctx.RequirementChecklists.Where(x => x.LoanID == lId).Count();
                    var lon = ctx.Loans.Find(lId);
                    var c2 = ctx.Requirements.Where(x => x.ServiceID == lon.ServiceID).Count();
                    if (c1 != c2)
                        qst = "Are you sure you want to process this loan even with incomplete requirements?";
                }


                System.Windows.MessageBoxResult mr = System.Windows.MessageBox.Show(qst, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == System.Windows.MessageBoxResult.Yes)
                {
                    using (var ctx = new newerContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        if (status == "Approval" || status == "Renewal Approval")
                        {

                            lon.Status = "Approved";
                            if (status == "Renewal Approval")
                            {
                                lon.Status = "Approved for Renewal";
                                var rn = ctx.LoanRenewals.Where(x => x.newLoanID == lId).First();
                                rn.Status = "Approved";
                            }
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
                using (var ctx = new newerContext())
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
                using (var ctx = new newerContext())
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
                    int num = 1;
                    double p2 = Payment;
                    while (Remaining > -1)
                    {
                        Remaining = Remaining - Payment;
                        GenSOA soa = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = Remaining.ToString("N2") };
                        ctx.GenSOA.Add(soa);
                        num++;
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
            using (var ctx = new newerContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.cID = lon.ClientID;
            }
            frm.ShowDialog();
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
                //sheet.Cells[12, 5] = "Cheque No.";
                sheet.Cells[12, 5] = "Amount";
                sheet.Cells[12, 7] = "Due Date";
                sheet.Cells[12, 9] = "Remaining Balance";
                sheet.get_Range("A12", "K12").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A12", "K12").Font.Underline = true;

                int y = 13;

                using (var ctx = new newerContext())
                {
                    var ser = from se in ctx.GenSOA
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    foreach (var i in ser)
                    {
                        sheet.Cells[y, 3] = i.PaymentNumber;
                        //sheet.Cells[y, 5] = "-";
                        sheet.Cells[y, 5] = i.Amount;
                        sheet.Cells[y, 7] = i.PaymentDate.ToString().Split(' ')[0];
                        sheet.Cells[y, 9] = i.RemainingBalance;


                        y++;
                    }
                    
                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("I13", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    //sheet.get_Range("K13", "K" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

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
