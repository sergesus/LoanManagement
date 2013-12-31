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

using System.Data.Entity;
using LoanManagement.Domain;
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfReportForLoans.xaml
    /// </summary>
    public partial class wpfReportForLoans : MetroWindow
    {
        public int UserID;

        public wpfReportForLoans()
        {
            InitializeComponent();
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

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //toEdit
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;
                checkServices();
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtTo.SelectedDate.Value.Date < dtFrom.SelectedDate.Value.Date)
                {
                    System.Windows.MessageBox.Show("TO date must be greater than or equal to FROM date", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (cmbDept.Text == "" || cmbStat.Text == "" || cmbTOL.Text == "")
                {
                    System.Windows.MessageBox.Show("Please complete all information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                printSOA();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void printSOA()
        {
            string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iListOfLoans.xls";
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
                sheet.Name = "List Of Loans";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.PageSetup.RightMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "J8"].MergeCells = true;
                sheet.Range["A8", "J8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "List Of Loans";
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
                    
                    sheet.Cells[11, 1] = "Status:  " + cmbStat.Text;
                    sheet.Cells[12, 1] = "Department:  " + cmbDept.Text;
                    sheet.Cells[13, 1] = "Type:  " + cmbTOL.Text;
                    sheet.Cells[14, 1] = "From: " + dtFrom.SelectedDate.Value.ToString().Split(' ')[0];
                    sheet.Cells[15, 1] = "To: " + dtTo.SelectedDate.Value.ToString().Split(' ')[0];
                }

                sheet.get_Range("A11", "K15").Font.Italic = true;
                sheet.get_Range("A11", "K15").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.Cells[16, 2] = "Loan ID";
                sheet.Cells[16, 3] = "Client";
                sheet.Cells[16, 4] = "Term";
                sheet.Cells[16, 5] = "Mode";
                ComboBoxItem typeItem = (ComboBoxItem)cmbStat.SelectedItem;
                string st = typeItem.Content.ToString();
                ComboBoxItem typeItem2= (ComboBoxItem)cmbDept.SelectedItem;
                string dp = typeItem2.Content.ToString();
                ComboBoxItem typeItem3 = (ComboBoxItem)cmbTOL.SelectedItem;
                string tp = typeItem3.Content.ToString();

                if (st == "Applied")
                    sheet.Cells[16, 6] = "Amt. " + st;
                else if (st == "Approved" || st == "Declined")
                    sheet.Cells[16, 6] = "Amt. Approved";
                else
                    sheet.Cells[16, 6] = "Amt. Released";

                sheet.Cells[16, 7] = "Remaining";
                sheet.Cells[16, 8] = "Amt. Paid";
                if(st == "Under Collection")
                    sheet.Cells[16, 8] = "Total Collection";

                if (st == "All")
                    sheet.Cells[16, 9] = "Status";
                else
                    sheet.Cells[16, 9] = "";

                if (st == "All")
                    st = "";
                if (dp == "Both")
                    dp = "";
                if (tp == "All")
                    tp = "";


                sheet.get_Range("B16", "J16").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("B16", "J16").Font.Underline = true;

                int y = 18;


                int ictr = 0;
                using (var ctx = new newerContext())
                {
                    
                    
                    var ser = from se in ctx.Loans
                              where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                              select se;

                    if (st != "")
                    {
                        if (st == "Applied")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.LoanApplication.DateApplied <= dtTo.SelectedDate.Value.Date && se.LoanApplication.DateApplied >= dtFrom.SelectedDate.Value.Date)
                                  select se;
                        }
                        else if (st == "Approved")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.ApprovedLoan.DateApproved <= dtTo.SelectedDate.Value && se.ApprovedLoan.DateApproved >= dtFrom.SelectedDate.Value)
                                  select se;
                        }
                        else if (st == "Declined")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.DeclinedLoan.DateDeclined <= dtTo.SelectedDate.Value && se.DeclinedLoan.DateDeclined >= dtFrom.SelectedDate.Value)
                                  select se;
                        }
                        else if (st == "Paid")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.PaidLoan.DateFinished <= dtTo.SelectedDate.Value && se.PaidLoan.DateFinished >= dtFrom.SelectedDate.Value)
                                  select se;
                        }
                        else if (st == "Under Collection")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.PassedToCollector.DatePassed <= dtTo.SelectedDate.Value && se.PassedToCollector.DatePassed >= dtFrom.SelectedDate.Value)
                                  select se;
                        }
                        else if (st == "Closed Account")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.ClosedAccount.First().DateClosed <= dtTo.SelectedDate.Value && se.ClosedAccount.First().DateClosed >= dtFrom.SelectedDate.Value)
                                  select se;
                        }
                        else if (st == "Resturctured")
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.RestructuredLoan.DateRestructured <= dtTo.SelectedDate.Value && se.RestructuredLoan.DateRestructured >= dtFrom.SelectedDate.Value)
                                  select se;
                        }
                        else
                        {
                            ser = from se in ctx.Loans
                                  where se.Status.Contains(st) && se.Service.Department.Contains(dp) && se.Service.Name.Contains(tp)
                                  && (se.ReleasedLoan.DateReleased <= dtTo.SelectedDate.Value.Date && se.ReleasedLoan.DateReleased >= dtFrom.SelectedDate.Value.Date)
                                  select se;
                        }

                    }

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;

                    foreach (var i in ser)
                    {
                        ictr++;
                        sheet.Cells[y, 2] = i.LoanID;
                        string myString = i.Client.LastName + ", " + i.Client.FirstName + " " + i.Client.MiddleName + " " + i.Client.Suffix;
                        try
                        {
                            myString = myString.Substring(0, 30);
                        }
                        catch (Exception) { myString = i.Client.LastName + ", " + i.Client.FirstName + " " + i.Client.MiddleName + " " + i.Client.Suffix; }
                        sheet.Cells[y, 3] = myString;
                        sheet.Cells[y, 4] = i.Term + " mo.";
                        sheet.Cells[y, 5] = i.Mode;

                        try
                        {
                            if (st == "Applied")
                                sheet.Cells[y, 6] = i.LoanApplication.AmountApplied.ToString("N2");
                            else if (st == "Approved" || st == "Declined")
                                sheet.Cells[y, 6] = i.ApprovedLoan.AmountApproved.ToString("N2");
                            else
                                sheet.Cells[y, 6] = i.ReleasedLoan.Principal.ToString("N2");
                        }
                        catch (Exception) { sheet.Cells[y, 6] = ""; }

                        string rem = "";
                        string pd = "";

                        if (i.Status == "Released")
                        {
                            if (i.Service.Department == "Micro Business")
                            {
                                
                                var rmn = from rm in ctx.MPaymentInfoes
                                          where rm.LoanID == i.LoanID && rm.PaymentStatus == "Paid"
                                          select rm;
                                double r = 0;
                                foreach (var item in rmn)
                                {
                                    r = r + item.TotalPayment;
                                }
                                //System.Windows.MessageBox.Show(i.LoanID.ToString());
                                //var ctr = ctx.MPaymentInfoes.Where(x => x.LoanID == i.LoanID && x.PaymentStatus == "Pending").Count();
                                //System.Windows.MessageBox.Show(ctr.ToString());
                                rem = ctx.MPaymentInfoes.Where(x => x.LoanID ==i.LoanID && x.PaymentStatus == "Pending").First().RemainingLoanBalance.ToString("N2");
                                pd = r.ToString("N2");
                            }
                            else
                            {
                                var rmn = from rm in ctx.FPaymentInfo
                                          where rm.LoanID == i.LoanID && rm.PaymentStatus == "Cleared"
                                          select rm;
                                double r = 0;
                                foreach (var item in rmn)
                                {
                                    r = r + item.Amount;
                                }
                                double remain = i.ReleasedLoan.TotalLoan - r;
                                rem = remain.ToString("N2");
                                pd = r.ToString("N2");
                            }
                        }
                        else if (i.Status == "Under Collection")
                            rem = i.PassedToCollector.RemainingBalance.ToString("N2");
                        else if (i.Status == "Applied" || i.Status == "Approved" || i.Status == "Declined" || i.Status == "Canceled")
                        {
                            rem = "-";
                            pd = "-";
                        }
                        else
                        {
                            rem = "-";
                            pd = "-";
                        }

                        if (st == "")
                            sheet.Cells[y, 9] = i.Status + "     ";

                        sheet.Cells[y, 7] = rem;
                        sheet.Cells[y, 8] = pd;
                        
                        y++;
                    }

                    sheet.get_Range("B18", "B" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("C18", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("D18", "D" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("E18", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("F18", "F" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("G18", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("H18", "H" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("I18", "I" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                }
                if (ictr < 1)
                {
                    System.Windows.MessageBox.Show("No records to generate", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }

                //sheet.Range["A16", "I" + y].AutoFit();
                sheet.get_Range("B16", "I" + (y - 1)).EntireColumn.AutoFit();
                sheet.get_Range("B16", "I" + (y - 1)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iListOfLoans.pdf";
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

        private void cmbDept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkServices();
        }
    }
}
