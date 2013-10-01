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

using System.Data.Entity;
using LoanManagement.Domain;
using MahApps.Metro.Controls;
using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanRestructure.xaml
    /// </summary>
    public partial class wpfLoanRestructure : MetroWindow
    {

        public int lId;
        public int myNum;
        public TextBox[] textarray;
        public wpfLoanRestructure()
        {
            InitializeComponent();
        }

        public void reset()
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    var rmn = from rm in ctx.FPaymentInfo
                              where rm.LoanID == lId && rm.PaymentStatus == "Cleared"
                              select rm;
                    double r = 0;
                    foreach (var item in rmn)
                    {
                        r = r + item.Amount;
                    }
                    double remain = lon.ReleasedLoan.TotalLoan - r;
                    txtTerm.Text = lon.Term.ToString();
                    txtAmt.Text = remain.ToString("N2");
                    txtInt.Text = lon.Service.RestructureInterest.ToString();
                    cmbMode.Text = lon.Mode;
                    refresh();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void refresh()
        {
            try
            {
                using (var ctx = new MyLoanContext())
                {
                    myNum = 0;
                    ctx.Database.ExecuteSqlCommand("delete from dbo.GenSOAs");
                    ctx.SaveChanges();
                    var lon = ctx.Loans.Find(lId);
                    Double Amt = Convert.ToDouble(txtAmt.Text);
                    //lblPrincipal.Content = Amt.ToString("N2");
                    //txtAmt.Text = Amt.ToString("N2");
                    //txtAmt.SelectionStart = txtAmt.Text.Length - 3;
                    Double TotalInt = Convert.ToDouble(txtInt.Text) * Convert.ToInt32(txtTerm.Text);
                    TotalInt = TotalInt / 100;
                    /*double ded = lon.Service.AgentCommission;
                    var dec = from de in ctx.Deductions
                              where de.ServiceID == lon.ServiceID
                              select de;
                    foreach (var item in dec)
                    {
                        ded = ded + item.Percentage;
                    }
                    Double Deduction = ded / 100;
                    Double NetProceed = (Convert.ToDouble(txtAmt.Text)) - (Convert.ToDouble(txtAmt.Text) * Deduction);
                
                     */
                    Double WithInt = (Convert.ToDouble(txtAmt.Text)) + (Convert.ToDouble(txtAmt.Text) * TotalInt);
                    //lblProceed.Content = NetProceed.ToString("N2");
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
                    /*else if (cmbMode.Text == "One-Time Payment")
                    {
                        NetProceed = NetProceed - ((Convert.ToDouble(txtAmt.Text) * TotalInt));
                        lblProceed.Content = NetProceed.ToString("N2");
                        lblInt.Content = txtAmt.Text;
                        Remaining = Convert.ToDouble(lblPrincipal.Content);
                        Payment = Remaining;
                        Interval = Convert.ToInt32(txtTerm.Text);
                        dInt = DateInterval.Month;
                        lbl4.Content = "Total Payment";
                    }*/

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
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
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
                using (var ctx = new MyLoanContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    if (lon.Service.Department == "Financing")
                    {
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "One-Time Payment" });
                    }
                    else if (lon.Service.Department == "Micro Business")
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

                    cmbBank.Items.Clear();
                    var ban = from ba in ctx.Banks
                              where ba.Active == true
                              select ba;
                    foreach (var item in ban)
                    {
                        cmbBank.Items.Add(item.BankName);
                    }
                    reset();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void lblInt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRestructure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult mr = MessageBox.Show("Sure?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                        int bId = bk.BankID;
                        var lon = ctx.Loans.Find(lId);
                        lon.Status = "Resturctured";
                        Loan l = new Loan { AgentID = lon.AgentID, CI = 0, Term = Convert.ToInt32(txtTerm.Text), Status = "Released", ServiceID = lon.ServiceID, Mode = cmbMode.Text, CoBorrower = lon.CoBorrower, ClientID = lon.ClientID, BankID = bId };
                        ReleasedLoan rl = new ReleasedLoan { LoanID = l.LoanID, AgentsCommission = 0, DateReleased = DateTime.Today.Date, MonthlyPayment = Convert.ToDouble(lblMonthly.Content), NetProceed = 0, Principal = 0, TotalLoan = Convert.ToDouble(lblInt.Content) };
                        RestructuredLoan rln = new RestructuredLoan { LoanID = lId, NewLoanID = l.LoanID, DateRestructured = DateTime.Today, Fee = Convert.ToDouble(txtAmt.Text) * (lon.Service.RestructureFee / 100) };
                        var fp = from f in ctx.FPaymentInfo
                                 where f.PaymentStatus != "Cleared" && f.LoanID == lId
                                 select f;
                        foreach (var item in fp)
                        {
                            item.PaymentStatus = "Void";
                        }

                        var lo = from ly in ctx.GenSOA
                                 select ly;
                        int y = 0;
                        foreach (var item in lo)
                        {
                            FPaymentInfo fpy = new FPaymentInfo { PaymentNumber = item.PaymentNumber, Amount = Convert.ToDouble(item.Amount), ChequeInfo = textarray[y].Text, LoanID = l.LoanID, ChequeDueDate = item.PaymentDate, PaymentDate = item.PaymentDate, PaymentStatus = "Pending", RemainingBalance = Convert.ToDouble(item.RemainingBalance) };
                            ctx.FPaymentInfo.Add(fpy);
                            y++;
                        }

                        ctx.Loans.Add(l);
                        ctx.ReleasedLoans.Add(rl);
                        ctx.RestructuredLoans.Add(rln);

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
