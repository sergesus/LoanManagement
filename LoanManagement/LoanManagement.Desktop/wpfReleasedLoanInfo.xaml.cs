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
    /// Interaction logic for wpfReleasedLoanInfo.xaml
    /// </summary>
    public partial class wpfReleasedLoanInfo : MetroWindow
    {
        public int lId;
        public string status;

        public wpfReleasedLoanInfo()
        {
            InitializeComponent();
        }

        private void checkDue()
        {
            try
            {
                using (var ctx = new iContext())
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
                               where lo.Status == "Released"
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


                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgentInfo frm = new wpfAgentInfo();
                frm.status = "View";
                using (var ctx = new iContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    frm.aId = lon.AgentID;
                }
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void reset()
        {
            try
            {
                using (var ctx = new iContext())
                {
                    var lon = ctx.Loans.Find(lId);

                    byte[] imageArr;
                    imageArr = lon.Client.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;

                    lblAmt.Content = lon.ReleasedLoan.Principal.ToString("N2");
                    string[] mdt = lon.ReleasedLoan.DateReleased.ToString().Split(' ');
                    string d = mdt[0];
                    lblDt.Content = d;
                    lblMode.Content = lon.Mode;
                    lblTOL.Content = lon.Service.Name;
                    lblType.Content = lon.Service.Type;
                    var ctr = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "Due/Pending")).Count();
                    if (ctr > 0)
                    {
                        var l = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "Due/Pending")).First();
                        lblNextP.Content = l.PaymentDate.ToString().Split(' ')[0];
                        lblPayment.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                        double hf = lon.ReleasedLoan.MonthlyPayment * (lon.Service.Holding / 100);
                        lblHF.Content = hf.ToString("N2");
                        if (status == "Holding")
                        {
                            lbl1.Content = "Hold next cheque";
                            lbl2.Content = "Unhold held cheque";
                            var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                            var dt = dts.PaymentDate.AddDays(-14);
                            var dt1 = dt.AddDays(11);
                            if (dts.PaymentStatus == "On Hold")
                            {
                                dt = dts.PaymentDate.AddDays(-5);
                                dt1 = dt.AddDays(4);
                            }
                            mdt = dt.ToString().Split(' ');
                            lblSDt.Content = mdt[0];
                            mdt = dt1.ToString().Split(' ');
                            lblEDt.Content = mdt[0];
                        }
                        else if (status == "Adjustment")
                        {
                            lbl1.Content = "Adjust Payment";
                            lbl2.Content = "Void Adjustment";
                            var ctr1 = ctx.AdjustedLoans.Where(x => x.LoanID == lId).Count();
                            if (ctr1 > 0)
                            {
                                btnUpdate.Visibility = Visibility.Hidden;
                                btnVoid.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                btnUpdate.Visibility = Visibility.Visible;
                                btnVoid.Visibility = Visibility.Hidden;
                            }

                        }
                        else if (status == "Restructure")
                        {
                            btnUpdate.Content = "Restructure Loan";
                            btnVoid.Visibility = Visibility.Hidden;
                            /*btnVoid.Content = "Void Adjustment";
                            var ctr1 = ctx.AdjustedLoans.Where(x => x.LoanID == lId).Count();
                            if (ctr1 > 0)
                            {
                                btnUpdate.Visibility = Visibility.Hidden;
                                btnVoid.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                btnUpdate.Visibility = Visibility.Visible;
                                btnVoid.Visibility = Visibility.Hidden;
                            }*/

                        }
                        else
                        {
                            lblS.Visibility = Visibility.Hidden;
                            lblE.Visibility = Visibility.Hidden;
                            lblSDt.Content = "";
                            lblEDt.Content = "";
                        }
                    }
                    else
                    {
                        lblNextP.Content = "-";
                        lblPayment.Content = "-";
                        lblHF.Content = "-";
                    }

                    var rmn = from rm in ctx.FPaymentInfo
                              where rm.LoanID == lId && rm.PaymentStatus == "Cleared"
                              select rm;
                    double r = 0;
                    foreach (var item in rmn)
                    {
                        r = r + item.Amount;
                    }
                    double remain = lon.ReleasedLoan.TotalLoan - r;
                    lblRemaining.Content = remain.ToString("N2");

                    if (status != "Holding")
                    {
                        lblS.Visibility = Visibility.Hidden;
                        lblE.Visibility = Visibility.Hidden;
                        lblSDt.Content = "";
                        lblEDt.Content = "";
                    }

                    var c2 = ctx.RestructuredLoans.Where(x => x.NewLoanID == lon.LoanID).Count();
                    if (c2 > 0)
                    {
                        var lon2 = ctx.RestructuredLoans.Where(x => x.NewLoanID == lon.LoanID).First();
                        lblRes2.Content = "Amount Restructured: ";
                        lblAmt.Content = lon2.Loan.ReleasedLoan.TotalLoan;
                    }


                    checkDue();
                    var chqs = from ge in ctx.FPaymentInfo
                               where ge.LoanID == lId
                               select new { No = ge.PaymentNumber, ChequeID = ge.ChequeInfo, TotalPayment = ge.Amount, ChequeDueDate = ge.ChequeDueDate, PaymentDate = ge.PaymentDate, Status = ge.PaymentStatus, DateCleared = ge.ClearCheque.DateCleared };

                    ctx.Database.ExecuteSqlCommand("delete from dbo.ViewLoans");
                    foreach (var i in chqs)
                    {
                        string s;
                        string[] dtCleared=null;
                        string myStr;
                        if (i.DateCleared != null)
                        {
                            s = i.DateCleared.ToString();
                            dtCleared = s.Split(' ');
                            myStr = dtCleared[0];
                        }
                        else
                        {
                            myStr = "";
                        }
                        s = i.PaymentDate.ToString();
                        string[] dtPayment = s.Split(' ');
                        s = i.ChequeDueDate.ToString();
                        string[] dtDue = s.Split(' ');
                        ViewLoan vl = new ViewLoan { DateCleared = myStr, DueDate = dtDue[0], PaymentDate= dtPayment[0], PaymentInfo= i.ChequeID, PaymentNumber=i.No, Status=i.Status, TotalPayment=i.TotalPayment.ToString("N2")};
                        ctx.ViewLoans.Add(vl);
                    }
                    ctx.SaveChanges();
                    var chqs1 = from ge in ctx.ViewLoans
                               select new { No = ge.PaymentNumber, ChequeID = ge.PaymentInfo, TotalPayment = ge.TotalPayment, ChequeDueDate = ge.DueDate, PaymentDate = ge.PaymentDate, Status = ge.Status, DateCleared = ge.DateCleared };
                    dg.ItemsSource = chqs1.ToList();

                    if (status == "View")
                    {
                        btnFull.Visibility = Visibility.Hidden;
                        btnUpdate.Visibility = Visibility.Hidden;
                        btnVoid.Visibility = Visibility.Hidden;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
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
                using (var ctx = new iContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    lblName.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName;
                }
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "UReleasing")
                {
                    wpfFReleasing frm = new wpfFReleasing();
                    frm.status = "UReleasing";
                    frm.lId = lId;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "Holding")
                {
                    using (var ctx = new iContext())
                    {
                        //var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                        if (DateTime.Today.Date > Convert.ToDateTime(lblEDt.Content) || DateTime.Today.Date < Convert.ToDateTime(lblSDt.Content))
                        {
                            System.Windows.MessageBox.Show("Unable to hold next cheque", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        /*MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                            double hFee = dts.Amount * (dts.Loan.Service.Holding / 100);
                            hFee = Convert.ToDouble(hFee.ToString("N2"));
                            HeldCheque hc = new HeldCheque { DateHeld = DateTime.Today.Date, LoanID = lId, NewPaymentDate = dts.PaymentDate.AddDays(7), OriginalPaymentDate = dts.PaymentDate, PaymentNumber = dts.PaymentNumber, HoldingFee = hFee };
                            dts.PaymentDate = dts.PaymentDate.AddDays(7);
                            dts.PaymentStatus = "On Hold";
                            ctx.HeldCheques.Add(hc);
                            ctx.SaveChanges();
                            reset();

                            System.Windows.MessageBox.Show("Okay");


                        }*/
                        wpfCheckout frm = new wpfCheckout();
                        frm.lId = lId;
                        frm.status = "Holding";
                        frm.ShowDialog();

                    }
                }
                else if (status == "Adjustment")
                {
                    wpfPaymentAdjustment frm = new wpfPaymentAdjustment();
                    frm.lId = lId;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "Restructure")
                {
                    wpfLoanRestructure frm = new wpfLoanRestructure();
                    frm.lId = lId;
                    this.Close();
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "UReleasing")
                {
                    MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new iContext())
                        {
                            var lon = ctx.Loans.Find(lId);
                            lon.Status = "Approved";
                            ctx.ReleasedLoans.Remove(lon.ReleasedLoan);
                            var lons = from lo in ctx.FPaymentInfo
                                       where lo.LoanID == lId
                                       select lo;
                            foreach (var item in lons)
                            {
                                ctx.FPaymentInfo.Remove(item);
                            }
                            ctx.SaveChanges();
                            this.Close();
                            System.Windows.MessageBox.Show("Okay");
                            
                        }
                    }
                }
                else if (status == "Holding")
                {
                    using (var ctx = new iContext())
                    {
                        var ctrs = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "On Hold").Count();
                        if (ctrs < 1)
                        {
                            System.Windows.MessageBox.Show("No cheque to unhold", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        //var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                        MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mr == MessageBoxResult.Yes)
                        {
                            var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                            var dt = ctx.HeldCheques.Where(x => x.LoanID == lId && x.PaymentNumber == dts.PaymentNumber && x.NewPaymentDate == dts.PaymentDate).First();
                            dts.PaymentDate = dt.OriginalPaymentDate;
                            dts.PaymentStatus = "Pending";
                            ctx.HeldCheques.Remove(dt);
                            ctx.SaveChanges();
                            reset();

                            System.Windows.MessageBox.Show("Okay");


                        }

                    }
                }
                else if (status == "Adjustment")
                {
                    MessageBoxResult mr = System.Windows.MessageBox.Show("Sure?", "Question", MessageBoxButton.YesNo);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new iContext())
                        {
                            AdjustedLoan al = ctx.AdjustedLoans.Find(lId);
                            var py = from p in ctx.FPaymentInfo
                                     where p.PaymentStatus != "Cleared" && p.LoanID == lId
                                     select p;
                            double num = al.Days * (-1);
                            foreach (var item in py)
                            {
                                item.PaymentDate = item.PaymentDate.AddDays(num);
                            }
                            ctx.AdjustedLoans.Remove(al);
                            ctx.SaveChanges();
                            System.Windows.MessageBox.Show("Okay");
                            this.Close();
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

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            wpfViewClientInfo frm = new wpfViewClientInfo();
            frm.status = "View2";
            frm.Height = 600;
            using (var ctx = new iContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.cID = lon.ClientID;
            }
            frm.ShowDialog();
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            reset();
        }
    }
}
