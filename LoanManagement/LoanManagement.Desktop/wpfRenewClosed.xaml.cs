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
    /// Interaction logic for wpfRenewClosed.xaml
    /// </summary>
    public partial class wpfRenewClosed : MetroWindow
    {
        public wpfRenewClosed()
        {
            InitializeComponent();
        }
        public int UserID;
        public int lId;
        public int myNum;
        public TextBox[] textarray = new TextBox[0];

        public void reset()
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    myNum = ctx.FPaymentInfo.Where(x => x.LoanID == lId && x.PaymentStatus == "Void").Count();

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
                        //labelarray[ctr].Content = "Cheque No. " + (ctr + 1).ToString();
                        textarray[ctr] = new TextBox();
                        textarray[ctr].Height = 25;
                        textarray[ctr].Width = 300;
                        textarray[ctr].FontSize = 16;
                        textarray[ctr].MaxLength = 6;
                        sp[ctr] = new StackPanel();
                        sp[ctr].Width = 300;
                        sp[ctr].Height = 60;
                        sp[ctr].Children.Add(labelarray[ctr]);
                        sp[ctr].Children.Add(textarray[ctr]);
                        stck.Children.Add(sp[ctr]);
                        if (ctr == 0)
                        {
                            textarray[0].LostFocus += new RoutedEventHandler(txt_LostFocus);
                        }
                    }
                    var ch = from c in ctx.FPaymentInfo
                             where c.LoanID == lId && c.PaymentStatus == "Void"
                             select c;
                    int num = 0;
                    foreach (var item in ch)
                    {
                        labelarray[num].Content = "Cheque No. " + item.PaymentNumber;
                        num++;
                    }

                    cmbBank.Items.Clear();
                    var ban = from ba in ctx.Banks
                              where ba.Active == true
                              select ba;
                    foreach (var item in ban)
                    {
                        cmbBank.Items.Add(item.BankName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int srs = Convert.ToInt32(textarray[0].Text);
                for (int x = 1; x < myNum; x++)
                {
                    srs++;
                    textarray[x].Text = srs.ToString();
                }
            }
            catch (Exception)
            {
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
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var i in textarray)
                {
                    if (i.Text.Length != 6)
                    {
                        System.Windows.MessageBox.Show("Please input all cheque numbers", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    bool err;
                    int res;
                    String str = i.Text;
                    err = int.TryParse(str, out res);
                    if (err == false)
                    {
                        System.Windows.MessageBox.Show("Please input the correct format for cheque numbers(Strictly numbers only.)", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                for (int x = 0; x < textarray.Length; x++)
                {
                    for (int y = x + 1; y < textarray.Length; y++)
                    {
                        if (textarray[x].Text == textarray[y].Text)
                        {
                            System.Windows.MessageBox.Show("No duplications of cheque numbers", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }



                MessageBoxResult mr = MessageBox.Show("Are you sure you want to process this transaction?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new newerContext())
                    {
                        var bk = ctx.Banks.Where(x => x.BankName == cmbBank.Text).First();
                        int bId = bk.BankID;
                        var lon = ctx.Loans.Find(lId);
                        lon.Status = "Released";//active
                        lon.BankID = bId;
                        var ch = from c in ctx.FPaymentInfo
                                 where c.LoanID == lId && c.PaymentStatus == "Void"
                                 select c;
                        int Interval = 0;
                        DateInterval dInt = new DateInterval();
                        if (lon.Mode == "Monthly")
                        {
                            Interval = 1;
                            dInt = DateInterval.Month;
                        }
                        else if (lon.Mode == "Semi-Monthly")
                        {
                            Interval = 15;
                            dInt = DateInterval.Day;
                        }
                        else if (lon.Mode == "Weekly")
                        {
                            Interval = 7;
                            dInt = DateInterval.Day;
                        }
                        else if (lon.Mode == "Daily")
                        {
                            Interval = 1;
                            dInt = DateInterval.Day;
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
                        DateTime dt = dtPcker.SelectedDate.Value.Date;
                        int num = 0;
                        foreach (var item in ch)
                        {
                            item.ChequeInfo = textarray[num].Text;
                            item.ChequeDueDate = dt;
                            item.PaymentDate = dt;
                            item.PaymentStatus = "Pending";
                            dt = DateAndTime.DateAdd(dInt, Interval, dt);
                            num++;
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Processed renewal for Closed Account Loan " + lon.LoanID };
                        ctx.AuditTrails.Add(at);

                        ctx.SaveChanges();
                        MessageBox.Show("Transaction has been successfully processed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
