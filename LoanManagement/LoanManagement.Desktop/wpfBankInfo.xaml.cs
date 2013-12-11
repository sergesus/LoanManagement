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

using LoanManagement.Domain;
using System.Data.Entity;
using System.Windows.Forms;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfBranchInfo.xaml
    /// </summary>
    public partial class wpfBranchInfo : MetroWindow
    {
        public string status;
        public int bId;
        public int UserID;

        public wpfBranchInfo()
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
                if (!Regex.IsMatch(str, @"^[a-zA-Z0-9 @.]*$") || str.Length > 25)
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
        
        public void reset()
        {
            try
            {
                lblCity.Content = "";
                lblProvince.Content = "";
                lblStreet.Content = "";

                tbAddress.IsEnabled = !false;
                tbInfo.IsEnabled =! false;

                btnAddAddress.Content = "Add";
                btnEdtAddress.Content = "Edit";
                dgAddress.IsEnabled = true;
                btnDelAddress.Visibility = Visibility.Visible;
                grpAddress.Visibility = Visibility.Hidden;
                txtCity.Text = "";
                txtProvince.Text = "";
                txtStreet.Text = "";

                int num1 = 0;
                if (status == "Add")
                {
                    using (var ctx = new iContext())
                    {
                        num1 = ctx.TempAdresses.Count();
                    }
                }
                else
                {
                    using (var ctx = new iContext())
                    {
                        num1 = ctx.BankAdresses.Where(x => x.BankID == bId).Count();
                    }
                }
                if (num1 > 0)
                {
                    btnDelAddress.IsEnabled = true;
                    btnEdtAddress.IsEnabled = true;
                }
                else
                {
                    btnDelAddress.IsEnabled = !true;
                    btnEdtAddress.IsEnabled = !true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAddAddress.Content.ToString() != "Add")
                {
                    if (lblStreet.Content == "?" || lblProvince.Content == "?" || lblCity.Content == "?"
                    || String.IsNullOrWhiteSpace(txtStreet.Text) || String.IsNullOrWhiteSpace(txtProvince.Text) || String.IsNullOrWhiteSpace(txtCity.Text))
                    {
                        System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields");
                        return;
                    }
                }

                if (btnAddAddress.Content.ToString() == "Add")
                {
                    grpAddress.Visibility = Visibility.Visible;
                    btnAddAddress.Content = "Save";
                    btnEdtAddress.Content = "Cancel";
                    btnEdtAddress.IsEnabled = true;
                    btnDelAddress.Visibility = Visibility.Hidden;

                    tbAddress.IsEnabled = false;
                    tbInfo.IsEnabled = false;

                }
                else if (btnAddAddress.Content.ToString() == "Save")
                {
                    if (txtCity.Text == "" || txtProvince.Text == "" || txtStreet.Text == "")
                    {
                        System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new iContext())
                        {
                            var add = ctx.BankAdresses.Where(x => x.BankID == bId).Count();
                            int ctr = add + 1;
                            BankAddress adds = new BankAddress { BankID = bId, BankNum = ctr, Street = txtStreet.Text, Province = txtProvince.Text, City = txtCity.Text };
                            ctx.BankAdresses.Add(adds);
                            ctx.SaveChanges();
                            var adds1 = from ad in ctx.BankAdresses
                                        where ad.BankID == bId
                                        select new { BankNumber = ad.BankNum, Street = ad.Street, Province = ad.Province, City = ad.City };
                            dgAddress.ItemsSource = adds1.ToList();
                        }


                        reset();
                        return;
                    }

                    using (var ctx = new iContext())
                    {
                        TempAddress add = new TempAddress { Street = txtStreet.Text, Province = txtProvince.Text, City = txtCity.Text };
                        ctx.TempAdresses.Add(add);
                        ctx.SaveChanges();
                        dgAddress.ItemsSource = ctx.TempAdresses.ToList();
                    }

                    reset();
                }
                else
                {
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new iContext())
                        {
                            int bankNum = Convert.ToInt32(getRow(dgAddress, 0));
                            var add = ctx.BankAdresses.Where(x => x.BankID == bId && x.BankNum == bankNum).First();
                            add.City = txtCity.Text;
                            add.Province = txtProvince.Text;
                            add.Street = txtStreet.Text;
                            ctx.SaveChanges();
                            var adds = from ad in ctx.BankAdresses
                                       where ad.BankID == bId
                                       select new { BankNumber = ad.BankNum, Street = ad.Street, Province = ad.Province, City = ad.City };
                            dgAddress.ItemsSource = adds.ToList();
                        }
                        reset();
                        return;
                    }


                    using (var ctx = new iContext())
                    {
                        var add = ctx.TempAdresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        add.City = txtCity.Text;
                        add.Province = txtProvince.Text;
                        add.Street = txtStreet.Text;
                        ctx.SaveChanges();
                        dgAddress.ItemsSource = ctx.TempAdresses.ToList();
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

        private void btnEdtAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtAddress.Content.ToString() == "Edit")
                {
                    btnEdtAddress.Content = "Cancel";
                    btnAddAddress.Content = "Update";
                    dgAddress.IsEnabled = false;
                    btnDelAddress.Visibility = Visibility.Hidden;
                    grpAddress.Visibility = Visibility.Visible;
                    tbAddress.IsEnabled = false;
                    tbInfo.IsEnabled = false;
                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new iContext())
                        {
                            var add = ctx.BankAdresses.Where(x=> x.BankID==bId && x.BankNum==Convert.ToInt32(getRow(dgAddress,0))).First();
                            txtCity.Text = add.City;
                            txtProvince.Text = add.Province;
                            txtStreet.Text = add.Street;
                        }

                        return;
                    }

                    using (var ctx = new iContext())
                    {
                        var add = ctx.TempAdresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        txtCity.Text = add.City;
                        txtProvince.Text = add.Province;
                        txtStreet.Text = add.Street;
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

        private void btnDelAddress_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new iContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        int bankNum = Convert.ToInt32(getRow(dgAddress, 0));
                        var add1 = ctx.BankAdresses.Where(x => x.BankID == bId && x.BankNum == bankNum).First();
                        ctx.BankAdresses.Remove(add1);
                        ctx.SaveChanges();

                        var address = from ad in ctx.BankAdresses
                                      select ad;
                        int ctr = 1;
                        foreach (var item in address)
                        {
                            item.BankNum = ctr;
                            ctr++;

                        }
                        ctx.SaveChanges();
                        var adds = from ad in ctx.BankAdresses
                                   where ad.BankID == bId
                                   select new { BankNumber = ad.BankNum, Street = ad.Street, Province = ad.Province, City = ad.City };
                        dgAddress.ItemsSource = adds.ToList();
                        return;
                    }

                    var add = ctx.TempAdresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                    ctx.TempAdresses.Remove(add);
                    ctx.SaveChanges();
                    dgAddress.ItemsSource = ctx.TempAdresses.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lblName.Content == "?" || lblDesc.Content == "?"
                    || String.IsNullOrWhiteSpace(txtName.Text))
                {
                    System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (status == "Add")
                {
                    using (var ctx = new iContext())
                    {
                        var num = ctx.Banks.Where(x => x.BankName == txtName.Text).Count();
                        if (num > 0)
                        {
                            return;
                        }

                        Bank bank = new Bank { BankName = txtName.Text, Description = txtDesc.Text, Active = true };


                        var add = from ad in ctx.TempAdresses
                                  select ad;
                        int ctr = 1;
                        foreach (var item in add)
                        {
                            BankAddress bAd = new BankAddress { BankNum = ctr, City = item.City, Province = item.Province, Street = item.Street };
                            ctx.BankAdresses.Add(bAd);
                            ctr++;
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Added new Bank " + txtName.Text };
                        ctx.AuditTrails.Add(at);

                        ctx.Banks.Add(bank);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("New Bank Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else
                {
                    using (var ctx = new iContext())
                    {
                        var bank = ctx.Banks.Find(bId);
                        bank.BankName = txtName.Text;
                        bank.Description = txtDesc.Text;
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Bank " + txtName.Text };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Bank Successfully Updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                using (var ctx = new iContext())
                {
                    DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        Bank bnk = ctx.Banks.Find(bId);
                        bnk.Active = false;
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Bank has been successfuly deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    using (var ctx = new iContext())
                    {
                        ctx.Database.ExecuteSqlCommand("delete from dbo.TempAddresses");
                    }
                    reset();
                    btnDel.Visibility = Visibility.Hidden;
                }
                else
                {
                    using (var ctx = new iContext())
                    {
                        var ctr = ctx.Loans.Where(x => x.BankID == bId && x.Status == "Released").Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Only selected values be updated and Record cannot be deleted at this moment", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            btnDel.IsEnabled = false;
                            txtName.IsEnabled = false;
                        }

                        var add = from ad in ctx.BankAdresses
                                  where ad.BankID == bId
                                  select new { BankNumber = ad.BankNum, Street = ad.Street, Province = ad.Province, City = ad.City };
                        dgAddress.ItemsSource = add.ToList();
                    }
                    reset();
                    btnDel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtName, lblName, true);
        }

        private void txtDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtDesc, lblDesc, true);
        }

        private void txtStreet_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtStreet, lblStreet, true);
        }

        private void txtProvince_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtProvince, lblProvince, true);
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtProvince, lblProvince, true);
        }
    }
}
