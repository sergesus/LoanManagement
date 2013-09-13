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

using LoanManagement.Domain;
using MahApps.Metro.Controls;
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfChequeClearing.xaml
    /// </summary>
    public partial class wpfChequeClearing : MetroWindow
    {
        public wpfChequeClearing()
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
                using (var ctx = new iContext())
                {
                    var chq = from ch in ctx.FPaymentInfo
                              where ch.PaymentStatus == "Deposited"
                              && !(from o in ctx.TempClearings select o.FPaymentInfoID).Contains(ch.FPaymentInfoID)
                              select new { PaymentID = ch.FPaymentInfoID, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, TypeOfLoan = ch.Loan.Service.Name };
                    dg1.ItemsSource = chq.ToList();

                    var chq1 = from ch in ctx.TempClearings
                               select new { PaymentID = ch.FPaymentInfoID, ChequeID = ch.FPaymentInfo.ChequeInfo, ClientName = ch.FPaymentInfo.Loan.Client.FirstName + " " + ch.FPaymentInfo.Loan.Client.LastName, TypeOfLoan = ch.FPaymentInfo.Loan.Service.Name };
                    dg2.ItemsSource = chq1.ToList();
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

                using (var ctx = new iContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete  from dbo.TempClearings");
                }
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
                {
                    int n = Convert.ToInt32(getRow(dg1,0));
                    TempClearing tc = new TempClearing { FPaymentInfoID = n };
                    ctx.TempClearings.Add(tc);
                    ctx.SaveChanges();
                    rg();
                }
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
                using (var ctx = new iContext())
                {
                    int n = Convert.ToInt32(getRow(dg2, 0));
                    TempClearing tc = ctx.TempClearings.Where(x => x.FPaymentInfoID == n).First();
                    ctx.TempClearings.Remove(tc);
                    ctx.SaveChanges();
                    rg();
                }
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
                using (var ctx = new iContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete  from dbo.TempClearings");
                }
                rg();
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
                using (var ctx = new iContext())
                {
                    //ctx.Database.ExecuteSqlCommand("delete  from dbo.TempClearings");
                    var chq = from ch in ctx.FPaymentInfo
                              where ch.PaymentStatus == "Deposited"
                              && !(from o in ctx.TempClearings select o.FPaymentInfoID).Contains(ch.FPaymentInfoID)
                              select ch;
                    foreach (var item in chq)
                    {
                        TempClearing tc = new TempClearing { FPaymentInfoID = item.FPaymentInfoID };
                        ctx.TempClearings.Add(tc);
                    }
                    ctx.SaveChanges();
                    rg();
                }
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
            MessageBoxResult mr = MessageBox.Show("You sure?","Question",MessageBoxButton.YesNo);
            if (mr == MessageBoxResult.Yes)
            {
                using (var ctx = new iContext())
                {
                    var chq = from ch in ctx.TempClearings
                              select ch;
                    foreach (var item in chq)
                    {
                        var ch = ctx.FPaymentInfo.Find(item.FPaymentInfoID);
                        ch.PaymentStatus = "Cleared";
                        var ctr = ctx.FPaymentInfo.Where(x=> x.LoanID==item.FPaymentInfo.LoanID && x.PaymentStatus=="Due/Pending").Count();
                        if (ctr > 0)
                        {
                            var ch2 = ctx.FPaymentInfo.Where(x => x.LoanID == item.FPaymentInfo.LoanID && x.PaymentStatus == "Due/Pending").First();
                            ch2.PaymentStatus = "Due";
                        }
                        ClearedCheque cc = new ClearedCheque { FPaymentInfoID = item.FPaymentInfoID, DateCleared = DateTime.Today.Date };
                        ctx.ClearedCheques.Add(cc);
                    }
                    ctx.SaveChanges();
                    MessageBox.Show("Okay");
                    this.Close();
                }
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
