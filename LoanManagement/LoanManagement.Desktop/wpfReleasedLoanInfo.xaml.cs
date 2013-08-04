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

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            wpfAgentInfo frm = new wpfAgentInfo();
            frm.status = "View";
            using (var ctx = new SystemContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.aId = lon.AgentID;
            }
            frm.ShowDialog();
        }

        public void reset()
        {
            using (var ctx = new SystemContext())
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
                          where rm.LoanID == lId && rm.PaymentStatus == "Paid"
                          select rm;
                double r = 0;
                foreach (var item in rmn)
                {
                    r = r + item.Amount;
                }
                double remain = lon.ReleasedLoan.TotalLoan - r;
                lblRemaining.Content = remain.ToString("N2");

                var chqs = from ge in ctx.FPaymentInfo
                           where ge.LoanID == lId
                           select new { No = ge.PaymentNumber, ChequeID = ge.ChequeInfo, TotalPayment = ge.Amount, ChequeDueDate = ge.ChequeDueDate , PaymentDate = ge.PaymentDate, Status = ge.PaymentStatus };
                
                
                dg.ItemsSource = chqs.ToList();

                if (status == "Holding")
                {
                    btnUpdate.Content = "Hold next cheque";
                    btnVoid.Content = "Unhold held cheque";
                    var dts = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Pending").First();
                    var dt = dts.PaymentDate.AddDays(-14);
                    lblSDt.Content = dt.ToString();
                    dt = dt.AddDays(11);
                    lblEDt.Content = dt.ToString(); ;
                }
                else
                {
                    lblS.Visibility = Visibility.Hidden;
                    lblE.Visibility = Visibility.Hidden;
                    lblSDt.Content = "";
                    lblEDt.Content = "";
                }
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
        }

        private void btnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (status == "UReleasing")
            {
                MessageBoxResult mr = System.Windows.MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new SystemContext())
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
        }
    }
}
