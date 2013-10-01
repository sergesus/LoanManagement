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

        public int lId;
        public int myNum;
        public TextBox[] textarray = new TextBox[0];

        public void reset()
        {
            try
            {
                using (var ctx = new MyLoanContext())
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
                        sp[ctr] = new StackPanel();
                        sp[ctr].Width = 300;
                        sp[ctr].Height = 60;
                        sp[ctr].Children.Add(labelarray[ctr]);
                        sp[ctr].Children.Add(textarray[ctr]);
                        stck.Children.Add(sp[ctr]);
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
                MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new MyLoanContext())
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
