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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfBranchInfo.xaml
    /// </summary>
    public partial class wpfBranchInfo : MetroWindow
    {
        public string status;
        public int bId;

        public wpfBranchInfo()
        {
            InitializeComponent();
        }

        public void reset()
        {
            try
            {
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
                if (btnAddAddress.Content.ToString() == "Add")
                {
                    grpAddress.Visibility = Visibility.Visible;
                    btnAddAddress.Content = "Save";
                    btnEdtAddress.Content = "Cancel";
                    btnEdtAddress.IsEnabled = true;
                    btnDelAddress.Visibility = Visibility.Hidden;

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
                if (txtName.Text == "")
                {
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

                        ctx.Banks.Add(bank);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("New Bank Added");
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
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Bank Successfully Updated");
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
                        System.Windows.Forms.MessageBox.Show("Bank successfuly deleted");
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
    }
}
