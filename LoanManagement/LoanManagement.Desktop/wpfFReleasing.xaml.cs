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
    /// Interaction logic for wpfFReleasing.xaml
    /// </summary>
    public partial class wpfFReleasing : MetroWindow
    {
        public string status;
        public int lId;
        public int bId;
        public int myNum;
        public TextBox[] textarray = new TextBox[0];
        public wpfFReleasing()
        {
            InitializeComponent();
        }

        private void refresh()
        {
            if (status == "UReleasing")
            {
                using (var ctx = new MyContext())
                {
                    var lons = from ge in ctx.FPaymentInfo
                               where ge.LoanID == lId
                               select ge;
                        myNum = ctx.FPaymentInfo.Where(x => x.LoanID == lId).Count();
                }
            }

            textarray = new TextBox[myNum];
            Label[] labelarray = new Label[myNum];
            StackPanel[] sp = new StackPanel[myNum];
            stck.Children.Clear();
            for (int ctr = 0; ctr < myNum; ctr++)
            {
                labelarray[ctr] = new Label();
                labelarray[ctr].Height = 30;
                //labelarray[ctr].Width = 50;
                labelarray[ctr].FontSize = 16;
                labelarray[ctr].Content = "Cheque No. " + (ctr + 1).ToString();
                textarray[ctr] = new TextBox();
                textarray[ctr].Height = 25;
                textarray[ctr].Width = 300;
                textarray[ctr].FontSize = 16;
                sp[ctr] = new StackPanel();
                sp[ctr].Width = 300;
                sp[ctr].Height = 60;
                sp[ctr].Children.Add(labelarray[ctr]);
                sp[ctr].Children.Add(textarray[ctr]);
                stck.Children.Add(sp[ctr]);

            }

        }

        private void refr()
        {
            try
            {
                if (status == "Releasing")
                {
                    using (var ctx = new MyContext())
                    {
                        myNum = 0;
                        ctx.Database.ExecuteSqlCommand("delete from dbo.GenSOAs");
                        ctx.SaveChanges();
                        var lon = ctx.Loans.Find(lId);
                        Double Amt = Convert.ToDouble(txtAmt.Text);
                        //lblPrincipal.Content = Amt.ToString("N2");
                        txtAmt.Text = Amt.ToString("N2");
                        txtAmt.SelectionStart = txtAmt.Text.Length - 3;
                        Double TotalInt = lon.Service.Interest * Convert.ToInt32(txtTerm.Text);
                        TotalInt = TotalInt / 100;
                        double ded = lon.Service.AgentCommission;
                        var dec = from de in ctx.Deductions
                                  where de.ServiceID == lon.ServiceID
                                  select de;
                        foreach (var item in dec)
                        {
                            ded = ded + item.Percentage;
                        }
                        Double Deduction = ded / 100;
                        Double NetProceed = (Convert.ToDouble(txtAmt.Text)) - (Convert.ToDouble(txtAmt.Text) * Deduction);
                        Double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);
                        lblProceed.Content = NetProceed.ToString("N2");
                        lblInt.Content = WithInt.ToString("N2");
                        Double Payment = 0;
                        DateTime dt = DateTime.Today.Date;
                        double Interval = 0;
                        DateInterval dInt = DateInterval.Month;
                        Double Remaining = WithInt;
                        if (cmbMode.Text == "Monthly")
                        {
                            Interval = 1;
                            dInt = DateInterval.Month;
                            Payment = WithInt / Convert.ToInt32(txtTerm.Text);
                        }
                        else if (cmbMode.Text == "Semi-Monthly")
                        {
                            Interval = 15;
                            dInt = DateInterval.Day;
                            Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 2);
                        }
                        else if (cmbMode.Text == "Weekly")
                        {
                            Interval = 7;
                            dInt = DateInterval.Day;
                            Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 4);
                        }
                        else if (cmbMode.Text == "Daily")
                        {
                            Interval = 1;
                            dInt = DateInterval.Day;
                            Payment = WithInt / ((Convert.ToInt32(txtTerm.Text) * 4) * 7);
                        }
                        else if (lon.Mode == "One-Time Payment")
                        {
                            NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                            lblProceed.Content = NetProceed.ToString("N2");
                            lblInt.Content = txtAmt.Text;
                            Remaining = Convert.ToDouble(lblPrincipal.Content);
                            Payment = Remaining;
                            Interval = Convert.ToInt32(txtTerm.Text);
                            dInt = DateInterval.Month;
                            lbl4.Content = "Total Payment";
                        }

                        dt = DateAndTime.DateAdd(dInt, Interval, dt);
                        
                        int num = 1;
                        while (Remaining > 1)
                        {
                            //System.Windows.MessageBox.Show(num.ToString());
                            Remaining = Remaining - Payment;
                            GenSOA soa = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = Remaining.ToString("N2") };
                            ctx.GenSOA.Add(soa);
                            num++;
                            //System.Windows.MessageBox.Show(Remaining.ToString());
                            dt = DateAndTime.DateAdd(dInt, Interval, dt);
                            myNum++;
                        }
                        lblMonthly.Content = Payment.ToString("N2");
                        ctx.SaveChanges();
                        var gen = from ge in ctx.GenSOA
                                  select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                        dgSOA.ItemsSource = gen.ToList();
                        refresh();
                        return;
                    }
                }
                else if (status == "UReleasing")
                {
                    using (var ctx = new MyContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var lons = from ge in ctx.FPaymentInfo
                                   where ge.LoanID == lId
                                   select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                        dgSOA.ItemsSource = lons.ToList();
                        txtAmt.Text = lon.ReleasedLoan.TotalLoan.ToString("N2");
                        lblMonthly.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                        lblPrincipal.Content = txtAmt.Text;
                        lblProceed.Content = lon.ReleasedLoan.NetProceed.ToString("N2");
                        myNum = ctx.FPaymentInfo.Where(x => x.LoanID == lId).Count();
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.SelectedItem = lon.Mode;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
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

            //num = 0;
            if (status == "Releasing")
            {
                using (var ctx = new MyContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    var ser = ctx.Services.Find(lon.ServiceID);
                    cmbMode.Items.Clear();
                    Double Amt = Convert.ToDouble(lon.ApprovedLoan.AmountApproved);
                    lblPrincipal.Content = Amt.ToString("N2");
                    txtAmt.Text = Amt.ToString("N2");
                    if (ser.Department == "Financing")
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "One-Time Payment" });
                    }
                    else if (ser.Department == "Micro Business")
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Weekly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Daily" });
                    }
                    else
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Weekly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Daily" });
                    }
                    txtTerm.Text = lon.Term.ToString();
                    cmbMode.Text = lon.Mode;
                    refr();
                    cmbBank.Items.Clear();
                    var ban = from ba in ctx.Banks
                              where ba.Active == true
                              select ba;
                    foreach (var item in ban)
                    {
                        cmbBank.Items.Add(item.BankName);
                    }
                    //MessageBox.Show(myNum.ToString());
                    refresh();
                }
            }
            else if (status == "UReleasing")
            {

                txtAmt.IsEnabled = false;
                txtTerm.IsEnabled = false;
                cmbMode.IsEnabled = false;
                btnRef.IsEnabled = false;
                btnRelease.Content = "Update";
                refresh();
                refr();
                int myCtr = 0;
                using (var ctx = new MyContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    var bnk = ctx.Banks.Find(lon.BankID);
                    cmbBank.Items.Clear();
                    var ban = from ba in ctx.Banks
                              where ba.Active == true
                              select ba;
                    foreach (var item in ban)
                    {
                        cmbBank.Items.Add(item.BankName);
                    }
                    //cmbBank.SelectedIndex = 0;
                    cmbBank.Text = bnk.BankName;
                    cmbMode.Items.Clear();
                    cmbMode.Items.Add(lon.Mode);
                    cmbMode.Text = lon.Mode;
                    var lons = from lo in ctx.FPaymentInfo
                               where lo.LoanID == lId
                               select lo;
                    foreach (var item in lons)
                    {
                        //MessageBox.Show(myCtr.ToString());
                        textarray[myCtr].Text = item.ChequeInfo;
                        myCtr++;
                    }
                }
            }
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            refr();
        }

        private void cmbBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*using (var ctx = new MyContext())
            { 
                ComboBoxItem typeItem = (ComboBoxItem)cmbBank.SelectedItem;
                string value = typeItem.Content.ToString();
                var ban = ctx.Banks.Where(x => x.BankName == value).First();
                bId = ban.BankID;
            }*/
        }

        private void cmbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refr();
        }

        private void cmbMode_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cmbMode_DropDownClosed(object sender, EventArgs e)
        {
            refr();
        }

        private void txtTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            refr();
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Releasing")
            {
                if (Convert.ToDouble(txtAmt.Text) > Convert.ToDouble(lblPrincipal.Content))
                {
                    MessageBox.Show("Principal amount must not be greater than the maximum loanable amount","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult mr = MessageBox.Show("Are you sure?","Question",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new MyContext())
                    {
                        var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                        int bId = bk.BankID;
                        var lon = ctx.Loans.Find(lId);
                        lon.Status = "Released";
                        lon.BankID = bId;
                        lon.Mode = cmbMode.Text;
                        lon.Term = Convert.ToInt32(txtTerm.Text);
                        var cn = ctx.Services.Find(lon.ServiceID);
                        double co = cn.AgentCommission / 100;
                        double cm = Convert.ToDouble(txtAmt.Text) * co;
                        //MessageBox.Show(cm.ToString());
                        ReleasedLoan rl = new ReleasedLoan { AgentsCommission = cm, DateReleased= DateTime.Today.Date, LoanID= lId, MonthlyPayment=Convert.ToDouble(lblMonthly.Content), NetProceed=Convert.ToDouble(lblProceed.Content), Principal=Convert.ToDouble(txtAmt.Text), TotalLoan=Convert.ToDouble(lblInt.Content) };
                        lon.ReleasedLoan = rl;
                        var lo = from l in ctx.GenSOA
                                 select l;
                        int y = 0;
                        foreach (var item in lo)
                        {
                            FPaymentInfo fp = new FPaymentInfo { PaymentNumber = item.PaymentNumber, Amount = Convert.ToDouble(item.Amount), ChequeInfo = textarray[y].Text, LoanID = lId, ChequeDueDate=item.PaymentDate ,PaymentDate = item.PaymentDate, PaymentStatus = "Pending", RemainingBalance = Convert.ToDouble(item.RemainingBalance) };
                            ctx.FPaymentInfo.Add(fp);
                            y++;
                        }
                        ctx.SaveChanges();
                        MessageBox.Show("Okay");
                        this.Close();
                    }
                }
            }
            else if (status == "UReleasing")
            {
                int myCtr = 0;
                using (var ctx = new MyContext())
                {
                    var lons = from lo in ctx.FPaymentInfo
                               where lo.LoanID == lId
                               select lo;
                    foreach (var item in lons)
                    {
                        item.ChequeInfo=textarray[myCtr].Text;
                        myCtr++;
                    }
                    var lon = ctx.Loans.Find(lId);
                    var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                    int bId = bk.BankID;
                    lon.BankID = bId;
                    ctx.SaveChanges();
                    MessageBox.Show("Updated");
                    this.Close();
                }
            }
        }
    }
}
