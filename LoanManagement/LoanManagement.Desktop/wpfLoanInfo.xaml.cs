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
using System.Text.RegularExpressions;

using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanInfo.xaml
    /// </summary>
    public partial class wpfLoanInfo : MetroWindow
    {
        public int UserID;
        public int lId;
        public String name;

        public wpfLoanInfo()
        {
            InitializeComponent();
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

            reset();
        }

        private void reset()
        {
            using (var ctx = new newerContext())
            {
                var lon = ctx.Loans.Find(lId);
                lblName.Content = name;
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
                lblHF.Content = ctx.FPaymentInfo.Where(x => x.PaymentStatus.Contains("Due") && x.LoanID==lId).Count().ToString();
                var ctr = ctx.FPaymentInfo.Where(x => x.PaymentStatus == "Cleared" && x.LoanID == lId).Count();
                if (ctr > 0)
                {
                    var lst = ctx.FPaymentInfo.Where(x => x.PaymentStatus == "Cleared" && x.LoanID == lId);
                    int n = 1;
                    var str = "";
                    foreach (var itm in lst)
                    {
                        if (n == ctr)
                        {
                            str = itm.ClearCheque.DateCleared.ToString().Split(' ')[0];
                        }
                    }
                    lblLastP.Content = str;
                }
                else
                {
                    lblLastP.Content = "-";
                }
                var sm = ctx.FPaymentInfo.Where(x => x.PaymentStatus != "Cleared" && x.LoanID == lId);
                double sum = 0;
                foreach (var itm in sm)
                {
                    sum = sum + itm.Amount;
                }
                lblRemaining.Content = sum.ToString("N2");
                var sm2 = ctx.FPaymentInfo.Where(x => x.PaymentStatus.Contains("Due") && x.LoanID == lId);
                sum = 0;
                foreach (var itm in sm2)
                {
                    sum = sum + itm.Amount;
                }
                lblBal.Content = sum.ToString("N2");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            wpfViewClientInfo frm = new wpfViewClientInfo();
            frm.status = "View2";
            frm.Height = 600;
            using (var ctx = new newerContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.cID = lon.ClientID;
            }
            frm.ShowDialog();
        }
    }
}
