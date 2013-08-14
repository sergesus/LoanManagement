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

        private void checkDue()
        {
            using (var ctx = new MyContext())
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
            if (rdDue.IsChecked == true)
            {
                using (var ctx = new MyContext())
                {
                    var chq = from ch in ctx.FPaymentInfo
                              where ch.PaymentStatus == "Due"
                              select new { LoanID = ch.LoanID, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate };
                    dg.ItemsSource=chq.ToList();
                    var ctr = ctx.FPaymentInfo.Where(x => x.PaymentStatus == "Due").Count();
                    String str = "Number of cheques to Deposit : " + ctr.ToString();
                    
                    //lblTOL.Text = str.ToString();
                    //MessageBox.Show(str);
                    state="Dep";
                    btnDep.Content = "Deposit all cheques";
                }
            }
            else
            {
                using (var ctx = new MyContext())
                { 
                    var chq = from ch in ctx.FPaymentInfo
                              where ch.PaymentStatus=="Deposited"
                              select new { LoanID = ch.LoanID, ChecqueNumber = ch.PaymentNumber, ChequeID = ch.ChequeInfo, ClientName = ch.Loan.Client.FirstName + " " + ch.Loan.Client.LastName, PaymentDate = ch.PaymentDate, DateDeposited = ch.DepositedCheque.DepositDate };
                    dg.ItemsSource=chq.ToList();
                    btnDep.Content = "Void";
                    state="Undep";
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

        private void btnDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (state == "Dep")
                {
                    MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new MyContext())
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
                else
                {
                    using (var ctx = new MyContext())
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
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void rdDue_Checked(object sender, RoutedEventArgs e)
        {
            rg();
        }

        private void rdDeposited_Checked(object sender, RoutedEventArgs e)
        {
            rg();
        }
    }
}
