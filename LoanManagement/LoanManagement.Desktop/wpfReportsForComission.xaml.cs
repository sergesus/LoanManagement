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
    /// Interaction logic for wpfReportsForComission.xaml
    /// </summary>
    public partial class wpfReportsForComission : MetroWindow
    {
        public int UserID;

        public wpfReportsForComission()
        {
            InitializeComponent();
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

                string FileName = AppDomain.CurrentDomain.BaseDirectory + @"iCommision.xls";
                Microsoft.Office.Interop.Excel._Application xl = null;
                Microsoft.Office.Interop.Excel._Workbook wb = null;
                Microsoft.Office.Interop.Excel._Worksheet sheet = null;
                bool SaveChanges = false;


                if (File.Exists(FileName)) { File.Delete(FileName); }

                GC.Collect();

                // Create a new instance of Excel from scratch

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.Visible = false;
                wb = (Microsoft.Office.Interop.Excel._Workbook)(xl.Workbooks.Add(Missing.Value));
                sheet = (Microsoft.Office.Interop.Excel._Worksheet)(wb.Sheets[1]);

                // set come column heading names
                sheet.Name = "Commision Reports";
                sheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                sheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.PageSetup.RightFooter = "Page &P of &N";
                sheet.PageSetup.TopMargin = 0.5;
                sheet.Range["A2", "F2"].MergeCells = true;
                sheet.Range["A7", "E7"].MergeCells = true;
                sheet.Range["A8", "J8"].MergeCells = true;
                sheet.Range["A8", "J8"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[8, 1] = "Commision Reports";
                sheet.get_Range("A8", "J8").Font.Bold = true;
                sheet.get_Range("A8", "J8").Font.Size = 18;
                sheet.Range["A9", "J9"].MergeCells = true;
                sheet.Range["A9", "J9"].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                sheet.Cells[9, 1] = "Date Prepared: " + DateTime.Now;
                sheet.Range["A1", "Z1"].Columns.AutoFit();
                sheet.Range["A2", "Z2"].Columns.AutoFit();
                //sheet.Cells["1:100"].Rows.AutoFit(); 
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                sheet.Shapes.AddPicture(imagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, 60, 0, 500, 100);
                sheet.PageSetup.CenterHeaderPicture.Filename = imagePath;

                sheet.Range["A10", "D10"].MergeCells = true;

                sheet.Cells[11, 1] = dtFrom.SelectedDate.Value.ToString().Split(' ')[0] + " To " + dtTo.SelectedDate.Value.ToString().Split(' ')[0];
                sheet.get_Range("A11", "K15").Font.Italic = true;
                sheet.get_Range("A11", "K15").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                sheet.Cells[14, 1] = "Agent Name";
                sheet.Cells[14, 3] = "No. Of Accounts";
                sheet.Cells[14, 5] = "Total Release";
                sheet.Cells[14, 7] = "Total Commission";
                sheet.get_Range("A14", "M14").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                sheet.get_Range("A14", "M14").Font.Underline = true;

                int y = 15;

                using (var ctx = new finalContext())
                {
                    var ser = from se in ctx.Agents
                              where se.Active == true
                              select se;

                    ctx.Database.ExecuteSqlCommand("delete from dbo.AgentCommissions");
                    ctx.SaveChanges();
                    foreach (var itm in ser)
                    {
                        var com = from co in ctx.ReleasedLoans
                                  where co.Loan.AgentID == itm.AgentID && (co.DateReleased >= dtFrom.SelectedDate.Value && co.DateReleased <= dtTo.SelectedDate.Value)
                                  select co;
                        double tCom = 0;
                        double tRel = 0;
                        int n = 0;
                        try
                        {
                            n = com.Count();
                            tCom = com.Sum(x => x.AgentsCommission);
                            tRel = com.Sum(x => x.Principal);
                        }
                        catch (Exception)
                        {
                            tCom = 0;
                            tRel = 0;
                        }
                        AgentCommission ac = new AgentCommission{ AgentName=(itm.LastName + ", " + itm.FirstName), NoOfAccounts=n, TotalCommission=tCom, TotalRelease = tRel };
                        ctx.AgentCommission.Add(ac);
                    }
                    ctx.SaveChanges();

                    var ser2 = (from se in ctx.AgentCommission
                                select se).OrderByDescending(x=> x.TotalRelease);

                    var emp2 = ctx.Employees.Find(UserID);

                    //sheet.Cells[10, 1] = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    sheet.PageSetup.LeftFooter = "Prepared By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    emp2 = ctx.Employees.Find(1);
                    sheet.PageSetup.CenterFooter = "Confirmed By: " + emp2.LastName + ", " + emp2.FirstName + " " + emp2.MI + " " + emp2.Suffix;
                    foreach (var i in ser2)
                    {
                        sheet.Cells[y, 1] = i.AgentName;
                        sheet.Cells[y, 3] = i.NoOfAccounts;
                        sheet.Cells[y, 5] = i.TotalRelease.ToString("N2");
                        sheet.Cells[y, 7] = i.TotalCommission.ToString("N2");
                        y++;
                    }
                    sheet.get_Range("A13", "A" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    sheet.get_Range("C13", "C" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("E13", "E" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    sheet.get_Range("G13", "G" + y).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

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

                string paramExportFilePath = AppDomain.CurrentDomain.BaseDirectory + @"iCommision.pdf";
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
    }
}
