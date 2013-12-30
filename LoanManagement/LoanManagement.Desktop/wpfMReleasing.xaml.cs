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
    /// Interaction logic for wpfMReleasing.xaml
    /// </summary>
    public partial class wpfMReleasing : MetroWindow
    {
        public string status;
        public int lId;
        public int UserID;
        public int ciId = 0;

        public wpfMReleasing()
        {
            InitializeComponent();
        }

        private void refr()
        {
            //try
            //{
                

                if (status == "Releasing")
                {
                    using (var ctx = new iContext())
                    {
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

                        ComboBoxItem typeItem = (ComboBoxItem)cmbMode.SelectedItem;
                        string value = typeItem.Content.ToString();
                        if (value == "Monthly")
                        {
                            Interval = 1;
                            dInt = DateInterval.Month;
                            Payment = WithInt / Convert.ToInt32(txtTerm.Text);
                            lbl4.Content = "Monthly Payment";
                        }
                        else if (value == "Semi-Monthly")
                        {
                            Interval = 15;
                            dInt = DateInterval.Day;
                            Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 2);
                            lbl4.Content = "Semi-Monthly Payment";
                        }
                        else if (value == "Weekly")
                        {
                            Interval = 7;
                            dInt = DateInterval.Day;
                            Payment = WithInt / (Convert.ToInt32(txtTerm.Text) * 4);
                        }
                        else if (value == "Daily")
                        {
                            Interval = 1;
                            dInt = DateInterval.Day;
                            Payment = WithInt / ((Convert.ToInt32(txtTerm.Text) * 4) * 7);
                        }
                        else if (value == "One-Time Payment")
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
                        while (Remaining > -1)
                        {
                            Remaining = Remaining - Payment;
                            GenSOA soa = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = Remaining.ToString("N2") };
                            ctx.GenSOA.Add(soa);
                            num++;
                            if (Remaining <= Payment)
                            {
                                Payment = Remaining;
                                if (Payment < 1)
                                {
                                    goto a;
                                }
                                GenSOA soa1 = new GenSOA { Amount = Payment.ToString("N2"), PaymentDate = dt, PaymentNumber = num, RemainingBalance = "0.00" };
                                ctx.GenSOA.Add(soa1);
                                num++;
                                goto a;
                            }
                            dt = DateAndTime.DateAdd(dInt, Interval, dt);
                            bool isHoliday = true;
                            while (isHoliday == true || dt.Date.DayOfWeek.ToString() == "Saturday" || dt.Date.DayOfWeek.ToString() == "Sunday")
                            {
                                if (dt.Date.DayOfWeek.ToString() == "Saturday") 
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 2, dt);
                                }
                                else if (dt.Date.DayOfWeek.ToString() == "Sunday")
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                }
                                var myC = ctx.Holidays.Where(x => x.Date.Month == dt.Date.Month && x.Date.Day == dt.Date.Day && x.isYearly == true).Count();
                                if (myC > 0)
                                {
                                    dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                    isHoliday = true;
                                }
                                else
                                {
                                    myC = ctx.Holidays.Where(x => x.Date.Month == dt.Date.Month && x.Date.Day == dt.Date.Day && x.Date.Year == dt.Date.Year && x.isYearly == !true).Count();
                                    if (myC > 0)
                                    {
                                        dt = DateAndTime.DateAdd(DateInterval.Day, 1, dt);
                                        isHoliday = true;
                                    }
                                    else
                                    {
                                        isHoliday = false;
                                    }
                                }
                            }
                        }
                    a:
                        lblMonthly.Content = Payment.ToString("N2");
                        ctx.SaveChanges();
                        var gen = from ge in ctx.GenSOA
                                  select new { PaymentNumber = ge.PaymentNumber, TotalPayment = ge.Amount, PaymentDate = ge.PaymentDate, RemainingBalance = ge.RemainingBalance };
                        dgSOA.ItemsSource = gen.ToList();
                        return;
                    }
                }
                else if (status == "UReleasing")
                {
                    using (var ctx = new iContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var pys = from p in ctx.MPaymentInfoes
                                  where p.LoanID == lId
                                  select new { No = p.PaymentNumber, Amount = p.Amount, PrevBalance = p.PreviousBalance, PrevBalanceInterest = p.BalanceInterest, TotalBalance = p.TotalBalance, TotalAmount = p.TotalAmount, DueDate = p.DueDate, RemaingBalance = p.RemainingLoanBalance, Status = p.PaymentStatus };
                        dgSOA.ItemsSource = pys.ToList();
                        txtAmt.Text = lon.ReleasedLoan.TotalLoan.ToString("N2");
                        lblMonthly.Content = lon.ReleasedLoan.MonthlyPayment.ToString("N2");
                        lblPrincipal.Content = txtAmt.Text;
                        lblProceed.Content = lon.ReleasedLoan.NetProceed.ToString("N2");
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.Items.Add(lon.Mode);
                        cmbMode.SelectedItem = lon.Mode;
                        var agt = ctx.Employees.Find(lon.CollectortID);
                        ciId = lon.CollectortID;
                        String str = "(" + ciId + ")" + agt.FirstName + " " + agt.MI + " " + agt.LastName;
                        txtCI.Text = str;
                       
                    }
                }
                if (Convert.ToInt16(txtTerm.Text) < 1)
                {
                    System.Windows.MessageBox.Show("Incorrect Format for term", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            /*}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Incorrect Format on some Fields / Incomplete Input(s)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refr();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
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
                    using (var ctx = new iContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var ser = ctx.Services.Find(lon.ServiceID);
                        cmbMode.Items.Clear();
                        Double Amt = Convert.ToDouble(lon.ApprovedLoan.AmountApproved);
                        lblPrincipal.Content = Amt.ToString("N2");
                        txtAmt.Text = Amt.ToString("N2");
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Weekly" });
                        cmbMode.Items.Add(new ComboBoxItem { Content = "Daily" });
                        txtTerm.Text = lon.Term.ToString();
                        cmbMode.Text = lon.Mode;
                        refr();
                    }
                }
                else if (status == "UReleasing")
                {

                    txtAmt.IsEnabled = false;
                    txtTerm.IsEnabled = false;
                    cmbMode.IsEnabled = false;
                    btnRef.IsEnabled = false;
                    btnRelease.Content = "Update";
                    refr();
                }
            /*}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/
        }

        private void cmbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                refr();
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
                refr();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Releasing")
                {
                    double max = 0;
                    double min = 0;
                    using (var ctx = new iContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var ser = ctx.Services.Find(lon.ServiceID);
                        max = ser.MaxTerm;
                        min = ser.MinTerm;


                        if (Convert.ToDouble(txtTerm.Text) > max || Convert.ToDouble(txtTerm.Text) < min)
                        {
                            System.Windows.MessageBox.Show("Term must not be greater than the maximum term(" + ser.MaxTerm + " mo.) OR less than the minimum term(" + ser.MinTerm + " mo.)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        max = ser.MaxValue;
                        min = ser.MinValue;

                        if (Convert.ToDouble(txtAmt.Text) > max || Convert.ToDouble(txtAmt.Text) < min)
                        {
                            System.Windows.MessageBox.Show("Principal amount must not be greater than the maximum loanable amount OR less than the minimum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    if (Convert.ToDouble(txtAmt.Text) > Convert.ToDouble(lblPrincipal.Content))
                    {
                        MessageBox.Show("Principal amount must not be greater than the maximum loanable amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if(ciId == 0)
                    {
                        MessageBox.Show("Please assign a collector for this loan", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mr == MessageBoxResult.Yes)
                    {
                        using (var ctx = new iContext())
                        {
                            int bId = 0;
                            var lon = ctx.Loans.Find(lId);
                            lon.Status = "Released";
                            lon.Mode = cmbMode.Text;
                            lon.Term = Convert.ToInt32(txtTerm.Text);
                            lon.CollectortID = ciId;
                            var cn = ctx.Services.Find(lon.ServiceID);
                            double co = cn.AgentCommission / 100;
                            double cm = Convert.ToDouble(txtAmt.Text) * co;
                            //MessageBox.Show(cm.ToString());
                            ReleasedLoan rl = new ReleasedLoan { AgentsCommission = cm, DateReleased = DateTime.Today.Date, LoanID = lId, MonthlyPayment = Convert.ToDouble(lblMonthly.Content), NetProceed = Convert.ToDouble(lblProceed.Content), Principal = Convert.ToDouble(txtAmt.Text), TotalLoan = Convert.ToDouble(lblInt.Content) };
                            lon.ReleasedLoan = rl;
                            var lo = ctx.GenSOA.Where(x => x.PaymentNumber == 1).First();
                            int y = 0;
                            MPaymentInfo py = new MPaymentInfo { Amount = Convert.ToDouble(lo.Amount), BalanceInterest = 0, DueDate = lo.PaymentDate, LoanID = lId, PaymentNumber = 1, PaymentStatus = "Pending", PreviousBalance = 0, RemainingLoanBalance = Convert.ToDouble(lblInt.Content), TotalAmount = Convert.ToDouble(lo.Amount), TotalBalance = 0 };
                            ctx.MPaymentInfoes.Add(py);
                            AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Released loan (" + lon.Service.Name + ") for client " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.LastName + " " + lon.Client.Suffix };
                            ctx.AuditTrails.Add(at);
                            ctx.SaveChanges();
                            //printSOA();
                            MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                    }
                }
                else if (status == "UReleasing")
                {
                    
                    using (var ctx = new iContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        lon.CollectortID = ciId;

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Released loan (" + lon.Service.Name + ") for client " + lon.Client.FirstName + " " + lon.Client.MiddleName + " " + lon.Client.LastName + " " + lon.Client.Suffix };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        MessageBox.Show("Transaction has been successfully updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Incorrect Format on some Fields / Incomplete Input(s)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "Collector";
                //frm.cId = ciId;
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
