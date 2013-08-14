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
            using (var ctx = new MyContext())
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
                ctx.SaveChanges();
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            wpfAgentInfo frm = new wpfAgentInfo();
            frm.status = "View";
            using (var ctx = new MyContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.aId = lon.AgentID;
            }
            frm.ShowDialog();
        }

        public void reset()
        {
            using (var ctx = new MyContext())
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
                lblDt.Content = lon.ReleasedLoan.DateReleased;
                lblMode.Content = lon.Mode;
                var l = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                lblNextP.Content = l.PaymentDate.ToString();
                lblPayment.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                lblTOL.Content = lon.Service.Name;
                lblType.Content = lon.Service.Type;

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

                

                if (status == "Holding")
                {
                    btnUpdate.Content = "Hold next cheque";
                    btnVoid.Content = "Unhold held cheque";
                    var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && (x.PaymentStatus == "Pending" || x.PaymentStatus == "On Hold")).First();
                    var dt = dts.PaymentDate.AddDays(-14);
                    var dt1 = dt.AddDays(11);
                    if (dts.PaymentStatus == "On Hold")
                    {
                        dt = dts.PaymentDate.AddDays(-5);
                        dt1 = dt.AddDays(4);
                    }
                    lblSDt.Content = dt.ToString();
                    lblEDt.Content = dt1.ToString(); ;
                }
                else
                {
                    lblS.Visibility = Visibility.Hidden;
                    lblE.Visibility = Visibility.Hidden;
                    lblSDt.Content = "";
                    lblEDt.Content = "";
                }
                checkDue();
                var chqs = from ge in ctx.FPaymentInfo
                           where ge.LoanID == lId
                           select new { No = ge.PaymentNumber, ChequeID = ge.ChequeInfo, TotalPayment = ge.Amount, ChequeDueDate = ge.ChequeDueDate, PaymentDate = ge.PaymentDate, Status = ge.PaymentStatus };


                dg.ItemsSource = chqs.ToList();
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;

            reset();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
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
                using (var ctx = new MyContext())
                {
                    //var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                    if (DateTime.Today.Date > Convert.ToDateTime(lblEDt.Content) || DateTime.Today.Date < Convert.ToDateTime(lblSDt.Content))
                    {
                        System.Windows.MessageBox.Show("Unable to hold next cheque","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                        return;
                    }
                     MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

                         
                     }

                }
            }
        }

        private void btnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (status == "UReleasing")
            {
                MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new MyContext())
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
                        System.Windows.MessageBox.Show("Okay");
                        this.Close();
                    }
                }
            }
            else if (status == "Holding")
            {
                using (var ctx = new MyContext())
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
        }
    }
}
