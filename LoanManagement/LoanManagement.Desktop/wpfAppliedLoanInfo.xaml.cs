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
using System.Diagnostics;
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
        public int UserID;
        public wpfAppliedLoanInfo()
        {
            InitializeComponent();
        }

        public void reset()
        {
            try
            {
                using (var ctx = new newerContext())
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
                        lbl1.Content = "Update Approval";
                        lbl2.Content = "Revert Approval";
                        if (lon.Status == "Declined")
                        {
                            btnApprove.Visibility = Visibility.Hidden;
                        }
                    }
                    else if (status == "Releasing")
                    {
                        lbl1.Content = "Release Loan";
                        btnDecline.Visibility = Visibility.Hidden;
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
                reset();
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;

                if (status == "View")
                {
                    btnApprove.Visibility = Visibility.Hidden;
                    btnDecline.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgentInfo frm = new wpfAgentInfo();
                frm.status = "View";
                using (var ctx = new newerContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    frm.aId = lon.AgentID;
                }
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Approval" || status == "UApproval")
                {
                    using (var ctx = new newerContext())
                    {
                        var lon = ctx.Loans.Find(lId);
                        var ctr = ctx.Loans.Where(x => x.ClientID == lon.ClientID && (x.Status == "Applied" || x.Status == "Released" || x.Status == "Approved") && x.LoanID != lon.LoanID).Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Client cannot have more than one(1) Loan application/Loan", "Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }

                        ctr = ctx.Loans.Where(x => x.ClientID == lon.ClientID && (x.Status == "Closed Account" || x.Status == "Under Collection")).Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Client has bad record due to Closed Account/Unpaid Balance. Unable to process loan.", "Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }

                        ctr = ctx.Loans.Where(x => x.ClientID == lon.ClientID && x.Status == "Released" && x.Service.Department != lon.Service.Department).Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Client has an existing loan on other department", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    wpfLoanApproval frm = new wpfLoanApproval();
                    frm.status = status;
                    frm.UserID = UserID;
                    frm.lId = lId;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "Releasing")
                {
                    String iDept = "";
                    using (var ctx = new newerContext())
                    {
                        var ln = ctx.Loans.Find(lId);
                        iDept = ln.Service.Department;
                    }
                    if (iDept == "Financing")
                    {
                        wpfFReleasing frm = new wpfFReleasing();
                        frm.status = status;
                        frm.UserID = UserID;
                        frm.lId = lId;
                        this.Close();
                        frm.ShowDialog();
                    }
                    else
                    {
                        wpfMReleasing frm = new wpfMReleasing();
                        frm.status = status;
                        frm.UserID = UserID;
                        frm.lId = lId;
                        this.Close();
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Approval")
                {
                    wpfLoanDeclining frm = new wpfLoanDeclining();
                    frm.lId = lId;
                    frm.UserID = UserID;
                    this.Close();
                    frm.ShowDialog();
                }
                else if (status == "UApproval")
                {
                    System.Windows.MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to update this loan?", "Question", MessageBoxButton.YesNo);
                    if (mr == System.Windows.MessageBoxResult.Yes)
                    {
                        using (var ctx = new newerContext())
                        {
                            var lon = ctx.Loans.Find(lId);
                            var ctr = ctx.Loans.Where(x => x.ClientID == lon.ClientID && (x.Status == "Applied" || x.Status == "Released" || x.Status == "Approved") && x.LoanID != lon.LoanID).Count();
                            if (lon.Status == "Declined")
                            {
                                if (ctr > 0)
                                {
                                    System.Windows.MessageBox.Show("Client cannot have more than one(1) Loan application/Loan", "Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    return;
                                }

                                ctr = ctx.Loans.Where(x => x.ClientID == lon.ClientID && (x.Status == "Closed Account" || x.Status == "Under Collection")).Count();
                                if (ctr > 0)
                                {
                                    System.Windows.MessageBox.Show("Client has bad record due to Closed Account/Unpaid Balance. Unable to process loan.", "Notice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    return;
                                }

                                ctr = ctx.Loans.Where(x => x.ClientID == lon.ClientID && x.Status == "Released" && x.Service.Department != lon.Service.Department).Count();
                                if (ctr > 0)
                                {
                                    System.Windows.MessageBox.Show("Client has an existing loan on other department", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }

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
                            System.Windows.MessageBox.Show("Transaction has been voided","Information",MessageBoxButton.OK,MessageBoxImage.Information);
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {

        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string folderName = @"F:\Loan Files";
                string pathString = System.IO.Path.Combine(folderName, "Loan " + lId.ToString());
                if (!Directory.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(pathString);
                    Process.Start(@"F:\Loan Files\Loan " + lId.ToString());
                }
                else
                {
                    Process.Start(@"F:\Loan Files\Loan " + lId.ToString());
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
