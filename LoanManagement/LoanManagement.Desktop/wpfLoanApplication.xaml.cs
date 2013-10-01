﻿using System;
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
        public string status;
        public int lId;
        public int ciId;

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
            try
            {
                ciId = 0;
                using (var ctx = new MyLoanContext())
                {
                    var svc = from sv in ctx.Services
                              where sv.Active == true
                              select sv;
                    foreach (var item in svc)
                    {
                        ComboBoxItem itm = new ComboBoxItem();
                        itm.Content = item.Name;
                        cmbServices.Items.Add(itm);
                    }
                }
                cmbServices.IsEnabled = true;
                grdAgent.Visibility = Visibility.Hidden;
                grdCoBorrower.Visibility = Visibility.Hidden;
                cboxAgent.IsEnabled = true;

                if (status == "Edit")
                {
                    cboxAgent.IsEnabled = !true;
                    using (var ctx = new MyLoanContext())
                    {

                        var lon = ctx.Loans.Find(lId);
                        cId = lon.ClientID;
                        cmbServices.IsEnabled = !true;
                        cmbServices.Text = lon.Service.Name;
                        txtCat.Text = lon.Service.Type;
                        cmbMode.Text = lon.Mode;
                        var la = ctx.LoanApplications.Where(x => x.LoanID == lId).First();
                        txtAmt.Text = la.AmountApplied.ToString();
                        txtTerm.Text = lon.Term.ToString();
                        if (lon.AgentID != 0)
                        {
                            agentId = lon.AgentID;
                            var agt = ctx.Agents.Find(agentId);
                            String str = "(" + agentId.ToString() + ")" + agt.FirstName + " " + agt.MI + ". " + agt.LastName;
                            txtAgent.Text = str;
                            grdAgent.Visibility = Visibility.Visible;
                            cboxAgent.IsChecked = true;
                        }

                        if (lon.CI != 0)
                        {
                            ciId = lon.CI;
                            var agt = ctx.Employees.Find(ciId);
                            String str = "(" + agentId.ToString() + ")" + agt.FirstName + " " + agt.MI + ". " + agt.LastName;
                            txtCI.Text = str;
                        }

                        if (lon.Service.Type == "Non Collateral")
                        {
                            cbId = lon.CoBorrower;
                            var agt = ctx.Clients.Find(cbId);
                            String str = "(" + cbId + ")" + agt.FirstName + " " + agt.MiddleName + " " + agt.LastName;
                            txtID.Text = str;
                            grdCoBorrower.Visibility = Visibility.Visible;
                        }


                    }
                }

                reset();



                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void reset()
        {
            try
            {
                using (var ctx = new MyLoanContext())
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
                wpfClientInfo frm = new wpfClientInfo();
                frm.status = "View";
                frm.cId = cId;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            
            try
            {
                if (txtAgent.Text == "")
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var agt = ctx.Agents.Find(agentId);
                        String str = "(" + agentId.ToString() + ")" + agt.FirstName + " " + agt.MI + ". " + agt.LastName;
                        txtAgent.Text = str;
                    }
                }
                
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void cmbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (var ctx = new MyLoanContext())
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

                    lblAmt.Content = "(Min. of " + ser.MinValue.ToString("C") + " and Max. of " + ser.MaxValue.ToString("C") + ")";
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void cboxAgent_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                grdAgent.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void cboxAgent_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                grdAgent.Visibility = Visibility.Hidden;
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
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "Agent";
                frm.ShowDialog();
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
                wpfAgentInfo frm = new wpfAgentInfo();
                frm.status = "Add";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new MyLoanContext())
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
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new MyLoanContext())
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
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "Client";
                frm.status2 = "LoanApplication";
                frm.cId = cId;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            try
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

                using (var ctx = new MyLoanContext())
                {
                    var ser = ctx.Services.Find(servId);
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

                using (var ctx = new MyLoanContext())
                {
                    var ser = ctx.Services.Find(servId);
                    double deduction = 0;
                    var ded = from de in ctx.Deductions
                              where de.ServiceID == servId
                              select de;
                    foreach (var item in ded)
                    {
                        deduction = deduction + item.Percentage;
                    }
                    deduction = deduction + ser.AgentCommission;
                    if (ser.Type == "Collateral")
                    {
                        cbId = 0;
                    }

                    if (status == "Add")
                    {

                        Loan loan = new Loan { };

                        LoanApplication la = new LoanApplication { AmountApplied = Convert.ToDouble(txtAmt.Text), DateApplied = DateTime.Now.Date };
                        if (cboxAgent.IsChecked == true)
                        {
                            loan = new Loan { CI = ciId, AgentID = agentId, ClientID = cId, CoBorrower = cbId, ServiceID = servId, Status = "Applied", Term = Convert.ToInt32(txtTerm.Text), LoanApplication = la, Mode = cmbMode.Text };
                        }
                        else
                        {
                            loan = new Loan { CI = ciId, AgentID = 0, ClientID = cId, CoBorrower = cbId, ServiceID = servId, Status = "Applied", Term = Convert.ToInt32(txtTerm.Text), LoanApplication = la, Mode = cmbMode.Text };
                        }


                        ctx.Loans.Add(loan);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Loan Successfuly Applied");
                        this.Close();
                    }
                    else
                    {

                        System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to update?", "Question", MessageBoxButtons.YesNo);

                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            var ln = ctx.Loans.Find(lId);
                            if (cboxAgent.IsChecked == true)
                            {
                                ln.AgentID = agentId;
                                ln.CoBorrower = cbId;
                                ln.Mode = cmbMode.Text;
                                ln.CI = ciId;
                                ln.LoanApplication.AmountApplied = Convert.ToDouble(txtAmt.Text);
                                ln.Term = Convert.ToInt32(txtTerm.Text);
                            }
                            else
                            {
                                ln.CoBorrower = cbId;
                                ln.Mode = cmbMode.Text;
                                ln.CI = ciId;
                                ln.LoanApplication.AmountApplied = Convert.ToDouble(txtAmt.Text);
                                ln.Term = Convert.ToInt32(txtTerm.Text);
                            }
                            ctx.SaveChanges();
                            System.Windows.MessageBox.Show("Record updated");
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

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClientSearch frm = new wpfClientSearch();
                frm.status = "CI";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_LocationChanged(object sender, EventArgs e)
        {

        }


    }
}
