﻿using System;
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
using System.IO;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;


using System.Data.Entity;
using LoanManagement.Domain;
using Microsoft.VisualBasic;

using MahApps.Metro.Controls;
namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfMain.xaml
    /// </summary>
    public partial class wpfMain : MetroWindow
    {
        public int UserID;

        public wpfMain()
        {
            InitializeComponent();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("asd");
        }

        private void checkDue()
        {
            try
            {
                using (var ctx = new newContext())
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
                        var iAmt = itm.TotalAmount;
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
                            
                            iAmt = tAmount;
                            n++;
                            
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

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                reset();
                if (lb1.SelectedIndex == 0)
                {
                    tb1.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 1)
                {
                    tb2.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 2)
                {
                    tb3.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 3)
                {
                    tb4.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 4)
                {
                    tb5.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MetroWindow_Activated_1(object sender, EventArgs e)
        {
            try
            {
                //lbM.UnselectAll();
                reset();
                checkDue();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void reset()
        {
            try
            {
                itm1.IsSelected = false;
                itm2.IsSelected = false;
                itm3.IsSelected = false;
                itm4.IsSelected = false;
                itm5.IsSelected = false;
                itm6.IsSelected = false;
                itm7.IsSelected = false;
                itm8.IsSelected = false;
                itm9.IsSelected = false;
                itm10.IsSelected = false;
                itm11.IsSelected = false;
                itm12.IsSelected = false;
                itm13.IsSelected = false;
                itm14.IsSelected = false;
                itm15.IsSelected = false;
                itm16.IsSelected = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLogin frm = new wpfLogin();
                frm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
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
                //grdLo.Background = myBrush;
                //grdM.Background = myBrush;
                //grdBG.Background = myBrush;

                //System.Windows.MessageBox.Show(DateTime.Now.DayOfWeek.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void ListBoxItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfBank frm = new wpfBank();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_4(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_5(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm8_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm9_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm9_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm10_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectPayment frm = new wpfSelectPayment();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm12_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectClosed frm = new wpfSelectClosed();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm13_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.iDept = "Financing";
                frm.status = "Adjustment";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm11_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Restructure";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm14_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnBank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfBank frm = new wpfBank();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAgents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.UserID = UserID;
                frm.status = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoanAppllication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.UserID = UserID;
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectPayment frm = new wpfSelectPayment();
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectClosed frm = new wpfSelectClosed();
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.UserID = UserID;
                frm.status = "Restructure";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.iDept = "Financing";
                frm.UserID = UserID;
                frm.status = "Adjustment";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoanAppllication_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Micro Business";
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnClients_Copy10_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnBank_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfBank frm = new wpfBank();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAgents_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.status = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy11_Click(object sender, RoutedEventArgs e)
        {
            wpfUsers frm = new wpfUsers();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "List Of Clients";

                PdfPage page = document.AddPage();
                page.Orientation = PageOrientation.Landscape;
                String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\GFC.jpg";
                XImage xImage = XImage.FromFile(imagePath);
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                //Header Start
                //gfx.DrawString("Guahan Financing Corporation", font, XBrushes.Black, new XRect(0, 0, page.Width, 80), XStringFormats.Center);
                //System.Windows.MessageBox.Show(xImage.Width.ToString());
                gfx.DrawImage(xImage, 40, 10, xImage.Width - 260,xImage.Height / 3);
                font = new XFont("Verdana", 18, XFontStyle.Italic);
                gfx.DrawString("List Of Clients", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                font = new XFont("Verdana", 10, XFontStyle.Italic);
                gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                //Header End

                //ColumnHeader Start
                font = new XFont("Verdana", 10, XFontStyle.Bold);
                gfx.DrawString("Name", font, XBrushes.Black, new XRect(0, 0, 200, 350), XStringFormats.Center);
                gfx.DrawString("Gender", font, XBrushes.Black, new XRect(0, 0, 520, 350), XStringFormats.Center);
                gfx.DrawString("Status", font, XBrushes.Black, new XRect(0, 0, 720, 350), XStringFormats.Center);
                gfx.DrawString("Email", font, XBrushes.Black, new XRect(0, 0, 950, 350), XStringFormats.Center);
                gfx.DrawString("Contact", font, XBrushes.Black, new XRect(0, 0, 1157, 350), XStringFormats.Center);
                //ColumnHeader End

                int n = 380;
                int p = 1;
                font = new XFont("Verdana", 10, XFontStyle.Regular);
                using (var ctx = new newContext())
                {
                    var clt = from cl in ctx.Clients
                              where cl.Active == true
                              select cl;

                    foreach (var i in clt)
                    {
                        //LEFT ALIGN
                        XStringFormat xt = new XStringFormat();
                        xt.Alignment = XStringAlignment.Near;
                        xt.LineAlignment = XLineAlignment.Center;
                        //
                        gfx.DrawString("\t" + i.LastName + ", " + i.FirstName + " " + i.MiddleName + " " + i.Suffix, font, XBrushes.Black, new XRect(1,1,200,n),xt);
                        gfx.DrawString(i.Sex, font, XBrushes.Black, new XRect(0, 0, 520, n), XStringFormats.Center);
                        gfx.DrawString(i.Status, font, XBrushes.Black, new XRect(0, 0, 720, n), XStringFormats.Center);
                        gfx.DrawString(i.Email, font, XBrushes.Black, new XRect(0, 0, 950, n), XStringFormats.Center);
                        var ctr = ctx.ClientContacts.Where(x => x.ClientID == i.ClientID).Count();
                        if (ctr > 0)
                        {
                            var em = ctx.ClientContacts.Where(x => x.ClientID == i.ClientID).First();
                            String str = em.Contact.ToString();
                            gfx.DrawString(str, font, XBrushes.Black, new XRect(0, 0, 1150, n), XStringFormats.Center);
                        }
                        n += 30;
                        if (n >= 1150)
                        {
                            gfx.DrawString("Page " + p.ToString(), font, XBrushes.Black, new XRect(0, 0, 1500, 1150), XStringFormats.Center); ;
                            page = document.AddPage();
                            page.Orientation = PageOrientation.Landscape;
                            gfx = XGraphics.FromPdfPage(page);
                            font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                            gfx.DrawImage(xImage, 40, 10, xImage.Width - 260, xImage.Height / 3);
                            font = new XFont("Verdana", 18, XFontStyle.Italic);
                            gfx.DrawString("List Of Clients", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                            font = new XFont("Verdana", 10, XFontStyle.Italic);
                            gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                            //ColumnHeader Start
                            font = new XFont("Verdana", 10, XFontStyle.Bold);
                            gfx.DrawString("Name", font, XBrushes.Black, new XRect(0, 0, 200, 250), XStringFormats.Center);
                            gfx.DrawString("Gender", font, XBrushes.Black, new XRect(0, 0, 420, 250), XStringFormats.Center);
                            gfx.DrawString("Status", font, XBrushes.Black, new XRect(0, 0, 620, 250), XStringFormats.Center);
                            gfx.DrawString("Email", font, XBrushes.Black, new XRect(0, 0, 850, 250), XStringFormats.Center);
                            gfx.DrawString("Contact", font, XBrushes.Black, new XRect(0, 0, 1057, 250), XStringFormats.Center);
                            //ColumnHeader End
                            n = 280;
                            p++;
                        }
                    }
                    if (n < 1150)
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
                

                const string filename = "Clients.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "List Of Services";

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
                gfx.DrawString("List Of Services", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                font = new XFont("Verdana", 10, XFontStyle.Italic);
                gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                //Header End

                //ColumnHeader Start
                font = new XFont("Verdana", 10, XFontStyle.Bold);
                gfx.DrawString("Service Name", font, XBrushes.Black, new XRect(0, 0, 200, 350), XStringFormats.Center);
                gfx.DrawString("Type", font, XBrushes.Black, new XRect(0, 0, 450, 350), XStringFormats.Center);
                gfx.DrawString("Department", font, XBrushes.Black, new XRect(0, 0, 650, 350), XStringFormats.Center);
                gfx.DrawString("Interest %", font, XBrushes.Black, new XRect(0, 0, 850, 350), XStringFormats.Center);
                gfx.DrawString("Commission %", font, XBrushes.Black, new XRect(0, 0, 1050, 350), XStringFormats.Center);
                //ColumnHeader End

                int n = 380;
                int p = 1;
                font = new XFont("Verdana", 10, XFontStyle.Regular);
                using (var ctx = new newContext())
                {
                    var ser = from se in ctx.Services
                              where se.Active == true
                              select se;

                    foreach (var i in ser)
                    {
                        gfx.DrawString(i.Name, font, XBrushes.Black, new XRect(0, 0, 200, n), XStringFormats.Center);
                        gfx.DrawString(i.Type, font, XBrushes.Black, new XRect(0, 0, 450, n), XStringFormats.Center);
                        gfx.DrawString(i.Department, font, XBrushes.Black, new XRect(0, 0, 650, n), XStringFormats.Center);
                        gfx.DrawString(i.Interest.ToString(), font, XBrushes.Black, new XRect(0, 0, 850, n), XStringFormats.Center);
                        gfx.DrawString(i.AgentCommission.ToString(), font, XBrushes.Black, new XRect(0, 0, 1050, n), XStringFormats.Center);
                        n += 30;
                        if (n >= 1150)
                        {
                            gfx.DrawString("Page " + p.ToString(), font, XBrushes.Black, new XRect(0, 0, 1500, 1150), XStringFormats.Center);
                            page = document.AddPage();
                            page.Orientation = PageOrientation.Landscape;
                            gfx = XGraphics.FromPdfPage(page);
                            font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                            gfx.DrawImage(xImage, 40, 10, xImage.Width - 260, xImage.Height / 3);
                            font = new XFont("Verdana", 18, XFontStyle.Italic);
                            gfx.DrawString("List Of Services", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                            font = new XFont("Verdana", 10, XFontStyle.Italic);
                            gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                            //ColumnHeader Start
                            gfx.DrawString("Service Name", font, XBrushes.Black, new XRect(0, 0, 200, 250), XStringFormats.Center);
                            gfx.DrawString("Type", font, XBrushes.Black, new XRect(0, 0, 450, 250), XStringFormats.Center);
                            gfx.DrawString("Department", font, XBrushes.Black, new XRect(0, 0, 650, 250), XStringFormats.Center);
                            gfx.DrawString("Interest %", font, XBrushes.Black, new XRect(0, 0, 850, 250), XStringFormats.Center);
                            gfx.DrawString("Commission %", font, XBrushes.Black, new XRect(0, 0, 1050, 250), XStringFormats.Center);
                            //ColumnHeader End
                            n = 280;
                            p++;
                        }
                    }
                    if (n < 1150)
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

                const string filename = "Services.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "List Of Employees";

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
                gfx.DrawString("List Of Employees", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                font = new XFont("Verdana", 10, XFontStyle.Italic);
                gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                //Header EndDateTime.Today.Date.ToString(), font, XBrushes.Black, new XRect(0, 0, 200, 200), XStringFormats.Center);
                //Header End

                //ColumnHeader Start
                font = new XFont("Verdana", 10, XFontStyle.Bold);
                gfx.DrawString("Name", font, XBrushes.Black, new XRect(0, 0, 180, 350), XStringFormats.Center);
                gfx.DrawString("Position", font, XBrushes.Black, new XRect(0, 0, 400, 350), XStringFormats.Center);
                gfx.DrawString("Department", font, XBrushes.Black, new XRect(0, 0, 600, 350), XStringFormats.Center);
                gfx.DrawString("Email", font, XBrushes.Black, new XRect(0, 0, 830, 350), XStringFormats.Center);
                gfx.DrawString("Contact", font, XBrushes.Black, new XRect(0, 0, 1050, 350), XStringFormats.Center);
                //ColumnHeader End

                int n = 380;
                int p = 1;
                font = new XFont("Verdana", 10, XFontStyle.Regular);
                using (var ctx = new newContext())
                {
                    var emp = from em in ctx.Employees
                              where em.Active == true
                              select em;

                    foreach (var i in emp)
                    {
                        gfx.DrawString(i.LastName + ", " + i.FirstName + " " + i.MI + " " + i.Suffix, font, XBrushes.Black, new XRect(0, 0, 180, n), XStringFormats.Center);
                        gfx.DrawString(i.Position.PositionName, font, XBrushes.Black, new XRect(0, 0, 400, n), XStringFormats.Center);
                        gfx.DrawString(i.Department, font, XBrushes.Black, new XRect(0, 0, 600, n), XStringFormats.Center);
                        gfx.DrawString(i.Email.ToString(), font, XBrushes.Black, new XRect(0, 0, 830, n), XStringFormats.Center);
                        var ctr = ctx.EmployeeContacts.Where(x => x.EmployeeID == i.EmployeeID).Count();
                        if (ctr > 0)
                        {
                            var em = ctx.EmployeeContacts.Where(x => x.EmployeeID == i.EmployeeID).First();
                            String str = em.Contact.ToString();
                            gfx.DrawString(str, font, XBrushes.Black, new XRect(0, 0, 1050, n), XStringFormats.Center);
                        }
                        n += 30;
                        if (n >= 1150)
                        {
                            gfx.DrawString("Page " + p.ToString(), font, XBrushes.Black, new XRect(0, 0, 1500, 1150), XStringFormats.Center); ;
                            page = document.AddPage();
                            page.Orientation = PageOrientation.Landscape;
                            gfx = XGraphics.FromPdfPage(page);
                            font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                            gfx.DrawImage(xImage, 40, 10, xImage.Width - 260, xImage.Height / 3);
                            font = new XFont("Verdana", 18, XFontStyle.Italic);
                            gfx.DrawString("List Of Employees", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                            font = new XFont("Verdana", 10, XFontStyle.Italic);
                            gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                            //ColumnHeader Start
                            font = new XFont("Verdana", 10, XFontStyle.Bold);
                            gfx.DrawString("Name", font, XBrushes.Black, new XRect(0, 0, 180, 250), XStringFormats.Center);
                            gfx.DrawString("Position", font, XBrushes.Black, new XRect(0, 0, 400, 250), XStringFormats.Center);
                            gfx.DrawString("Department", font, XBrushes.Black, new XRect(0, 0, 600, 250), XStringFormats.Center);
                            gfx.DrawString("Email", font, XBrushes.Black, new XRect(0, 0, 830, 250), XStringFormats.Center);
                            gfx.DrawString("Contact", font, XBrushes.Black, new XRect(0, 0, 1050, 250), XStringFormats.Center);
                            //ColumnHeader End
                            n = 280;
                            p++;
                        }
                    }
                    if (n < 1150)
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

                const string filename = "Employees.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAgents_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "List Of Agents";

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
                gfx.DrawString("List Of Agents", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                font = new XFont("Verdana", 10, XFontStyle.Italic);
                gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                //Header End

                //ColumnHeader Start
                font = new XFont("Verdana", 10, XFontStyle.Bold);
                gfx.DrawString("Name", font, XBrushes.Black, new XRect(0, 0, 180, 350), XStringFormats.Center);
                gfx.DrawString("Email", font, XBrushes.Black, new XRect(0, 0, 400, 350), XStringFormats.Center);
                gfx.DrawString("Contact", font, XBrushes.Black, new XRect(0, 0, 600, 350), XStringFormats.Center);
                //ColumnHeader End

                int n = 380;
                int p = 1;
                font = new XFont("Verdana", 10, XFontStyle.Regular);
                using (var ctx = new newContext())
                {
                    var emp = from em in ctx.Agents
                              where em.Active == true
                              select em;

                    foreach (var i in emp)
                    {
                        gfx.DrawString(i.LastName + ", " + i.FirstName + " " + i.MI + " " + i.Suffix, font, XBrushes.Black, new XRect(0, 0, 180, n), XStringFormats.Center);
                        gfx.DrawString(i.Email, font, XBrushes.Black, new XRect(0, 0, 400, n), XStringFormats.Center);
                        var ctr = ctx.AgentContacts.Where(x => x.AgentID == i.AgentID).Count();
                        if (ctr > 0)
                        {
                            var em = ctx.AgentContacts.Where(x => x.AgentID == i.AgentID).First();
                            String str = em.Contact.ToString();
                            gfx.DrawString(str, font, XBrushes.Black, new XRect(0, 0, 600, 250), XStringFormats.Center);
                        }
                        n += 30;
                        if (n >= 1150)
                        {
                            gfx.DrawString("Page " + p.ToString(), font, XBrushes.Black, new XRect(0, 0, 1500, 1150), XStringFormats.Center);
                            page = document.AddPage();
                            page.Orientation = PageOrientation.Landscape;
                            gfx = XGraphics.FromPdfPage(page);
                            font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                            gfx.DrawImage(xImage, 40, 10, xImage.Width - 260, xImage.Height / 3);
                            font = new XFont("Verdana", 18, XFontStyle.Italic);
                            gfx.DrawString("List Of Agents", font, XBrushes.Black, new XRect(0, 0, page.Width, 220), XStringFormats.Center);
                            font = new XFont("Verdana", 10, XFontStyle.Italic);
                            gfx.DrawString("As of " + DateTime.Today.Date.ToString().Split(' ')[0], font, XBrushes.Black, new XRect(0, 0, page.Width, 250), XStringFormats.Center);
                            //ColumnHeader Start
                            font = new XFont("Verdana", 10, XFontStyle.Bold);

                            //ColumnHeader End
                            n = 280;
                            p++;
                        }
                    }
                    if (n < 1150)
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

                const string filename = "Agents.pdf";
                document.Save(filename);
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnPosition_Click(object sender, RoutedEventArgs e)
        {
            wpfPosition frm = new wpfPosition();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnVLoan_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.status = "View";
            frm.ShowDialog();
        }

        private void btnVClient_Click(object sender, RoutedEventArgs e)
        {
            wpfClientSearch frm = new wpfClientSearch();
            frm.status = "Client";
            frm.status2 = "View1";
            frm.ShowDialog();
        }

        private void btnClients_Copy9_Click(object sender, RoutedEventArgs e)
        {
            wpfAudtiTrail frm = new wpfAudtiTrail();
            frm.ShowDialog();
        }

        private void btnVAging_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.status = "Aging";
            frm.ShowDialog();
        }

        private void btnMReleasing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.UserID = UserID;
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnHoliday_Click(object sender, RoutedEventArgs e)
        {
            wpfHoliday frm = new wpfHoliday();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnMPayments_Click(object sender, RoutedEventArgs e)
        {
            wpfSelectMPayment frm = new wpfSelectMPayment();
            frm.UserID = UserID;
            frm.ShowDialog();
        }

        private void btnCPayments_Click(object sender, RoutedEventArgs e)
        {
            wpfSelectCPayments frm = new wpfSelectCPayments();
            frm.UserID = UserID;
            frm.ShowDialog();
        }


        




    }
}
