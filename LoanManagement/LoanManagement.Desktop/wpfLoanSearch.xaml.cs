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

using MahApps.Metro.Controls;
using LoanManagement.Domain;
using System.Data.Entity;
using System.IO;
using System.Diagnostics;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanSearch.xaml
    /// </summary>
    public partial class wpfLoanSearch : MetroWindow
    {
        public string status;
        public string iDept;
        public int UserID;
        public bool cont = false;

        public wpfLoanSearch()
        {
            InitializeComponent();
        }

        public string getRow(System.Windows.Controls.DataGrid dg, int row)
        {
            try
            {
                object item = dg.SelectedItem;
                string str = (dg.SelectedCells[row].Column.GetCellContent(item) as TextBlock).Text;
                return str;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return "";
            }
        }

        public void rg()
        {
            try
            {
                int n = 0;
                try 
                {
                    n = Convert.ToInt32(txtSearch.Text);
                }
                catch(Exception)
                {
                    n = 0;
                }
                if (status == "UApproval")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.Status == "Approved" || ln.Status == "Declined") && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();

                    }
                }
                else if (status == "Approval" || status == "Application")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Applied" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();

                    }
                }
                else if (status == "Renewal Approval")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Applied for Renewal" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();

                    }
                }
                else if (status == "Confirmation")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.TemporaryLoanApplications
                                  where ln.Service.Department == iDept && (ln.TemporaryLoanApplicationID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.TemporaryLoanApplicationID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                    }
                }
                else if (status == "Releasing")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Approved" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();

                    }
                }
                else if (status == "Renewal Releasing")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Approved for Renewal" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();

                    }
                }
                else if (status == "UReleasing")
                {
                    if (iDept == "Financing")
                    {
                        using (var ctx = new finalContext())
                        {
                            var lon = from ln in ctx.Loans
                                      where ln.Status == "Released" && ln.Service.Department == iDept && ln.FPaymentInfo.Where(x => x.PaymentStatus != "Pending").Count() == 0 && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                      select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                            dgLoan.ItemsSource = lon.ToList();

                        }
                    }
                    else
                    {
                        using (var ctx = new finalContext())
                        {
                            var lon = from ln in ctx.Loans
                                      where ln.Status == "Released" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                      select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                            dgLoan.ItemsSource = lon.ToList();

                        }
                    }
                }
                else if (status == "Holding")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.Status == "Released" || ln.Status == "Active") && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();

                    }
                }
                else if (status == "Renewal Application")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.Status == "Released" || ln.Status == "Active") && (ln.FPaymentInfo.Where(x => x.PaymentStatus == "Due" || x.PaymentStatus == "On Hold" || x.PaymentStatus == "Due" || x.PaymentStatus == "Due/Pending" || x.PaymentStatus=="Pending").Count() < 4) && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Apply Renewal";
                    }
                }
                else if (status == "Voiding")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.Status == "Released" || ln.Status == "Active" || ln.Status=="Paid") && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Void last payment";
                    }
                }
                else if (status == "Voiding2")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.Status == "Under Collection") && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Void last payment";
                    }
                }
                else if (status == "VoidClosed")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Closed Account" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Void Closed Account";
                    }
                }
                else if (status == "Pass")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Closed Account" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Pass To Collectors";
                    }
                }
                else if (status == "Renewal")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ln.Status == "Closed Account" && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Renew Closed Account";
                    }
                }
                else if (status == "Adjustment")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.Status == "Released" || ln.Status == "Active") && (ln.FPaymentInfo.Where(x => x.PaymentStatus == "Due" || x.PaymentStatus == "On Hold" || x.PaymentStatus == "Due" || x.PaymentStatus == "Due/Pending").Count() < 1) && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "Adjust Payment Date";
                    }
                }
                else if (status == "Restructure")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ((ln.Status == "Released" || ln.Status == "Active") && (ln.FPaymentInfo.Where(x => x.PaymentStatus == "Cleared").Count() >= 3)) || (ln.Status == "Closed Account") && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "View";
                    }
                }
                else if (status == "Full")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where ((ln.Status == "Released" || ln.Status == "Active") && (ln.FPaymentInfo.Where(x => x.PaymentStatus != "Cleared").Count() > 1)) && ln.Service.Department == iDept && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "View";
                    }
                }
                else if(status=="View")
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = from ln in ctx.Loans
                                  where (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ","").Contains(txtSearch.Text.Replace(" ","")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "View";
                    }
                }
                else if (status == "Aging")
                {
                    using (var ctx = new finalContext())
                    {
                        //var ctr = ctx.Loans.Where(x=> x.LoanID == 5)
                        var lon = from ln in ctx.Loans
                                  where (ln.FPaymentInfo.Where(x=> x.PaymentStatus.Contains("Due")).Count() >= 2 ) && (ln.Status == "Released" || ln.Status == "Active") && (ln.LoanID == n || ln.Service.Name.Contains(txtSearch.Text) || (ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName).Replace(" ", "").Contains(txtSearch.Text.Replace(" ", "")))
                                  select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, NumberOfMonthsUnpaid = ln.FPaymentInfo.Where(x=> x.PaymentStatus.Contains("Due")).Count(), ClientName = ln.Client.FirstName + " " + ln.Client.MiddleName + " " + ln.Client.LastName };
                        dgLoan.ItemsSource = lon.ToList();
                        btnView.Content = "View Loan Options";
                    }
                }
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
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Application")
                {
                    wpfLoanApplication frm = new wpfLoanApplication();
                    frm.status = "Edit";
                    int id = Convert.ToInt32(getRow(dgLoan, 0));
                    frm.lId = id;
                    frm.UserID = UserID;
                    frm.btnContinue.Content = "Update";
                    frm.ShowDialog();
                }
                else if (status == "Renewal Application")
                {
                    wpfLoanApplication frm = new wpfLoanApplication();
                    frm.status = "Renewal";
                    int id = Convert.ToInt32(getRow(dgLoan, 0));
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.LoanRenewals.Where(x => x.LoanID == id && (x.Status == "Pending" || x.Status == "Approved")).Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("There is an existing application for this loan","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    frm.lId = id;
                    frm.UserID = UserID;
                    frm.btnContinue.Content = "Renew";
                    frm.ShowDialog();
                }
                else if (status == "Confirmation")
                {
                    using (var ctx = new finalContext())
                    {
                        int lID = Convert.ToInt32(getRow(dgLoan, 0));
                        var clt = ctx.TemporaryLoanApplications.Where(x => x.TemporaryLoanApplicationID == lID).First();
                        int cid = clt.ClientID;
                        var ctr = ctx.Clients.Where(x => x.ClientID == cid).Count();
                        if (ctr == 0)
                        {
                            MessageBox.Show("Client doesn't exists");
                            return;
                        }

                        if (clt.Client.isConfirmed == false)
                        {
                            System.Windows.MessageBox.Show("Client is not yet confirmed. Please confirm the client first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Released").Count();
                        if (ictr > 0)
                        {
                            ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Released" && x.Service.Department != iDept).Count();
                            if (ictr > 0)
                            {
                                System.Windows.MessageBox.Show("Client has an existing loan on other department", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Released" && x.Service.Type == "Non Collateral" && x.Service.Department == iDept).Count();
                                if (ictr > 0)
                                {
                                    System.Windows.MessageBox.Show("Client has an existing Non-Collateral loan. The client can only apply for collateral loan.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }

                        ictr = ctx.Loans.Where(x => x.ClientID == cid && (x.Status == "Applied" || x.Status == "Approved")).Count();
                        if (ictr > 0)
                        {
                            System.Windows.MessageBox.Show("Client cannot have multiple applications at the same time", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        ictr = ctx.Loans.Where(x => x.ClientID == cid && x.Status == "Under Collection").Count();
                        if (ictr > 0)
                        {
                            System.Windows.MessageBox.Show("Client cannot have another application while having a loan under collection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        wpfLoanApplication frm = new wpfLoanApplication();
                        frm.cId = Convert.ToInt32(clt.ClientID);
                        frm.status = "Confirmation";
                        frm.btnContinue.Content = "Confirm";
                        frm.iDept = iDept;
                        frm.UserID = UserID;
                        frm.lId = lID;
                        this.Close();
                        frm.ShowDialog();
                        this.Close();
                    }
                }
                else if (status == "UReleasing")
                {
                    wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                    int num = Convert.ToInt32(getRow(dgLoan, 0));
                    frm.lId = num;
                    frm.UserID = UserID;
                    frm.status = status;
                    frm.iDept = iDept;
                    frm.ShowDialog();
                }
                else if (status == "Full")
                {
                    wpfNewCheque frm = new wpfNewCheque();
                    int num = Convert.ToInt32(getRow(dgLoan, 0));
                    frm.lID = num;
                    frm.UserID = UserID;
                    frm.status = status;
                    frm.ShowDialog();
                }
                else if (status == "Holding")
                {
                    wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                    int num = Convert.ToInt32(getRow(dgLoan, 0));
                    frm.lId = num;
                    frm.UserID = UserID;
                    frm.status = status;
                    frm.ShowDialog();
                }
                else if (status == "Voiding")
                {
                    if (iDept == "Financing")
                    {
                        using (var ctx = new finalContext())
                        {
                            int n = Convert.ToInt32(getRow(dgLoan, 0));
                            int num = ctx.FPaymentInfo.Where(x => x.LoanID == n && x.PaymentStatus == "Deposited").Count();
                            if (num > 0)
                            {
                                MessageBox.Show("Unable to void payment due to deposited cheque", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                            num = ctx.FPaymentInfo.Where(x => x.LoanID == n && x.PaymentStatus == "Cleared").Count();
                            if (num > 0)
                            {
                                var fp1 = from f in ctx.FPaymentInfo
                                          where f.LoanID == n && f.PaymentStatus == "Cleared"
                                          select f;
                                int x = 0;
                                FPaymentInfo fp = null;
                                foreach (var item in fp1)
                                {
                                    if (x == num - 1)
                                    {
                                        fp = item;
                                    }
                                    x++;
                                }
                                //var fp = ctx.FPaymentInfo.Where(x => x.LoanID == n && x.PaymentStatus == "Cleared").Last();
                                MessageBox.Show("The last payment has the following info: \n No :" + fp.PaymentNumber + " \n ChequeNumber: " + fp.ChequeInfo + " \n Amount: " + fp.Amount.ToString("N2") + "","Information",MessageBoxButton.OK, MessageBoxImage.Information);
                                MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (mr == MessageBoxResult.Yes)
                                {
                                    wpfPassword frm = new wpfPassword();
                                    frm.status = "void";
                                    frm.ID = UserID;
                                    frm.ShowDialog();

                                    if (cont == false)
                                    {
                                        MessageBox.Show("Please re-enter you password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        return;
                                    }   
                                    fp.PaymentStatus = "Deposited";
                                    ClearedCheque cc = ctx.ClearedCheques.Find(fp.ClearCheque.FPaymentInfoID);
                                    ctx.ClearedCheques.Remove(cc);
                                    AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Voided Payment for Cheque " + fp.ChequeInfo };
                                    ctx.AuditTrails.Add(at);
                                    ctx.SaveChanges();
                                    MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No payments to void", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        using (var ctx = new finalContext())
                        {
                            int n = Convert.ToInt32(getRow(dgLoan, 0));
                            int num = ctx.MPaymentInfoes.Where(x => x.LoanID == n && x.PaymentStatus == "Paid").Count();
                            if (num > 0)
                            {
                                var fp1 = from f in ctx.MPaymentInfoes
                                          where f.LoanID == n && f.PaymentStatus == "Paid"
                                          select f;
                                int x = 0;
                                MPaymentInfo fp = null;
                                foreach (var item in fp1)
                                {
                                    if (x == num - 1)
                                    {
                                        fp = item;
                                    }
                                    x++;
                                }
                                //var fp = ctx.FPaymentInfo.Where(x => x.LoanID == n && x.PaymentStatus == "Cleared").Last();
                                var mp = ctx.MPaymentInfoes.Where(m => m.LoanID == n && m.PaymentNumber == fp.PaymentNumber).First();
                                MessageBox.Show("The last payment has the following info: \n No :" + mp.PaymentNumber + "\n Total Payment: " + mp.TotalPayment.ToString("N2") + "");
                                MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (mr == MessageBoxResult.Yes)
                                {
                                    wpfPassword frm = new wpfPassword();
                                    frm.status = "void";
                                    frm.ID = UserID;
                                    frm.ShowDialog();

                                    if (cont == false)
                                    {
                                        MessageBox.Show("Please re-enter you password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        return;
                                    }

                                    try
                                    {
                                        var m2 = ctx.MPaymentInfoes.Where(m => m.LoanID == n && m.PaymentStatus == "Pending").First();
                                        ctx.MPaymentInfoes.Remove(m2);
                                    }
                                    catch (Exception)
                                    { 
                                        
                                    }
                                    mp.PaymentStatus = "Pending";
                                    mp.TotalPayment=0;
                                    mp.PaymentDate = null;

                                    x=1;
                                    var mps = from m in ctx.MPaymentInfoes
                                              where m.PaymentNumber == mp.PaymentNumber
                                              select m;

                                    foreach (var itm in mps)
                                    { 
                                        if(x!=1)
                                            ctx.MPaymentInfoes.Remove(itm);
                                        x++;
                                    }
                                    var lon = ctx.Loans.Find(n);
                                    if (lon.Status == "Paid")
                                    {
                                        lon.Status = "Released";
                                        var pl = ctx.PaidLoans.Find(n);
                                        ctx.PaidLoans.Remove(pl);
                                    }
                                    
                                    AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Voided Payment for Payment Number " + fp.PaymentNumber + " for Loan " + n };
                                    ctx.AuditTrails.Add(at);
                                    ctx.SaveChanges();
                                    MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No payments to void", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
                else if (status == "Voiding2")
                {
                    using (var ctx = new finalContext())
                    {
                        int n = Convert.ToInt32(getRow(dgLoan, 0));
                        int num = ctx.CollectionInfoes.Where(x => x.LoanID == n).Count();
                        if (num > 0)
                        {
                            var fp1 = from f in ctx.CollectionInfoes
                                      where f.LoanID == n
                                      select f;
                            int x = 0;
                            CollectionInfo fp = null;
                            foreach (var item in fp1)
                            {
                                if (x == num - 1)
                                {
                                    fp = item;
                                }
                                x++;
                            }
                            MessageBox.Show("The last payment has the following info: \n Total Collection :" + fp.TotalCollection + "\n Collection Date: " + fp.DateCollected.ToString().Split(' ')[0] + "");
                            MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mr == MessageBoxResult.Yes)
                            {

                                var r = ctx.PassedToCollectors.Find(fp.LoanID);
                                r.RemainingBalance = r.RemainingBalance + fp.TotalCollection;
                                ctx.CollectionInfoes.Remove(fp);
                                AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Voided Collection for Loan " + n };
                                ctx.AuditTrails.Add(at);
                                ctx.SaveChanges();
                                MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No payments to void", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else if (status == "VoidClosed")
                {
                    int n = Convert.ToInt32(getRow(dgLoan, 0));
                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {

                        using (var ctx = new finalContext())
                        {
                            var lon = ctx.Loans.Find(n);
                            lon.Status = "Released";
                            var fp = ctx.ClosedAccounts.Where(x => x.LoanID == n && x.isPaid == false).First();
                            ctx.ClosedAccounts.Remove(fp);
                            var fp2 = from f in ctx.FPaymentInfo
                                      where f.LoanID == n && f.PaymentStatus != "Cleared"
                                      select f;
                            int myC = 0;
                            foreach (var item in fp2)
                            {
                                if (myC == 0)
                                {
                                    item.PaymentStatus = "Deposited";
                                }
                                else
                                {
                                    item.PaymentStatus = "Pending";
                                }
                                myC++;
                            }
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Voided Closed Account for Loan " + lon.LoanID };
                            ctx.AuditTrails.Add(at);
                            ctx.SaveChanges();
                            MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }
                else if (status == "Pass")
                {
                    int n = Convert.ToInt32(getRow(dgLoan, 0));
                    wpfPassToCollectors frm = new wpfPassToCollectors();
                    frm.n = n;
                    frm.UserID = UserID;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "Renewal")
                {
                    int n = Convert.ToInt32(getRow(dgLoan, 0));
                    wpfRenewClosed frm = new wpfRenewClosed();
                    frm.UserID = UserID;
                    frm.lId = n;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "Adjustment" || status == "Restructure" || status == "View")
                {
                    if (status == "View")
                    {
                        using (var ctx = new finalContext())
                        {
                            int n = Convert.ToInt32(getRow(dgLoan, 0));
                            var lon = ctx.Loans.Find(n);
                            if (lon.Status == "Applied" || lon.Status == "Declined" || lon.Status == "Approved")
                            {
                                wpfAppliedLoanInfo frm = new wpfAppliedLoanInfo();
                                frm.lId = n;
                                frm.status = "View";
                                frm.UserID = UserID;
                                frm.Height = 605.5;
                                frm.ShowDialog();
                            }
                            else
                            {
                                wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                                frm.lId = n;
                                frm.status = "View";
                                frm.Height = 605.5;
                                frm.UserID = UserID;
                                frm.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        int n = Convert.ToInt32(getRow(dgLoan, 0));
                        wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                        frm.lId = n;
                        frm.UserID = UserID;
                        frm.status = status;
                        if (status == "View")
                        {
                            frm.Height = 605.5;
                        }
                        this.Close();
                        frm.ShowDialog();
                    }
                }
                else if (status == "Aging")
                {
                    wpfLoanInfo frm = new wpfLoanInfo();
                    int num = Convert.ToInt32(getRow(dgLoan, 0));
                    frm.lId = num;
                    frm.name = getRow(dgLoan, 4);
                    frm.UserID = UserID;
                    frm.ShowDialog();
                }
                else
                {
                    wpfAppliedLoanInfo frm = new wpfAppliedLoanInfo();
                    int num = Convert.ToInt32(getRow(dgLoan, 0));
                    frm.UserID = UserID;
                    frm.lId = num;
                    frm.status = status;
                    if (status == "Renewal Releasing")
                        frm.status = "Renewal";
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            try
            {
                rg();
                if (status == "Confirmation")
                    btnViewFolder.Visibility = Visibility.Visible;
                else
                    btnViewFolder.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnViewFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string folderName = @"F:\Loan Files\Applications Online";
                string lid = getRow(dgLoan, 0);
                string pathString = System.IO.Path.Combine(folderName, "Application " + lid);
                if (lid == "")
                {
                    return;
                }
                if (!Directory.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(pathString);
                    Process.Start(pathString);
                }
                else
                {
                    Process.Start(pathString);
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
