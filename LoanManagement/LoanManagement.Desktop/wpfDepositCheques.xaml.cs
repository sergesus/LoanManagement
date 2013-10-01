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
using System.Data.Entity;
using LoanManagement.Domain;
//using LoanManagement.Domain;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfDepositCheques.xaml
    /// </summary>
    public partial class wpfDepositCheques : MetroWindow
    {
        public wpfDepositCheques()
        {
            InitializeComponent();
        }

        public string state;
        public string status;

        private void checkDue()
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = from lo in ctx.FPaymentInfo
                              where lo.PaymentDate <= DateTime.Today.Date && (lo.PaymentStatus == "Pending" || lo.PaymentStatus == "On Hold")
                              select lo;
                    foreach (var item in lon)
                    {
                        var ctr = ctx.FPaymentInfo.Where(x => (x.PaymentDate <= DateTime.Today.Date && x.LoanID == item.LoanID) && (x.PaymentStatus == "Due" || x.PaymentStatus == "Returned")).Count();
                        if (ctr == 0)
                        {
                            item.PaymentStatus = "Due";
                        }
                        else
                        {
                            item.PaymentStatus = "Due/Pending";
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

        private void rg()
        {
            try
            {
                if (rdDue.IsChecked == true)
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var chq = from ch in ctx.FPaymentInfo
                                  where ch.PaymentStatus == "Due"
                                  select new { LoanID = ch.LoanID, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate };
                        dg.ItemsSource = chq.ToList();
                        var ctr = ctx.FPaymentInfo.Where(x => x.PaymentStatus == "Due").Count();
                        String str = "Number of cheques to Deposit : " + ctr.ToString();

                        //lblTOL.Text = str.ToString();
                        //MessageBox.Show(str);
                        state = "Dep";
                        btnDep.Content = "Deposit all cheques";
                    }
                }
                else if (rdDeposited.IsChecked == true)
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var chq = from ch in ctx.FPaymentInfo
                                  where ch.PaymentStatus == "Deposited"
                                  select new { LoanID = ch.LoanID, No = ch.PaymentNumber, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate, DateDeposited = ch.DepositedCheque.DepositDate };
                        dg.ItemsSource = chq.ToList();
                        if (status == "Deposit")
                            btnDep.Content = "Void";
                        else if (status == "Returning")
                            btnDep.Content = "Mark as Returned";
                        state = "Undep";
                    }
                }
                else
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var chq = from ch in ctx.FPaymentInfo
                                  where ch.PaymentStatus == "Returned"
                                  select new { LoanID = ch.LoanID, No = ch.PaymentNumber, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate, DateDeposited = ch.DepositedCheque.DepositDate };
                        dg.ItemsSource = chq.ToList();
                        if (status == "Deposit")
                            btnDep.Content = "Re-deposit";
                        else if (status == "Returning")
                            btnDep.Content = "Void Returning";
                        state = "Redep";
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
                rdDue.IsChecked = true;
                if (status == "Returning")
                {
                    rdDue.Visibility = Visibility.Hidden;
                    rdDeposited.IsChecked = true;
                }
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Deposit")
                {
                    if (state == "Dep")
                    {
                        MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            using (var ctx = new MyLoanContext())
                            {
                                var chq = from ch in ctx.FPaymentInfo
                                          where ch.PaymentStatus == "Due"
                                          select ch;
                                foreach (var item in chq)
                                {
                                    /*var ctr = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Due/Pending").Count();
                                    if (ctr != 0)
                                    {
                                        var hq = ctx.FPaymentInfo.Where(x => x.LoanID == item.LoanID && x.PaymentStatus == "Due/Pending").First();
                                        hq.PaymentStatus = "Due";
                                    }*/
                                    var ch = ctx.DepositedCheques.Where(x => x.FPaymentInfoID == item.FPaymentInfoID).Count();
                                    if (ch > 0)
                                    {
                                        item.DepositedCheque.DepositDate = DateTime.Today.Date;
                                        item.PaymentStatus = "Deposited";
                                    }
                                    else
                                    {
                                        item.PaymentStatus = "Deposited";
                                        DepositedCheque dc = new DepositedCheque { DepositDate = DateTime.Today.Date, FPaymentInfoID = item.FPaymentInfoID };
                                        ctx.DepositedCheques.Add(dc);
                                    }
                                }
                                ctx.SaveChanges();
                                MessageBox.Show("Okay");
                                rg();
                            }
                        }
                    }
                    else if (state == "Undep")
                    {
                        using (var ctx = new MyLoanContext())
                        {
                            int id = Convert.ToInt32(getRow(dg, 0));
                            int num = Convert.ToInt32(getRow(dg, 1));
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                            DepositedCheque dp = ctx.DepositedCheques.Find(fp.FPaymentInfoID);
                            MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mr == MessageBoxResult.Yes)
                            {
                                fp.PaymentStatus = "Due";
                                ctx.DepositedCheques.Remove(dp);
                                ctx.SaveChanges();
                                MessageBox.Show("Okay");
                                checkDue();
                                rg();
                                rdDue.IsChecked = true;
                            }
                        }
                    }
                    else if (state == "Redep")
                    {
                        using (var ctx = new MyLoanContext())
                        {
                            int id = Convert.ToInt32(getRow(dg, 0));
                            int num = Convert.ToInt32(getRow(dg, 1));
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                            DepositedCheque dp = ctx.DepositedCheques.Find(fp.FPaymentInfoID);
                            MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mr == MessageBoxResult.Yes)
                            {
                                fp.PaymentStatus = "Deposited";
                                dp.DepositDate = DateTime.Today.Date;
                                ctx.SaveChanges();
                                MessageBox.Show("Okay");
                                checkDue();
                                rg();
                                //rdDue.IsChecked = true;
                            }
                        }
                    }
                }
                else if(status == "Returning")
                {
                    if (state == "Undep")//for returning
                    {
                        int id = Convert.ToInt32(getRow(dg, 0));
                        int num = Convert.ToInt32(getRow(dg, 1));
                        using (var ctx = new MyLoanContext())
                        {
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();

                            var ctr = ctx.ReturnedCheques.Where(x => x.FPaymentInfoID==fp.FPaymentInfoID).Count();

                            if (ctr > 0)
                            {
                                MessageBox.Show("Cheque has already been returned once");
                                return;
                            }

                            wpfChequeReturning frm = new wpfChequeReturning();
                            frm.fId = fp.FPaymentInfoID;
                            frm.ShowDialog();
                        }
                    }
                    else if (state == "Redep")//for voiding
                    {
                        int id = Convert.ToInt32(getRow(dg, 0));
                        int num = Convert.ToInt32(getRow(dg, 1));
                        using (var ctx = new MyLoanContext())
                        {
                            FPaymentInfo fp = ctx.FPaymentInfo.Where(x => x.LoanID == id && x.PaymentNumber == num).First();
                            ReturnedCheque rc = ctx.ReturnedCheques.Find(fp.FPaymentInfoID);
                            fp.PaymentStatus = "Deposited";
                            ctx.ReturnedCheques.Remove(rc);
                            ctx.SaveChanges();
                            MessageBox.Show("Okay");
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

        private void rdDue_Checked(object sender, RoutedEventArgs e)
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

        private void rdDeposited_Checked(object sender, RoutedEventArgs e)
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

        private void wdw1_Activated(object sender, EventArgs e)
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
    }
}
