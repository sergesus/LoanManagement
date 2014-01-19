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

using System.Data.Entity;
using LoanManagement.Domain;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfServiceInfo.xaml
    /// </summary>
    public partial class wpfServiceInfo : MetroWindow
    {
        public string status;
        public int sId;
        public int UserID;

        public wpfServiceInfo()
        {
            InitializeComponent();
        }

        public void checkNumeric(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                int res;
                String str = txt.Text;
                err = !int.TryParse(str, out res);
                int n = 0;
                n = int.Parse(str);

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {

                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter numeric values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    n = int.Parse(str);
                    if (n <= 0)
                    {
                        lbl.Content = "?";
                        lbl.ToolTip = "Value must be greater than 1";
                        lbl.Focus();
                        lbl.Foreground = System.Windows.Media.Brushes.Red;
                        lbl.FontWeight = FontWeights.ExtraBold;
                        return;
                    }
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkDouble(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                double res;
                String str = txt.Text;
                err = !double.TryParse(str, out res);
                double n = 0;
                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter numeric values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    n = double.Parse(str);
                    if (n <= 0)
                    {
                        lbl.Content = "?";
                        lbl.ToolTip = "Value must be greater than 1";
                        lbl.Focus();
                        lbl.Foreground = System.Windows.Media.Brushes.Red;
                        lbl.FontWeight = FontWeights.ExtraBold;
                        return;
                    }
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkEmail(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                String str = txt.Text;
                str = str.Replace(" ", "");
                str = str.Trim();
                str = str.ToLower();
                txt.Text = str;
                if (!Regex.IsMatch(str, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                    err = true;

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter the correct email format(yourname@site.com)";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkName(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                String str = txt.Text;
                str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ");
                str = str.Trim();
                str = str.ToLower();
                str = Regex.Replace(str, "(?:^|\\s)\\w", new MatchEvaluator(delegate(Match m) { return m.Value.ToUpper(); }));
                txt.Text = str;
                if (!Regex.IsMatch(str, @"^[a-zA-Z ]*$") || str.Length > 25)
                    err = true;

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {

                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter alphabetic values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.ToolTip = "Correct Format";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                    lbl.FontWeight = FontWeights.Normal;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkString(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                String str = txt.Text;
                str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ");
                str = str.Trim();
                //str = str.ToLower();
                str = Regex.Replace(str, "(?:^|\\s)\\w", new MatchEvaluator(delegate(Match m) { return m.Value.ToUpper(); }));
                txt.Text = str;
                if (!Regex.IsMatch(str, @"^[a-zA-Z0-9 @.]*$"))
                    err = true;

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter alphanumeric & symbol values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

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
                reset();
                return "";
            }
        }

        public void reset()
        {
            try
            {
                lblReqDesc.Content = "";
                lblReqName.Content = "";
                lblDedName.Content = "";
                lblDedPerc.Content = "";
                lblPenAdj.Content = "";
                lblPenCA.Content = "";
                lblPenDaif.Content = "";
                lblPenHolding.Content = "";
                lblPenIn.Content = "";
                lblPenLate.Content = "";
                lblPenRes.Content = "";

                lblCIName.Content = "";

                tbDed.IsEnabled = !false;
                tbInfo.IsEnabled = !false;
                tbPen.IsEnabled = !false;
                tbReq.IsEnabled = !false;
                tbCol.IsEnabled = !false;

                btnAddReq.Content = "Add";
                btnEdtReq.Content = "Edit";
                dgReq.IsEnabled = true;
                btnDelReq.Visibility = Visibility.Visible;
                grpReq.Visibility = Visibility.Hidden;
                txtReqName.Text = "";
                txtReqDesc.Text = "";

                btnAddDed.Content = "Add";
                btnEdtDed.Content = "Edit";
                dgDed.IsEnabled = true;
                btnDelDed.Visibility = Visibility.Visible;
                grpDed.Visibility = Visibility.Hidden;
                txtDedName.Text = "";
                txtDedPerc.Text = "";

                btnAddCI.Content = "Add";
                btnEdtCI.Content = "Edit";
                dgCI.IsEnabled = true;
                btnDelCI.Visibility = Visibility.Visible;
                grpCI.Visibility = Visibility.Hidden;
                txtCIName.Text = "";
                int num1 = 0;
                int num2 = 0;
                int num3 = 0;
                if (status == "Add")
                {
                    using (var ctx = new newerContext())
                    {
                        num1 = ctx.TempoRequirements.Count();
                        num2 = ctx.TempoDeductions.Count();
                        num3 = ctx.TempCollateralInformations.Count();
                    }
                }
                else
                {
                    using (var ctx = new newerContext())
                    {
                        num1 = ctx.Requirements.Where(x => x.ServiceID == sId).Count();
                        num2 = ctx.Deductions.Where(x => x.ServiceID == sId).Count();
                        num3 = ctx.CollateralInformations.Where(x => x.ServiceID == sId).Count();
                    }
                }
                if (num1 > 0)
                {
                    btnDelReq.IsEnabled = true;
                    btnEdtReq.IsEnabled = true;
                }
                else
                {
                    btnDelReq.IsEnabled = !true;
                    btnEdtReq.IsEnabled = !true;
                }
                if (num2 > 0)
                {
                    btnDelDed.IsEnabled = true;
                    btnEdtDed.IsEnabled = true;
                }
                else
                {
                    btnDelDed.IsEnabled = !true;
                    btnEdtDed.IsEnabled = !true;
                }
                if (num3 > 0)
                {
                    btnDelCI.IsEnabled = true;
                    btnEdtCI.IsEnabled = true;
                }
                else
                {
                    btnDelCI.IsEnabled = !true;
                    btnEdtCI.IsEnabled = !true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblName.Content == "?" || lblMinTerm.Content == "?" || lblMaxTerm.Content == "?" || lblMinVal.Content == "?" || lblMaxVal.Content == "?" || lblInterest.Content == "?" || lblDesc.Content == "?" || lblCom.Content == "?" || lblPenAdj.Content == "?" || lblPenCA.Content == "?" || lblPenDaif.Content == "?" || lblPenHolding.Content == "?" || lblPenIn.Content == "?" || lblPenLate.Content == "?"
                    || String.IsNullOrWhiteSpace(txtCom.Text) || String.IsNullOrWhiteSpace(txtHolding.Text) || String.IsNullOrWhiteSpace(txtDaif.Text) || String.IsNullOrWhiteSpace(txtClosed.Text) || String.IsNullOrWhiteSpace(txtResFee.Text) || String.IsNullOrWhiteSpace(txtResInt.Text) || String.IsNullOrWhiteSpace(txtLtPen.Text)
                    || String.IsNullOrWhiteSpace(txtName.Text) || String.IsNullOrWhiteSpace(txtMinTerm.Text) || String.IsNullOrWhiteSpace(txtMaxTerm.Text) || String.IsNullOrWhiteSpace(txtMinVal.Text) || String.IsNullOrWhiteSpace(txtMaxVal.Text) || String.IsNullOrWhiteSpace(txtInterest.Text) || String.IsNullOrWhiteSpace(txtDesc.Text))
                {
                    System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (txtName.Text == "" || txtDesc.Text == "" || txtInterest.Text == "" || txtMaxTerm.Text == "" || txtMinVal.Text == "" || txtMinTerm.Text == "" || cmbDept.Text == "" || cmbType.Text == "")
                {
                    return;
                }

                if (Convert.ToDouble(txtMaxVal.Text) < 1 || Convert.ToDouble(txtMinVal.Text) < 1 || Convert.ToDouble(txtMaxTerm.Text) < 1 || Convert.ToDouble(txtMinTerm.Text) < 0)
                {
                    System.Windows.MessageBox.Show("Terms and Values must be greater thant one(1)", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                if (status == "Add")
                {


                    using (var ctx = new newerContext())
                    {
                        Service ser = null;
                        if (cmbDept.Text == "Financing")
                        {
                            ser = new Service { Name = txtName.Text, Department = cmbDept.Text, Description = txtDesc.Text, Type = cmbType.Text, Active = true, Interest = Convert.ToDouble(txtInterest.Text), MinTerm = Convert.ToInt32(txtMinTerm.Text), MaxTerm = Convert.ToInt32(txtMaxTerm.Text), MinValue = Convert.ToDouble(txtMinVal.Text), MaxValue = Convert.ToDouble(txtMaxVal.Text), AgentCommission = Convert.ToDouble(txtCom.Text), Holding = Convert.ToDouble(txtHolding.Text), ClosedAccountPenalty = Convert.ToDouble(txtClosed.Text), DaifPenalty = Convert.ToDouble(txtDaif.Text), RestructureFee = Convert.ToDouble(txtResFee.Text), RestructureInterest = Convert.ToDouble(txtResInt.Text), AdjustmentFee = Convert.ToDouble(txtAdjust.Text), LatePaymentPenalty=0 };
                        }
                        else
                        {
                            ser = new Service { Name = txtName.Text, Department = cmbDept.Text, Description = txtDesc.Text, Type = cmbType.Text, Active = true, Interest = Convert.ToDouble(txtInterest.Text), MinTerm = Convert.ToInt32(txtMinTerm.Text), MaxTerm = Convert.ToInt32(txtMaxTerm.Text), MinValue = Convert.ToDouble(txtMinVal.Text), MaxValue = Convert.ToDouble(txtMaxVal.Text), AgentCommission = Convert.ToDouble(txtCom.Text), Holding = 0, ClosedAccountPenalty = 0, DaifPenalty = 0, RestructureFee = 0, RestructureInterest = 0, AdjustmentFee = 0, LatePaymentPenalty=Convert.ToDouble(txtLtPen.Text) };
                        }
                        var deds = from dd in ctx.TempoDeductions
                                   select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                        var rqs = from rq in ctx.TempoRequirements
                                  select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };

                        foreach (var item in deds)
                        {
                            Deduction dd = new Deduction { DeductionNum = item.DedNumber, Name = item.Name, Percentage = item.Percentage };
                            ctx.Deductions.Add(dd);
                        }

                        foreach (var item in rqs)
                        {
                            Requirement rr = new Requirement { RequirementNum = item.ReqNumber, Name = item.Name, Description = item.Description };
                            ctx.Requirements.Add(rr);
                        }

                        ComboBoxItem typeItem = (ComboBoxItem)cmbType.SelectedItem;
                        string value = typeItem.Content.ToString();
                        if (value == "Collateral")
                        {
                            var ci = from c in ctx.TempCollateralInformations
                                     select c;

                            foreach (var itm in ci)
                            {
                                CollateralInformation cl = new CollateralInformation { CollateralInformationNum = itm.TempCollateralInformationNum, Field = itm.Field };
                                ctx.CollateralInformations.Add(cl);
                            }
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Added new Service " + txtName.Text };
                        ctx.AuditTrails.Add(at);

                        ctx.Services.Add(ser);
                        ctx.SaveChanges();
                        MessageBox.Show("Service has been successfully added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else
                {
                    using (var ctx = new newerContext())
                    {
                        var ser = ctx.Services.Find(sId);
                        ser.Name = txtName.Text;
                        ser.Department = cmbDept.Text;
                        ser.Description = txtDesc.Text;
                        ser.Type = cmbType.Text;
                        ser.Interest = Convert.ToDouble(txtInterest.Text);
                        ser.MinTerm = Convert.ToInt32(txtMinTerm.Text);
                        ser.MaxTerm = Convert.ToInt32(txtMaxTerm.Text);
                        ser.MinValue = Convert.ToDouble(txtMinVal.Text);
                        ser.MaxValue = Convert.ToDouble(txtMaxVal.Text);
                        if (cmbDept.Text == "Financing")
                        {
                            ser.Holding = Convert.ToDouble(txtHolding.Text);
                            ser.DaifPenalty = Convert.ToDouble(txtDaif.Text);
                            ser.ClosedAccountPenalty = Convert.ToDouble(txtClosed.Text);
                            ser.AgentCommission = Convert.ToDouble(txtCom.Text);
                            ser.RestructureFee = Convert.ToDouble(txtResFee.Text);
                            ser.RestructureInterest = Convert.ToDouble(txtResInt.Text);
                            ser.AdjustmentFee = Convert.ToDouble(txtAdjust.Text);
                            ser.LatePaymentPenalty = 0;
                        }
                        else
                        {
                            ser.Holding = 0;
                            ser.DaifPenalty = 0;
                            ser.ClosedAccountPenalty = 0;
                            ser.AgentCommission = 0;
                            ser.RestructureFee = 0;
                            ser.RestructureInterest = 0;
                            ser.AdjustmentFee = 0;
                            ser.LatePaymentPenalty = Convert.ToDouble(txtLtPen.Text);
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Service " + txtName.Text };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        MessageBox.Show("Service has been successfully updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to delete this record?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new newerContext())
                    {
                        var agt = ctx.Services.Find(sId);
                        agt.Active = false;
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Record has been successfully deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;
                tbInfo.IsSelected = true;
                if (status == "Add")
                {
                    using (var ctx = new newerContext())
                    {
                        ctx.Database.ExecuteSqlCommand("delete from dbo.TempoRequirements");
                        ctx.Database.ExecuteSqlCommand("delete from dbo.TempoDeductions");
                        ctx.Database.ExecuteSqlCommand("delete from dbo.TempCollateralInformations");
                    }
                }
                else
                {
                    using (var ctx = new newerContext())
                    {
                        var ctr = ctx.Loans.Where(x => x.ServiceID == sId && x.Status == "Released").Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Only selected values can be updated and Record cannot be deleted at this moment", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtName.IsEnabled = false;
                            cmbDept.IsEnabled = false;
                            cmbType.IsEnabled = false;
                            btnDel.IsEnabled = false;
                            //tbInfo.IsEnabled = false;
                        }

                        var ser = ctx.Services.Find(sId);
                        txtDesc.Text = ser.Description;
                        txtInterest.Text = ser.Interest.ToString();
                        txtMaxTerm.Text = ser.MaxTerm.ToString();
                        txtMaxVal.Text = ser.MaxValue.ToString();
                        txtMinTerm.Text = ser.MinTerm.ToString();
                        txtMinVal.Text = ser.MinValue.ToString();
                        txtName.Text = ser.Name;
                        cmbDept.Text = ser.Department;
                        cmbType.Text = ser.Type;
                        txtHolding.Text = ser.Holding.ToString();
                        txtDaif.Text = ser.DaifPenalty.ToString();
                        txtClosed.Text = ser.ClosedAccountPenalty.ToString();
                        txtCom.Text = ser.AgentCommission.ToString();
                        txtResFee.Text = ser.RestructureFee.ToString();
                        txtResInt.Text = ser.RestructureInterest.ToString();
                        txtAdjust.Text = ser.AdjustmentFee.ToString();

                        var reqs = from rq in ctx.Requirements
                                   where rq.ServiceID == sId
                                   select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs.ToList();
                        var deds = from dd in ctx.Deductions
                                   where dd.ServiceID == sId
                                   select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                        dgDed.ItemsSource = deds.ToList();

                        var reqs2 = from rq in ctx.CollateralInformations
                                   where rq.ServiceID == sId
                                   select new { Number = rq.CollateralInformationNum, Field = rq.Field };
                        dgCI.ItemsSource = reqs2.ToList();

                    }
                }
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAddDed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAddDed.Content.ToString() == "Add")
                {
                    grpDed.Visibility = Visibility.Visible;
                    btnAddDed.Content = "Save";
                    btnEdtDed.Content = "Cancel";
                    btnEdtDed.IsEnabled = true;
                    btnDelDed.Visibility = Visibility.Hidden;
                    tbDed.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    tbPen.IsEnabled = false;
                    tbReq.IsEnabled = false;
                }
                else if (btnAddDed.Content.ToString() == "Save")
                {
                    if (txtDedName.Text == "" || txtDedPerc.Text == "" || txtDedName.Text == "?" || txtDedPerc.Text == "?")
                    {
                        System.Windows.MessageBox.Show("Please enter the correct format and/or complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            var ctr = ctx.Deductions.Where(x => x.ServiceID == sId).Count() + 1;
                            Deduction td = new Deduction { ServiceID = sId, DeductionNum = ctr, Name = txtDedName.Text, Percentage = Convert.ToDouble(txtDedPerc.Text) };
                            ctx.Deductions.Add(td);
                            ctx.SaveChanges();

                            var deds = from dd in ctx.Deductions
                                       where dd.ServiceID == sId
                                       select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                            dgDed.ItemsSource = deds.ToList();
                        }
                        reset();
                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        var ctr = ctx.TempoDeductions.Count() + 1;
                        TempoDeduction td = new TempoDeduction { DeductionNum = ctr, Name = txtDedName.Text, Percentage = Convert.ToDouble(txtDedPerc.Text) };
                        ctx.TempoDeductions.Add(td);
                        ctx.SaveChanges();

                        var deds = from dd in ctx.TempoDeductions
                                   select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                        dgDed.ItemsSource = deds.ToList();
                        reset();

                    }

                    reset();
                }
                else //for update
                {
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            int num = Convert.ToInt32(getRow(dgDed, 0));
                            var td = ctx.Deductions.Where(x => x.DeductionNum == num && x.ServiceID == sId).First();
                            td.Name = txtDedName.Text;
                            td.Percentage = Convert.ToDouble(txtDedPerc.Text);
                            ctx.SaveChanges();

                            var deds = from dd in ctx.Deductions
                                       where dd.ServiceID == sId
                                       select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                            dgDed.ItemsSource = deds.ToList();
                        }
                        reset();
                        return;
                    }


                    using (var ctx = new newerContext())
                    {
                        int num = Convert.ToInt32(getRow(dgDed, 0));
                        var td = ctx.TempoDeductions.Where(x => x.DeductionNum == num).First();
                        td.Name = txtDedName.Text;
                        td.Percentage = Convert.ToDouble(txtDedPerc.Text);
                        ctx.SaveChanges();
                        var deds = from dd in ctx.TempoDeductions
                                   select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                        dgDed.ItemsSource = deds.ToList();
                        reset();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAddReq_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAddReq.Content.ToString() != "Add")
                {
                    if (lblReqName.Content == "?" || lblReqDesc.Content == "?" || String.IsNullOrWhiteSpace(txtReqName.Text) || String.IsNullOrWhiteSpace(txtReqDesc.Text))
                    {
                        System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                if (btnAddReq.Content.ToString() == "Add")
                {
                    grpReq.Visibility = Visibility.Visible;
                    btnAddReq.Content = "Save";
                    btnEdtReq.Content = "Cancel";
                    btnEdtReq.IsEnabled = true;
                    btnDelReq.Visibility = Visibility.Hidden;

                    tbDed.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    tbPen.IsEnabled = false;
                    tbReq.IsEnabled = false;
                }
                else if (btnAddReq.Content.ToString() == "Save")
                {
                    if (txtReqName.Text == "" || txtReqDesc.Text == "")
                    {
                        System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            var ctr = ctx.Requirements.Where(x => x.ServiceID == sId).Count() + 1;
                            Requirement tr = new Requirement { ServiceID = sId, RequirementNum = ctr, Name = txtReqName.Text, Description = txtReqDesc.Text };
                            ctx.Requirements.Add(tr);
                            ctx.SaveChanges();
                            var reqs = from rq in ctx.Requirements
                                       where rq.ServiceID == sId
                                       select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                            dgReq.ItemsSource = reqs.ToList();
                        }
                        reset();
                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        var ctr = ctx.TempoRequirements.Count() + 1;
                        TempoRequirement tr = new TempoRequirement { RequirementNum = ctr, Name = txtReqName.Text, Description = txtReqDesc.Text };
                        ctx.TempoRequirements.Add(tr);
                        ctx.SaveChanges();

                        var rqs = from rq in ctx.TempoRequirements
                                  select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = rqs.ToList();
                        reset();

                    }

                    reset();
                }
                else //for update
                {
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            int num = Convert.ToInt32(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            tr.Name = txtReqName.Text;
                            tr.Description = txtReqDesc.Text;
                            ctx.SaveChanges();

                            var reqs = from rq in ctx.Requirements
                                       where rq.ServiceID == sId
                                       select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                            dgReq.ItemsSource = reqs.ToList();
                        }
                        reset();
                        return;
                    }


                    using (var ctx = new newerContext())
                    {
                        int num = Convert.ToInt32(getRow(dgReq, 0));
                        var tr = ctx.TempoRequirements.Where(x => x.RequirementNum == num).First();
                        tr.Name = txtReqName.Text;
                        tr.Description = txtReqDesc.Text;
                        ctx.SaveChanges();
                        var rqs = from rq in ctx.TempoRequirements
                                  select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = rqs.ToList();
                        reset();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnEdtReq_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtReq.Content.ToString() == "Edit")
                {
                    btnEdtReq.Content = "Cancel";
                    btnAddReq.Content = "Update";
                    dgReq.IsEnabled = false;
                    btnDelReq.Visibility = Visibility.Hidden;
                    grpReq.Visibility = Visibility.Visible;
                    tbDed.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    tbPen.IsEnabled = false;
                    tbReq.IsEnabled = false;
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            int num = Convert.ToInt32(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text=tr.Name;
                            txtReqDesc.Text=tr.Description;
                        }

                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        int num = Convert.ToInt32(getRow(dgReq, 0));
                        var tr = ctx.TempoRequirements.Where(x => x.RequirementNum == num).First();
                        txtReqName.Text = tr.Name;
                        txtReqDesc.Text = tr.Description;
                    }
                }
                else
                {
                    reset();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDelReq_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new newerContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        int nums = Convert.ToInt32(getRow(dgReq, 0));
                        var tr1 = ctx.Requirements.Where(x => x.RequirementNum == nums && x.ServiceID==sId).First();
                        ctx.Requirements.Remove(tr1);
                        ctx.SaveChanges();

                        var reqs1 = from req in ctx.Requirements
                                    where req.ServiceID == sId
                                   select req;
                        int ctr1 = 1;
                        foreach (var item in reqs1)
                        {
                            item.RequirementNum = ctr1;
                            ctr1++;
                        }

                        ctx.SaveChanges();
                        var reqs2= from rq in ctx.Requirements
                                   where rq.ServiceID == sId
                                   select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs2.ToList();
                        return;
                    }

                    int num = Convert.ToInt32(getRow(dgReq, 0));
                    var tr = ctx.TempoRequirements.Where(x => x.RequirementNum == num).First();
                    ctx.TempoRequirements.Remove(tr);
                    ctx.SaveChanges();

                    var reqs = from req in ctx.TempoRequirements
                               select req;
                    int ctr = 1;
                    foreach (var item in reqs)
                    {
                        item.RequirementNum = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var rqs = from rq in ctx.TempoRequirements
                              select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                    dgReq.ItemsSource = rqs.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void btnEdtDed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtDed.Content.ToString() == "Edit")
                {
                    btnEdtDed.Content = "Cancel";
                    btnAddDed.Content = "Update";
                    dgDed.IsEnabled = false;
                    btnDelDed.Visibility = Visibility.Hidden;
                    grpDed.Visibility = Visibility.Visible;
                    tbDed.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    tbPen.IsEnabled = false;
                    tbReq.IsEnabled = false;
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            int num = Convert.ToInt32(getRow(dgDed, 0));
                            var td = ctx.Deductions.Where(x => x.DeductionNum == num && x.ServiceID==sId).First();
                            txtDedName.Text = td.Name;
                            txtDedPerc.Text = td.Percentage.ToString();
                        }

                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        int num = Convert.ToInt32(getRow(dgDed, 0));
                        var td = ctx.TempoDeductions.Where(x => x.DeductionNum == num).First();
                        txtDedName.Text = td.Name;
                        txtDedPerc.Text = td.Percentage.ToString();
                    }
                }
                else
                {
                    reset();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDelDed_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new newerContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        int num1 = Convert.ToInt32(getRow(dgDed, 0));
                        var td1 = ctx.Deductions.Where(x => x.DeductionNum == num1 && x.ServiceID==sId).First();
                        ctx.Deductions.Remove(td1);
                        ctx.SaveChanges();

                        var deds1 = from dd in ctx.Deductions
                                    where dd.ServiceID == sId
                                   select dd;
                        int ctr1 = 1;
                        foreach (var item in deds1)
                        {
                            item.DeductionNum = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();

                        var deds = from dd in ctx.Deductions
                                   where dd.ServiceID == sId
                                   select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                        dgDed.ItemsSource = deds.ToList();

                        return;
                    }

                    int num = Convert.ToInt32(getRow(dgDed, 0));
                    var td = ctx.TempoDeductions.Where(x => x.DeductionNum == num).First();
                    ctx.TempoDeductions.Remove(td);
                    ctx.SaveChanges();

                    var deds3 = from dd in ctx.TempoDeductions
                               select dd;
                    int ctr = 1;
                    foreach (var item in deds3)
                    {
                        item.DeductionNum = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var deds4 = from dd in ctx.TempoDeductions
                               select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                    dgDed.ItemsSource = deds4.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtName, lblName, true);
        }

        private void txtMinTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (Convert.ToInt32(txtMinTerm.Text) > Convert.ToInt32(txtMaxTerm.Text))
                {
                    lblMaxTerm.Foreground = Brushes.Red;
                    lblMinTerm.Foreground = Brushes.Red;
                    lblMaxTerm.Content = "?";
                    lblMinTerm.Content = "?";
                }
                else
                {
                    lblMaxTerm.Foreground = Brushes.Green;
                    lblMinTerm.Foreground = Brushes.Green;
                    lblMaxTerm.Content = "✔";
                    lblMinTerm.Content = "✔";
                }
                checkNumeric(txtMinTerm, lblMinTerm, true);
            }
            catch
            {
                lblMaxTerm.Foreground = Brushes.Red;
                lblMinTerm.Foreground = Brushes.Red;
                lblMaxTerm.Content = "?";
                lblMinTerm.Content = "?";
            }
        }

        private void txtMaxTerm_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtMinTerm.Text) > Convert.ToInt32(txtMaxTerm.Text))
                {
                    lblMaxTerm.Foreground = Brushes.Red;
                    lblMinTerm.Foreground = Brushes.Red;
                    lblMaxTerm.Content = "?";
                    lblMinTerm.Content = "?";
                }
                else
                {
                    lblMaxTerm.Foreground = Brushes.Green;
                    lblMinTerm.Foreground = Brushes.Green;
                    lblMaxTerm.Content = "✔";
                    lblMinTerm.Content = "✔";
                }
                checkNumeric(txtMaxTerm, lblMaxTerm, true);
            }
            catch
            {
                lblMaxTerm.Foreground = Brushes.Red;
                lblMinTerm.Foreground = Brushes.Red;
                lblMaxTerm.Content = "?";
                lblMinTerm.Content = "?";
            }
        }

        private void txtMinVal_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(txtMinVal.Text) > Convert.ToDouble(txtMaxVal.Text))
                {
                    lblMaxVal.Foreground = Brushes.Red;
                    lblMinVal.Foreground = Brushes.Red;
                    lblMinVal.Content = "?";
                    lblMaxVal.Content = "?";
                }
                else
                {
                    lblMaxVal.Foreground = Brushes.Green;
                    lblMaxVal.Foreground = Brushes.Green;
                    lblMaxVal.Content = "✔";
                    lblMinVal.Content = "✔";
                }
                checkDouble(txtMinVal, lblMinVal, true);
            }
            catch
            {
                lblMaxVal.Foreground = Brushes.Red;
                lblMinVal.Foreground = Brushes.Red;
                lblMinVal.Content = "?";
                lblMaxVal.Content = "?";
            }
        }

        private void txtMaxVal_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToDouble(txtMinVal.Text) > Convert.ToDouble(txtMaxVal.Text))
                {
                    lblMaxVal.Foreground = Brushes.Red;
                    lblMinVal.Foreground = Brushes.Red;
                    lblMinVal.Content = "?";
                    lblMaxVal.Content = "?";
                }
                else
                {
                    lblMaxVal.Foreground = Brushes.Green;
                    lblMaxVal.Foreground = Brushes.Green;
                    lblMaxVal.Content = "✔";
                    lblMinVal.Content = "✔";
                }
                checkDouble(txtMaxVal, lblMaxVal, true);
            }
            catch
            {
                lblMaxVal.Foreground = Brushes.Red;
                lblMinVal.Foreground = Brushes.Red;
                lblMinVal.Content = "?";
                lblMaxVal.Content = "?";
            }
        }

        private void txtInterest_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtInterest, lblInterest, true);
        }

        private void txtDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtDesc, lblDesc, true);
        }

        private void txtReqName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtReqName, lblReqName, true);
        }

        private void txtReqDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtReqDesc, lblReqDesc, true);
        }

        private void cmbDept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem typeItem = (ComboBoxItem)cmbDept.SelectedItem;
                string value = typeItem.Content.ToString();
                if (value == "Financing")
                {
                    tbPen.IsEnabled = true;
                    grdFinan.Visibility = Visibility.Visible;
                    grdMicro.Visibility = Visibility.Hidden;
                }
                else if (value == "Micro Business")
                {
                    tbPen.IsEnabled = true;
                    grdFinan.Visibility = Visibility.Hidden;
                    grdMicro.Visibility = Visibility.Visible;
                }
                else
                {
                    tbPen.IsEnabled = !true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtCom_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtCom, lblCom, true);
        }

        private void txtDedName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtDedName, lblDedName, true);
        }

        private void txtDedPerc_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtDedPerc, lblDedPerc, true);
        }

        private void txtHolding_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtHolding, lblPenHolding, true);
        }

        private void txtAdjust_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtAdjust, lblPenAdj, true);
        }

        private void txtDaif_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtDaif, lblPenDaif, true);
        }

        private void txtClosed_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtClosed, lblPenCA, true);
        }

        private void txtResFee_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtResFee, lblPenRes, true);
        }

        private void txtResInt_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtResInt, lblPenIn, true);
        }

        private void txtLtPen_LostFocus(object sender, RoutedEventArgs e)
        {
            checkDouble(txtLtPen, lblPenLate, true);
        }

        private void btnAddCI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAddCI.Content.ToString() != "Add")
                {
                    if (lblCIName.Content == "?" || String.IsNullOrWhiteSpace(txtCIName.Text))
                    {
                        System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                if (btnAddCI.Content.ToString() == "Add")
                {
                    grpCI.Visibility = Visibility.Visible;
                    btnAddCI.Content = "Save";
                    btnEdtCI.Content = "Cancel";
                    btnEdtCI.IsEnabled = true;
                    btnDelCI.Visibility = Visibility.Hidden;

                    tbDed.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    tbPen.IsEnabled = false;
                    tbReq.IsEnabled = false;
                    tbCol.IsEnabled = false;
                }
                else if (btnAddCI.Content.ToString() == "Save")
                {
                    if (txtCIName.Text == "")
                    {
                        System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            var c = ctx.CollateralInformations.Where(x => x.ServiceID == sId && x.Field == txtCIName.Text).Count();
                            if (c > 0)
                            {
                                System.Windows.MessageBox.Show("Field already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }


                            var ctr = ctx.CollateralInformations.Where(x => x.ServiceID == sId).Count() + 1;
                            CollateralInformation ci = new CollateralInformation { CollateralInformationNum = ctr, Field = txtCIName.Text, ServiceID = sId };
                            ctx.CollateralInformations.Add(ci);
                            ctx.SaveChanges();
                            var reqs = from rq in ctx.CollateralInformations
                                       where rq.ServiceID == sId
                                       select new { Number = rq.CollateralInformationNum, Field = rq.Field};
                            dgCI.ItemsSource = reqs.ToList();
                        }
                        reset();
                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        var ctr = ctx.TempCollateralInformations.Count() + 1;
                        TempCollateralInformation ci = new TempCollateralInformation { Field = txtCIName.Text, TempCollateralInformationNum = ctr };
                        ctx.TempCollateralInformations.Add(ci);
                        ctx.SaveChanges();

                        var rqs = from rq in ctx.TempCollateralInformations
                                  select new { Number = rq.TempCollateralInformationNum, Field = rq.Field };
                        dgCI.ItemsSource = rqs.ToList();
                        reset();

                    }

                    reset();
                }
                else //for update
                {
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            

                            int num = Convert.ToInt32(getRow(dgCI, 0));
                            var ci = ctx.CollateralInformations.Where(x => x.CollateralInformationNum == num && x.ServiceID == sId).First();

                            if (ci.Field != txtCIName.Text)
                            {
                                var c = ctx.CollateralInformations.Where(x => x.ServiceID == sId && x.Field == txtCIName.Text).Count();
                                if (c > 0)
                                {
                                    System.Windows.MessageBox.Show("Field already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            
                            ci.Field = txtCIName.Text;
                            ctx.SaveChanges();

                            var reqs = from rq in ctx.CollateralInformations
                                       where rq.ServiceID == sId
                                       select new { Number = rq.CollateralInformationNum, Field = rq.Field };
                            dgCI.ItemsSource = reqs.ToList();
                            
                        }
                        reset();
                        return;
                    }


                    using (var ctx = new newerContext())
                    {
                        int num = Convert.ToInt32(getRow(dgCI, 0));
                        var ci = ctx.TempCollateralInformations.Where(x=> x.TempCollateralInformationNum == num).First();
                        ci.Field = txtCIName.Text;
                        ctx.SaveChanges();
                        var rqs = from rq in ctx.TempCollateralInformations
                                  select new { Number = rq.TempCollateralInformationNum, Field = rq.Field };
                        dgCI.ItemsSource = rqs.ToList();
                        reset();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnEdtCI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtCI.Content.ToString() == "Edit")
                {
                    btnEdtCI.Content = "Cancel";
                    btnAddCI.Content = "Update";
                    dgCI.IsEnabled = false;
                    btnDelCI.Visibility = Visibility.Hidden;
                    grpCI.Visibility = Visibility.Visible;
                    tbDed.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    tbPen.IsEnabled = false;
                    tbReq.IsEnabled = false;
                    tbCol.IsEnabled = false;
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new newerContext())
                        {
                            int num = Convert.ToInt32(getRow(dgCI, 0));
                            var ci = ctx.CollateralInformations.Where(x => x.CollateralInformationNum == num && x.ServiceID == sId).First();
                            txtCIName.Text = ci.Field;
                        }

                        return;
                    }

                    using (var ctx = new newerContext())
                    {
                        int num = Convert.ToInt32(getRow(dgCI, 0));
                        var ci = ctx.TempCollateralInformations.Where(x => x.TempCollateralInformationNum == num).First();
                        txtCIName.Text = ci.Field;
                    }
                }
                else
                {
                    reset();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDelCI_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new newerContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        int nums = Convert.ToInt32(getRow(dgCI, 0));
                        var tr1 = ctx.CollateralInformations.Where(x => x.CollateralInformationNum == nums && x.ServiceID == sId).First();
                        ctx.CollateralInformations.Remove(tr1);
                        ctx.SaveChanges();

                        var reqs1 = from req in ctx.CollateralInformations
                                    where req.ServiceID == sId
                                    select req;
                        int ctr1 = 1;
                        foreach (var item in reqs1)
                        {
                            item.CollateralInformationNum = ctr1;
                            ctr1++;
                        }

                        ctx.SaveChanges();
                        var reqs = from rq in ctx.CollateralInformations
                                   where rq.ServiceID == sId
                                   select new { Number = rq.CollateralInformationNum, Field = rq.Field };
                        dgCI.ItemsSource = reqs.ToList();
                        return;
                    }

                    int num = Convert.ToInt32(getRow(dgCI, 0));
                    var tr = ctx.TempCollateralInformations.Where(x => x.TempCollateralInformationNum == num).First();
                    ctx.TempCollateralInformations.Remove(tr);
                    ctx.SaveChanges();

                    var reqs2 = from req in ctx.TempCollateralInformations
                               select req;
                    int ctr = 1;
                    foreach (var item in reqs2)
                    {
                        item.TempCollateralInformationNum = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var rqs3 = from rq in ctx.TempCollateralInformations
                              select new { Number = rq.TempCollateralInformationNum, Field = rq.Field };
                    dgCI.ItemsSource = rqs3.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem typeItem = (ComboBoxItem)cmbType.SelectedItem;
                string value = typeItem.Content.ToString();
                if (value == "Collateral")
                {
                    tbCol.Visibility = Visibility.Visible;
                }
                else
                {
                    tbCol.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtCIName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtCIName, lblCIName, true);
        }

        private void txtMinTerm_GotFocus(object sender, RoutedEventArgs e)
        {

        }

    }
}
