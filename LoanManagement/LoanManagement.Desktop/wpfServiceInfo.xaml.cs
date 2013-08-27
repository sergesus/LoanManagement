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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfServiceInfo.xaml
    /// </summary>
    public partial class wpfServiceInfo : MetroWindow
    {
        public string status;
        public int sId;

        public wpfServiceInfo()
        {
            InitializeComponent();
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
            int num1 = 0;
            int num2 = 0;
            if (status == "Add")
            {
                using (var ctx = new MyLoanContext())
                {
                    num1 = ctx.TempoRequirements.Count();
                    num2 = ctx.TempoDeductions.Count();
                }
            }
            else
            {
                using (var ctx = new MyLoanContext())
                {
                    num1 = ctx.Requirements.Where(x => x.ServiceID==sId).Count();
                    num2 = ctx.Deductions.Where(x => x.ServiceID == sId).Count();
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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" || txtDesc.Text == "" || txtInterest.Text == "" || txtMaxTerm.Text == "" || txtMinVal.Text == "" || txtMinTerm.Text == "" || cmbDept.Text == "" || cmbType.Text == "")
            {
                return;
            }

            if (status == "Add")
            {


                using (var ctx = new MyLoanContext())
                {
                    Service ser = new Service { Name = txtName.Text, Department = cmbDept.Text, Description = txtDesc.Text, Type = cmbType.Text, Active = true, Interest = Convert.ToDouble(txtInterest.Text), MinTerm = Convert.ToInt32(txtMinTerm.Text), MaxTerm = Convert.ToInt32(txtMaxTerm.Text), MinValue = Convert.ToDouble(txtMinVal.Text), MaxValue = Convert.ToDouble(txtMaxVal.Text), AgentCommission=Convert.ToDouble(txtCom.Text), Holding=Convert.ToDouble(txtHolding.Text), ClosedAccountPenalty=Convert.ToDouble(txtClosed.Text), DaifPenalty= Convert.ToDouble(txtDaif.Text), RestructureFee=Convert.ToDouble(txtResFee.Text), RestructureInterest=Convert.ToDouble(txtResInt.Text), AdjustmentFee= Convert.ToDouble(txtAdjust.Text) };


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

                    ctx.Services.Add(ser);
                    ctx.SaveChanges();
                    MessageBox.Show("Service added");
                    this.Close();
                }
            }
            else
            {
                using (var ctx = new MyLoanContext())
                {
                    var ser=ctx.Services.Find(sId);
                    ser.Name = txtName.Text;
                    ser.Department = cmbDept.Text;
                    ser.Description = txtDesc.Text;
                    ser.Type = cmbType.Text;
                    ser.Interest = Convert.ToDouble(txtInterest.Text);
                    ser.MinTerm = Convert.ToInt32(txtMinTerm.Text);
                    ser.MaxTerm = Convert.ToInt32(txtMaxTerm.Text);
                    ser.MinValue = Convert.ToDouble(txtMinVal.Text);
                    ser.MaxValue = Convert.ToDouble(txtMaxVal.Text);
                    ser.Holding = Convert.ToDouble(txtHolding.Text);
                    ser.DaifPenalty = Convert.ToDouble(txtDaif.Text);
                    ser.ClosedAccountPenalty = Convert.ToDouble(txtClosed.Text);
                    ser.AgentCommission = Convert.ToDouble(txtCom.Text);
                    ser.RestructureFee = Convert.ToDouble(txtResFee.Text);
                    ser.RestructureInterest = Convert.ToDouble(txtResInt.Text);
                    ser.AdjustmentFee = Convert.ToDouble(txtAdjust.Text);
                    ctx.SaveChanges();
                    MessageBox.Show("Service updated");
                    this.Close();
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;
            if (status == "Add")
            {
                using (var ctx = new MyLoanContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempoRequirements");
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempoDeductions");
                }
            }
            else 
            {
                using (var ctx = new MyLoanContext())
                {
                    var ser=ctx.Services.Find(sId);
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
                               where dd.ServiceID==sId
                               select new { DedNumber = dd.DeductionNum, Name = dd.Name, Percentage = dd.Percentage };
                    dgDed.ItemsSource = deds.ToList();

                }
            }
            reset();
        }

        private void btnAddDed_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddDed.Content.ToString() == "Add")
            {
                grpDed.Visibility = Visibility.Visible;
                btnAddDed.Content = "Save";
                btnEdtDed.Content = "Cancel";
                btnEdtDed.IsEnabled = true;
                btnDelDed.Visibility = Visibility.Hidden;

            }
            else if (btnAddDed.Content.ToString() == "Save")
            {
                if (txtDedName.Text == "" || txtDedPerc.Text == "")
                {
                    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //for view
                if (status == "View")
                {
                    using (var ctx = new MyLoanContext())
                    {
                        var ctr = ctx.Deductions.Where(x => x.ServiceID == sId).Count() + 1;
                        Deduction td = new Deduction {ServiceID=sId, DeductionNum = ctr, Name = txtDedName.Text, Percentage = Convert.ToDouble(txtDedPerc.Text) };
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

                using (var ctx = new MyLoanContext())
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
                    using (var ctx = new MyLoanContext())
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


                using (var ctx = new MyLoanContext())
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

        private void btnAddReq_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddReq.Content.ToString() == "Add")
            {
                grpReq.Visibility = Visibility.Visible;
                btnAddReq.Content = "Save";
                btnEdtReq.Content = "Cancel";
                btnEdtReq.IsEnabled = true;
                btnDelReq.Visibility = Visibility.Hidden;

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
                    using (var ctx = new MyLoanContext())
                    {
                        var ctr = ctx.Requirements.Where(x => x.ServiceID == sId).Count() + 1;
                        Requirement tr = new Requirement {ServiceID=sId, RequirementNum = ctr, Name = txtReqName.Text, Description = txtReqDesc.Text };
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

                using (var ctx = new MyLoanContext())
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
                    using (var ctx = new MyLoanContext())
                    {
                        int num = Convert.ToInt32(getRow(dgReq, 0));
                        var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID==sId).First();
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


                using (var ctx = new MyLoanContext())
                {
                    int num = Convert.ToInt32(getRow(dgReq, 0));
                    var tr  = ctx.TempoRequirements.Where(x=> x.RequirementNum==num).First();
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

                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new MyLoanContext())
                        {
                            int num = Convert.ToInt32(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text=tr.Name;
                            txtReqDesc.Text=tr.Description;
                        }

                        return;
                    }

                    using (var ctx = new MyLoanContext())
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
                return;
            }
        }

        private void btnDelReq_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new MyLoanContext())
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

                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new MyLoanContext())
                        {
                            int num = Convert.ToInt32(getRow(dgDed, 0));
                            var td = ctx.Deductions.Where(x => x.DeductionNum == num && x.ServiceID==sId).First();
                            txtDedName.Text = td.Name;
                            txtDedPerc.Text = td.Percentage.ToString();
                        }

                        return;
                    }

                    using (var ctx = new MyLoanContext())
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
                return;
            }
        }

        private void btnDelDed_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new MyLoanContext())
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
                    return;
                }
            }
        }
    }
}
