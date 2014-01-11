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

using MahApps.Metro.Controls;
using System.Data.Entity;
using LoanManagement.Domain;
using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfMReleasing.xaml
    /// </summary>
    public partial class wpfMReleasing : MetroWindow
    {
        public string status;
        public int lId;
        public int UserID;
        public int ciId = 0;

        public wpfMReleasing()
        {
            InitializeComponent();
        }

        private void refr()
        {
            //try
            //{
                

                if (status == "Releasing")
                {
                    using (var ctx = new newerContext())
                    {
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
                        lblMonthly.Content = Payment.ToString("N2");
                        ctx.SaveChanges();
                        var gen = from ge in ctx.GenSOA
                                  select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                        dgSOA.ItemsSource = gen.ToList();
                        var c1 = ctx.GenSOA.Count();
                        p2 = Convert.ToDouble(p2.ToString("N2"));
                        //System.Windows.MessageBox.Show(p2.ToString());
                        lblInt.Content = (p2 * c1).ToString("N2");
                        return;
                    }
                }
                else if (status == "UReleasing")
                {
                    using (var ctx = new newerContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var pys = from p in ctx.MPaymentInfoes
                                  where p.LoanID == lId
                                  select new { No = p.PaymentNumber, Amount = p.Amount, PrevBalance = p.PreviousBalance, PrevBalanceInterest = p.BalanceInterest, TotalBalance = p.TotalBalance, TotalAmount = p.TotalAmount, DueDate = p.DueDate, RemaingBalance = p.RemainingLoanBalance, Status = p.PaymentStatus };
                        dgSOA.ItemsSource = pys.ToList();
                        txtAmt.Text = lon.ReleasedLoan.TotalLoan.ToString("N2");
                        lblMonthly.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                        lblPrincipal.Content = txtAmt.Text;
                        lblProceed.Content = lon.ReleasedLoan.NetProceed.ToString("N2");
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.Items.Add(lon.Mode);
                        cmbMode.SelectedItem = lon.Mode;
                        var agt = ctx.Employees.Find(lon.CollectortID);
                        ciId = lon.CollectortID;
                        String str = "(" + ciId + ")" + agt.FirstName + " " + agt.MI + " " + agt.LastName;
                        txtCI.Text = str;
                       
                    }
                }
                if (Convert.ToInt16(txtTerm.Text) < 1)
                {
                    System.Windows.MessageBox.Show("Incorrect Format for term", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            /*}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Incorrect Format on some Fields / Incomplete Input(s)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/
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

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
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
                    using (var ctx = new newerContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var ser = ctx.Services.Find(lon.ServiceID);
                        cmbMode.Items.Clear();
                        Double Amt = Convert.ToDouble(lon.ApprovedLoan.AmountApproved);
                        lblPrincipal.Content = Amt.ToString("N2");
                        txtAmt.Text = Amt.ToString("N2");
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Weekly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Daily" });
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.Text = lon.Mode;
                        refr();
                    }
                }
                else if (status == "UReleasing")
                {

                    txtAmt.IsEnabled = false;
                    txtTerm.IsEnabled = false;
                    cmbMode.IsEnabled = false;
                    btnRef.IsEnabled = false;
                    btnRelease.Content = "Update";
                    refr();
                }
            /*}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Releasing")
                {
                    double max = 0;
                    double min = 0;
                    using (var ctx = new newerContext())
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

                    if(ciId == 0)
                    {
                        MessageBox.Show("Please assign a collector for this loan", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new newerContext())
                        {
                            int bId = 0;
                            var lon = ctx.Loans.Find(lId);
                            lon.Status = "Released";
                            lon.Mode = cmbMode.Text;
                            lon.Term = Convert.ToInt32(txtTerm.Text);
                            lon.CollectortID = ciId;
                            var cn = ctx.Services.Find(lon.ServiceID);
                            double co = cn.AgentCommission / 100;
                            double cm = Convert.ToDouble(txtAmt.Text) * co;
                            //MessageBox.Show(cm.ToString());
                            ReleasedLoan rl = new ReleasedLoan { AgentsCommission = cm, DateReleased = DateTime.Today.Date, LoanID = lId, MonthlyPayment = Convert.ToDouble(lblMonthly.Content), NetProceed = Convert.ToDouble(lblProceed.Content), Principal = Convert.ToDouble(txtAmt.Text), TotalLoan = Convert.ToDouble(lblInt.Content) };
                            lon.ReleasedLoan = rl;
                            var lo = ctx.GenSOA.Where(x => x.PaymentNumber == 1).First();
                            int y = 0;
                            MPaymentInfo py = new MPaymentInfo { Amount = Convert.ToDouble(lo.Amount), BalanceInterest = 0, DueDate = lo.PaymentDate, LoanID = lId, PaymentNumber = 1, PaymentStatus = "Pending", PreviousBalance = 0, RemainingLoanBalance = Convert.ToDouble(lo.RemainingBalance), TotalAmount = Convert.ToDouble(lo.Amount), TotalBalance = 0 };
                            ctx.MPaymentInfoes.Add(py);
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
                    
                    using (var ctx = new newerContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        lon.CollectortID = ciId;

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "Collector";
                //frm.cId = ciId;
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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

                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);
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
                    catch(Exception){sheet.Cells[13, 1] = "Co-Borrower: -";}

                    var cl = ctx.Employees.Find(lon.CollectortID);
                    sheet.Cells[14, 1] = "Collector: " + cl.LastName + " , " +  cl.FirstName + " " + cl.MI + " " + cl.Suffix;
                    sheet.Cells[11, 10] = "Type of Loan: " + lon.Service.Name;
                    sheet.Cells[12, 10] = "Principal Loan: " + lon.ReleasedLoan.Principal.ToString("N2");
                    sheet.Cells[13, 10] = "First Payment: " + py.DueDate.ToString().Split(' ')[0];
                    sheet.Cells[14, 10] = "Amount: " + py.Amount.ToString("N2");
                }

                sheet.get_Range("A11", "K14").Font.Italic = true;
                sheet.get_Range("A11", "K14").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.Cells[16, 3] = "Payment No.";
                sheet.Cells[16, 5] = "Amount";
                sheet.Cells[16, 7] = "Due Date";
                sheet.Cells[16, 9] = "Remaining Balance";
                sheet.get_Range("B16", "J16").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("B16", "J16").Font.Underline = true;

                int y = 17;

                using (var ctx = new newerContext())
                {
                    var ser = from se in ctx.GenSOA
                              select se;

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Where(x=> x.PositionID == 1).First();
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;

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
                    //sheet.get_Range("J17", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

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
    }
}
