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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfAppliedLoanInfo.xaml
    /// </summary>
    public partial class wpfAppliedLoanInfo : MetroWindow
    {

        public int lId;
        public string status;
        public wpfAppliedLoanInfo()
        {
            InitializeComponent();
        }

        public void reset()
        {
            using (var ctx = new MyContext())
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

                lblTOL.Content = lon.Service.Name;
                lblType.Content = lon.Service.Type;
                lblMode.Content = lon.Mode;
                lblInt.Content = lon.Service.Interest + "%";
                lblPen.Content = lon.Service.Holding + "%";
                lblCom.Content = lon.Service.AgentCommission + "%";
                lblTerm.Content = lon.Term;
                lblStatus.Content = lon.Status;

                if (lon.Status == "Declined")
                {
                    lblR.Visibility = Visibility.Visible;
                    lblReason1.Visibility = Visibility.Visible;
                    lblReason1.Content = lon.DeclinedLoan.Reason;
                }
                else if (lon.Status == "Approved")
                {
                    lblR.Content = "Amount Approved:";
                    lblReason1.Content = "Php " + lon.ApprovedLoan.AmountApproved.ToString("N2");
                }
                else
                {
                    lblR.Visibility = Visibility.Hidden;
                    lblReason1.Visibility = Visibility.Hidden;
                }

                double ded = lon.Service.AgentCommission;
                var dec = from de in ctx.Deductions
                          where de.ServiceID == lon.ServiceID
                          select de;
                foreach (var item in dec)
                {
                    ded = ded + item.Percentage;
                }

                lblDed.Content = ded.ToString() + "%";
                lblAmt.Content = "Php " + lon.LoanApplication.AmountApplied.ToString("N0");
                lblDt.Content = lon.LoanApplication.DateApplied.ToString();

                lblName.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName;

                if (status == "UApproval")
                {
                    btnApprove.Content = "Update Approval";
                    btnDecline.Content = "Revert Approval";
                    if (lon.Status == "Declined")
                    {
                        btnApprove.Visibility = Visibility.Hidden;
                    }
                }
                else if (status == "Releasing")
                {
                    btnApprove.Content = "Release Loan";
                    btnDecline.Visibility=Visibility.Hidden;
                    lblR.Visibility = Visibility.Visible;
                    lblReason1.Visibility = Visibility.Visible;
                    lblR.Content = "Amount Approved:";
                    lblReason1.Content = "Php " + lon.ApprovedLoan.AmountApproved.ToString("N2");
                }

                if (lblStatus.Content.ToString() == "Approved")
                {
                    lblStatus.Foreground = System.Windows.Media.Brushes.Green;
                }
                else if (lblStatus.Content.ToString() == "Declined")
                {
                    lblStatus.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    lblStatus.Foreground = System.Windows.Media.Brushes.Black;
                }
            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            wpfAgentInfo frm = new wpfAgentInfo();
            frm.status = "View";
            using (var ctx = new MyContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.aId = lon.AgentID;
            }
            frm.ShowDialog();
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Approval" || status == "UApproval")
            {
                wpfLoanApproval frm = new wpfLoanApproval();
                frm.status = status;
                frm.lId = lId;
                this.Close();
                frm.ShowDialog();
            }
            else if (status == "Releasing")
            {
                wpfFReleasing frm = new wpfFReleasing();
                frm.status = status;
                frm.lId = lId;
                this.Close();
                frm.ShowDialog();
            }

        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Approval")
            {
                wpfLoanDeclining frm = new wpfLoanDeclining();
                frm.lId = lId;
                this.Close();
                frm.ShowDialog();
            }
            else if(status == "UApproval")
            {
                System.Windows.MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo);
                if (mr == System.Windows.MessageBoxResult.Yes)
                {
                    using (var ctx = new MyContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        if (lon.Status == "Approved")
                        {
                            ctx.ApprovedLoans.Remove(lon.ApprovedLoan);
                        }
                        else
                        {
                            ctx.DeclinedLoans.Remove(lon.DeclinedLoan);
                        }
                        lon.Status = "Applied";
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Transaction has been voided");
                        this.Close();
                    }
                }
            }
        }
    }
}
