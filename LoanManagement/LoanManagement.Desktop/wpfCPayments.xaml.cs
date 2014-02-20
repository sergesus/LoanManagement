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

using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfCPayments.xaml
    /// </summary>
    public partial class wpfCPayments : MetroWindow
    {
        public int UserID;

        public wpfCPayments()
        {
            InitializeComponent();
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            int lID;
            try
            {
                lID = Convert.ToInt32(txtID.Text);
            }
            catch (Exception)
            {
                lID = 0;
            }
            try
            {
                using (var ctx = new finalContext())
                {
                    var ctr = ctx.Loans.Where(x => x.LoanID == lID && x.Status == "Under Collection").Count();
                    if (ctr > 0)
                    {
                        txtAmt.IsEnabled = !false;
                        btnRecord.IsEnabled = !false;

                        var lon = ctx.Loans.Find(lID);

                        lblClient.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.Suffix;

                        lblTOL.Content = lon.Service.Name;

                        var mp = ctx.PassedToCollectors.Where(x => x.LoanID == lID).First();

                        lblTotalLoan.Content = mp.RemainingBalance.ToString("N2");


                    }
                    else
                    {
                        txtAmt.IsEnabled = false;
                        btnRecord.IsEnabled = false;
                        txtAmt.Text = "";
                        lblClient.Content = "";
                        lblTOL.Content = "";
                        lblTotalLoan.Content = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            double amt = 0;
            try
            {
                amt = Convert.ToDouble(txtAmt.Text);
                amt = Convert.ToDouble(amt.ToString("N2"));
            }
            catch (Exception)
            {
                amt = 0;
            }
            if (amt > Convert.ToDouble(lblTotalLoan.Content))
            {
                System.Windows.MessageBox.Show("Amount must not be greater that the remaining amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (amt < 1)
            {
                System.Windows.MessageBox.Show("Amount must be greater that 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int lID;
            try
            {
                lID = Convert.ToInt32(txtID.Text);
            }
            catch (Exception)
            {
                lID = 0;
            }

            using (var ctx = new finalContext())
            {
                CollectionInfo ci = new CollectionInfo { LoanID = lID, DateCollected = DateTime.Today.Date, TotalCollection = amt };
                var r = ctx.PassedToCollectors.Find(lID);
                double rBal = Convert.ToDouble((r.RemainingBalance - amt).ToString("N2"));
                r.RemainingBalance = rBal;
                ctx.CollectionInfoes.Add(ci);

                if (rBal <= 0)
                {
                    System.Windows.MessageBox.Show("The Loan has been successfully finished!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    var lon = ctx.Loans.Find(lID);
                    lon.Status = "Paid";
                    r.RemainingBalance = 0;
                    PaidLoan pl = new PaidLoan { LoanID = lID, DateFinished = DateTime.Today.Date };
                    ctx.PaidLoans.Add(pl);
                }

                ctx.SaveChanges();
                txtID.Text = "";
                txtID.Focus();
            }

        }
    }
}
