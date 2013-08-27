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

using MahApps.Metro.Controls;
using LoanManagement.Domain;
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanSearch.xaml
    /// </summary>
    public partial class wpfLoanSearch : MetroWindow
    {
        public string status;

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
            if (status == "UApproval")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Approved" || ln.Status=="Declined"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();

                }
            }
            else if(status=="Approval" || status=="Application")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Applied"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();

                }
            }
            else if (status == "Releasing")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Approved"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();

                }
            }
            else if (status == "UReleasing")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Released"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();

                }
            }
            else if (status == "Holding")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Released" || ln.Status=="Active"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();

                }
            }
            else if (status == "Voiding")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Released" || ln.Status == "Active"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();
                    btnView.Content = "Void last payment";
                }
            }
            else if (status == "VoidClosed")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Closed Account"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();
                    btnView.Content = "Void Closed Account";
                }
            }
            else if (status == "Renewal")
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from ln in ctx.Loans
                              where ln.Status == "Closed Account"
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, FirstName = ln.Client.FirstName, MiddleName = ln.Client.MiddleName, LastName = ln.Client.LastName };
                    dgLoan.ItemsSource = lon.ToList();
                    btnView.Content = "Renew Closed Account";
                }
            }
            
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
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

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Application")
            {
                wpfLoanApplication frm = new wpfLoanApplication();
                frm.status = "Edit";
                int id = Convert.ToInt32(getRow(dgLoan, 0));
                frm.lId = id;
                frm.btnContinue.Content = "Update";
                frm.ShowDialog();
            }
            else if (status == "UReleasing")
            {
                wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                int num = Convert.ToInt32(getRow(dgLoan, 0));
                frm.lId = num;
                frm.status = status;
                frm.ShowDialog();
            }
            else if (status == "Holding")
            {
                wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                int num = Convert.ToInt32(getRow(dgLoan, 0));
                frm.lId = num;
                frm.status = status;
                frm.ShowDialog();
            }
            else if (status == "Voiding")
            {
                using (var ctx = new MyLoanContext())
                {
                    int n = Convert.ToInt32(getRow(dgLoan,0));
                    int num = ctx.FPaymentInfo.Where(x => x.LoanID == n && x.PaymentStatus == "Deposited").Count();
                    if (num > 0)
                    {
                        MessageBox.Show("Unable to void payment due to deposited cheque");
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
                            if (x == num-1)
                            {
                                fp = item;
                            }
                            x++;
                        }
                        //var fp = ctx.FPaymentInfo.Where(x => x.LoanID == n && x.PaymentStatus == "Cleared").Last();
                        MessageBox.Show("The last payment has the following info: \n No :" + fp.PaymentNumber + " \n ChequeNumber: " + fp.ChequeInfo + " \n Amount: " + fp.Amount.ToString("N2") + "");
                        MessageBoxResult mr = MessageBox.Show("You sure?","Question",MessageBoxButton.YesNo);
                        if (mr == MessageBoxResult.Yes)
                        {
                            fp.PaymentStatus = "Deposited";
                            ClearedCheque cc = ctx.ClearedCheques.Find(fp.ClearCheque.FPaymentInfoID);
                            ctx.ClearedCheques.Remove(cc);
                            ctx.SaveChanges();
                            MessageBox.Show("Okay");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No payments to void");
                    }
                }
            }
            else if (status == "VoidClosed")
            {
                int n = Convert.ToInt32(getRow(dgLoan, 0));
                MessageBoxResult mr = MessageBox.Show("You sure?","Question",MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    
                    using (var ctx = new MyLoanContext())
                    {
                        var lon = ctx.Loans.Find(n);
                        lon.Status = "Released";//active
                        int num = ctx.ClosedAccounts.Where(x => x.LoanID == n).Count();
                        var fp1 = from f in ctx.ClosedAccounts
                                  where f.LoanID == n
                                  select f;
                        int y = 0;
                        ClosedAccount fp = null;
                        foreach (var item in fp1)
                        {
                            if (y == num - 1)
                            {
                                fp = item;
                            }
                            y++;
                        }
                        ctx.ClosedAccounts.Remove(fp);
                        var fp2 = from f in ctx.FPaymentInfo
                                 where f.LoanID == n && f.PaymentStatus != "Cleared"
                                 select f;
                        foreach (var item in fp2)
                        {
                            item.PaymentStatus = "Pending";
                        }
                        ctx.SaveChanges();
                        MessageBox.Show("Okay");
                        return;
                    }
                }
            }
            else if (status == "Renewal")
            {
                int n = Convert.ToInt32(getRow(dgLoan, 0));
                wpfRenewClosed frm = new wpfRenewClosed();
                frm.lId = n;
                this.Close();
                frm.ShowDialog();
            }
            else
            {
                wpfAppliedLoanInfo frm = new wpfAppliedLoanInfo();
                int num = Convert.ToInt32(getRow(dgLoan, 0));
                frm.lId = num;
                frm.status = status;
                frm.ShowDialog();
            }
            
            
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            rg();
        }
    }
}
