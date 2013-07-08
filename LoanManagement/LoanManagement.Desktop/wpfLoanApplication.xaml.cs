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
using System.Text.RegularExpressions;

using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanApplication.xaml
    /// </summary>
    public partial class wpfLoanApplication : MetroWindow
    {
        public int cId;
        public int cbId;
        public int agentId;
        public int servId;

        public wpfLoanApplication()
        {
            InitializeComponent();
        }

        private static byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitMap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitMap.RawFormat;
            var imageToConvert = System.Drawing.Image.FromFile(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, bmpFormat);
                return ms.ToArray();
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

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                var svc = from sv in ctx.Services
                          where sv.Active == true
                          select sv;
                foreach (var item in svc)
                {
                    ComboBoxItem itm=new ComboBoxItem();
                    itm.Content=item.Name;
                    cmbServices.Items.Add(itm);
                }
            }

            grdAgent.Visibility = Visibility.Hidden;
            grdCoBorrower.Visibility = Visibility.Hidden;

            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;
        }

        private void reset()
        {
            using (var ctx = new SystemContext())
            {
                var clt = ctx.Clients.Find(cId);
                txtLName.Text = clt.LastName;
                txtFName.Text = clt.FirstName;
                txtMName.Text = clt.MiddleName;
                txtSuffix.Text = clt.Suffix;
                txtSex.Text = clt.Sex;
                txtSSS.Text = clt.SSS;
                txtTIN.Text = clt.TIN;
                txtEmail.Text = clt.Email;
                dtBDay.SelectedDate = clt.Birthday;
                txtStatus.Text = clt.Status;

                byte[] imageArr;
                imageArr = clt.Photo;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CreateOptions = BitmapCreateOptions.None;
                bi.CacheOption = BitmapCacheOption.Default;
                bi.StreamSource = new MemoryStream(imageArr);
                bi.EndInit();
                img.Source = bi;

                


            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientInfo frm = new wpfClientInfo();
                frm.status = "View";
                frm.cId = cId;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            reset();
            try
            {
                if (txtAgent.Text == "")
                {
                    using (var ctx = new SystemContext())
                    {
                        var agt = ctx.Agents.Find(agentId);
                        String str = "(" + agentId.ToString() + ")" + agt.FirstName + " " + agt.MI + ". " + agt.LastName;
                        txtAgent.Text = str;
                    }
                }
                
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void cmbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                ComboBoxItem typeItem = (ComboBoxItem)cmbServices.SelectedItem;
                string value = typeItem.Content.ToString();
                var ser = ctx.Services.Where(x => x.Name == value).First();
                txtCat.Text = ser.Type;
                servId = ser.ServiceID;

                cmbMode.Items.Clear();

                if (ser.Department == "Financing")
                {
                    cmbMode.Items.Add(new ComboBoxItem { Content = "Monthly" });
                    cmbMode.Items.Add(new ComboBoxItem { Content = "Semi-Monthly" });
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

                lblAmt.Content = "(Min. of " + ser.MinValue.ToString("C") + " and Max. of "+ ser.MaxValue.ToString("C") +")";
                lblTerm.Content = "(Min. of " + ser.MinTerm + " month(s) and Max. of " + ser.MaxTerm + " month(s))";

                if (ser.Type == "Collateral")
                {
                    grdCoBorrower.Visibility = Visibility.Hidden;
                }
                else
                {
                    grdCoBorrower.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtAmt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtAmt.Text = Convert.ToDouble(txtAmt.Text).ToString("N0");
                txtAmt.SelectionStart = txtAmt.Text.Length;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void cboxAgent_Checked(object sender, RoutedEventArgs e)
        {
            grdAgent.Visibility = Visibility.Visible;
        }

        private void cboxAgent_Unchecked(object sender, RoutedEventArgs e)
        {
            grdAgent.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            wpfClientSearch frm = new wpfClientSearch();
            frm.status = "Agent";
            frm.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            wpfAgentInfo frm = new wpfAgentInfo();
            frm.status = "Add";
            frm.ShowDialog();
        }

        private void txtAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new SystemContext())
                {
                    var ser = ctx.Services.Find(servId);
                    double val=Convert.ToDouble(txtAmt.Text);
                    if (val > ser.MaxValue || val < ser.MinValue)
                    {
                        txtAmt.Foreground = System.Windows.Media.Brushes.Red;
                    }
                    else
                    {
                        txtAmt.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void txtTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new SystemContext())
                {
                    var ser = ctx.Services.Find(servId);
                    double val = Convert.ToDouble(txtTerm.Text);
                    if (val > ser.MaxTerm || val < ser.MinTerm)
                    {
                        txtTerm.Foreground = System.Windows.Media.Brushes.Red;
                    }
                    else
                    {
                        txtTerm.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            wpfClientSearch frm = new wpfClientSearch();
            frm.status = "Client";
            frm.status2 = "LoanApplication";
            frm.cId = cId;
            frm.ShowDialog();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (cmbServices.Text == "" || cmbMode.Text == "" || txtAmt.Text == "" || txtTerm.Text == "")
            {
                System.Windows.MessageBox.Show("Please complete the required information");
                return;
            }
            if (cboxAgent.IsChecked == true && txtAgent.Text == "")
            {
                System.Windows.MessageBox.Show("Please select the agent");
                return;
            }

            using (var ctx = new SystemContext())
            {
                var ser=ctx.Services.Find(servId);
                if (ser.Type == "Non-Collateral" && txtID.Text == "")
                {
                    System.Windows.MessageBox.Show("Please select the Co-Borrower");
                    return;
                }
                double val = Convert.ToDouble(txtAmt.Text);
                if (val > ser.MaxValue || val < ser.MinValue)
                {
                    System.Windows.MessageBox.Show("Invalid loan ammount");
                    return;
                }
                val = Convert.ToDouble(txtTerm.Text);
                if (val > ser.MaxTerm || val < ser.MinTerm)
                {
                    System.Windows.MessageBox.Show("Invalid desired term");
                }
                
            }

            using (var ctx = new SystemContext())
            {
                var ser = ctx.Services.Find(servId);
                double deduction = 0;
                var ded = from de in ctx.Deductions
                          where de.ServiceID == servId
                          select de;
                foreach(var item in ded)
                {
                    deduction = deduction + item.Percentage;
                }
                if (ser.Type == "Collateral")
                {
                    cbId = 0;
                }
                Loan loan=new Loan{};
                if (cboxAgent.IsChecked == true)
                {
                    loan = new Loan { AgentID = agentId, ClientID = cId, CoBorrower = cbId, Commission = ser.AgentCommission, Deduction = deduction, Interest = ser.Interest, Mode = cmbMode.Text, Penalty = ser.Penalty, Status = "Applied", Term = Convert.ToInt32(txtTerm.Text), Type = txtCat.Text, TypeOfLoan = cmbServices.Text };
                }
                else
                {
                    loan = new Loan { ClientID = cId, CoBorrower = cbId, Commission = ser.AgentCommission, Deduction = deduction, Interest = ser.Interest, Mode = cmbMode.Text, Penalty = ser.Penalty, Status = "Applied", Term = Convert.ToInt32(txtTerm.Text), Type = txtCat.Text, TypeOfLoan = cmbServices.Text };
                }

                LoanApplication la = new LoanApplication { AmmountApplied = Convert.ToDouble(txtAmt.Text), DateApplied = DateTime.Now.Date };
                ctx.LoanApplications.Add(la);
                ctx.Loans.Add(loan);
                ctx.SaveChanges();
                System.Windows.MessageBox.Show("Loan Successfuly Applied");

            }
        }


    }
}
