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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfClientInfo.xaml
    /// </summary>
    public partial class wpfClientInfo : Window
    {

        public string status;
        public int cId;
        string selectedFileName;
        public bool isChanged = false;

        public wpfClientInfo()
        {
            InitializeComponent();
        }

        public void reset()
        {
            //address
            btnAddAddress.Content = "Add";
            btnEdtAddress.Content = "Edit";
            dgAddress.IsEnabled = true;
            btnDelAddress.Visibility = Visibility.Visible;
            grpAddress.Visibility = Visibility.Hidden;
            txtHCity.Text = "";
            txtHProvince.Text = "";
            txtHStreet.Text = "";
            txtRentedVal.Text = "";
            rdOwned1.IsChecked = false;
            rdOwned2.IsChecked = false;
            rdRented.IsChecked = false;
            rdUsed.IsChecked = false;

            //contact
            btnAddContact.Content = "Add";
            btnEdtContact.Content = "Edit";
            dgContact.IsEnabled = true;
            btnDelContact.Visibility = Visibility.Visible;
            grpContact.Visibility = Visibility.Hidden;
            txtContact.Text = "";

            //dependent
            btnAddDep.Content = "Add";
            btnEdtDep.Content = "Edit";
            dgDependents.IsEnabled = true;
            btnDelDep.Visibility = Visibility.Visible;
            grpDependents.Visibility = Visibility.Hidden;
            txtDFName.Text = "";
            txtDLName.Text = "";
            txtDMName.Text = "";
            txtDSchool.Text = "";
            txtDSuffix.Text = "";

            //workinfo
            btnAddWork.Content = "Add";
            btnEdtWork.Content = "Edit";
            dgWork.IsEnabled = true;
            btnDelWork.Visibility = Visibility.Visible;
            grpWork.Visibility = Visibility.Hidden;
            txtWBsNumber.Text = "";
            txtWCity.Text = "";
            txtWDTI.Text = "";
            txtWIncome.Text = "";
            txtWLength.Text = "";
            txtWName.Text = "";
            txtWPLNumber.Text = "";
            txtWPosition.Text = "";
            txtWProvince.Text = "";
            txtWStreet.Text = "";

            //reference
            btnAddRef.Content = "Add";
            btnEdtRef.Content = "Edit";
            dgReference.IsEnabled = true;
            btnDelRef.Visibility = Visibility.Visible;
            grpReference.Visibility = Visibility.Hidden;
            txtRCity.Text = "";
            txtRContact.Text = "";
            txtRFName.Text = "";
            txtRLName.Text = "";
            txtRMI.Text = "";
            txtRProvince.Text = "";
            txtRStreet.Text = "";


            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            if (status == "Add")
            {
                using (var ctx = new SystemContext())
                {
                    num1 = ctx.TempHomeAddresses.Count();
                    num2 = ctx.TempClientContacts.Count();
                    num3 = ctx.TempDependents.Count();
                    num4 = ctx.TempWorks.Count();
                    num5 = ctx.TempReferences.Count();

                }
            }
            else
            {
                using (var ctx = new SystemContext())
                {
                    //num1 = ctx.EmployeeAddresses.Where(x => x.EmployeeID == uId).Count();
                    //num2 = ctx.EmployeeContacts.Where(x => x.EmployeeID == uId).Count();
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
            if (num2 > 0)
            {
                btnDelContact.IsEnabled = true;
                btnEdtContact.IsEnabled = true;
            }
            else
            {
                btnDelContact.IsEnabled = !true;
                btnEdtContact.IsEnabled = !true;
            }
            if (num3 > 0)
            {
                btnDelDep.IsEnabled = true;
                btnEdtDep.IsEnabled = true;
            }
            else
            {
                btnDelDep.IsEnabled = !true;
                btnEdtDep.IsEnabled = !true;
            }
            if (num4 > 0)
            {
                btnDelWork.IsEnabled = true;
                btnEdtWork.IsEnabled = true;
            }
            else
            {
                btnDelWork.IsEnabled = !true;
                btnEdtWork.IsEnabled = !true;
            }
            if (num5 > 0)
            {
                btnDelRef.IsEnabled = true;
                btnEdtRef.IsEnabled = true;
            }
            else
            {
                btnDelRef.IsEnabled = !true;
                btnEdtRef.IsEnabled = !true;
            }

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
                reset();
                return "";
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedFileName = dlg.FileName;
                //FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                img.Source = bitmap;
                isChanged = true;
            }
            else
            {
                isChanged = false;
            }
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)cmbStatus.SelectedItem;
            string value = typeItem.Content.ToString();
            if (value == "Married")
            {
                tbSps.Visibility = Visibility.Visible;
                grd1.Visibility = Visibility.Visible;
            }
            else
            {
                tbSps.Visibility = Visibility.Hidden;
                grd1.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show(cmbStatus.Text);
            if (cmbStatus.Text == "Married")
            {
                tbSps.Visibility = Visibility.Visible;
                grd1.Visibility = Visibility.Visible;
            }
            else
            {
                tbSps.Visibility = Visibility.Hidden;
                grd1.Visibility = Visibility.Hidden;
            }

            if (status == "Add")
            {
                using (var ctx = new SystemContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempHomeAddresses");
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempClientContacts");
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempDependents");
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempWorks");
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempReferences");
                }
            }
            reset();
        }

        private void cmbSex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            String ownership = "";
            double val = 0;
            
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
                //if (txtReqName.Text == "" || txtReqDesc.Text == "")
                //{
                //    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                //for view
                if (status == "View")
                {
                    if (rdOwned1.IsChecked == true)
                    {
                        ownership = "Owned(Not Mortgage)";
                    }
                    else if (rdOwned2.IsChecked == true)
                    {
                        ownership = "Owned(Mortgage)";
                    }
                    else if (rdUsed.IsChecked == true)
                    {
                        ownership = "Used Free";
                    }
                    else
                    {
                        ownership = "Rented";
                        val = Convert.ToDouble(txtRentedVal.Text);
                    }
                    /*using (var ctx = new SystemContext())
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    var ctr = ctx.TempHomeAddresses.Count() + 1;
                    
                    TempHomeAddress th = new TempHomeAddress { AddressNumber = ctr, City = txtHCity.Text, LengthOfStay = txtHLength.Text, MonthlyFee = val, OwnershipType = ownership, Province = txtHProvince.Text, Street = txtHStreet.Text };
                    ctx.TempHomeAddresses.Add(th);
                    ctx.SaveChanges();

                    var ads = from ad in ctx.TempHomeAddresses
                              select new { AddressNumber=ad.AddressNumber, Street = ad.Street, Province= ad.Province, City= ad.City, OwnershipType=ad.OwnershipType, MonthlyFee=ad.MonthlyFee, LengthOfStay=ad.LengthOfStay };
                    dgAddress.ItemsSource = ads.ToList();
                    reset();

                }

                reset();
            }
            else //for update
            {
                //for view
                if (status == "View")
                {
                    /*using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgReq, 0));
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
                    return;*/
                }
                if (rdOwned1.IsChecked == true)
                {
                    ownership = "Owned(Not Mortgage)";
                }
                else if (rdOwned2.IsChecked == true)
                {
                    ownership = "Owned(Mortgage)";
                }
                else if (rdUsed.IsChecked == true)
                {
                    ownership = "Used Free";
                }
                else
                {
                    ownership = "Rented";
                    val = Convert.ToDouble(txtRentedVal.Text);
                }

                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt16(getRow(dgAddress, 0));
                    var tr = ctx.TempHomeAddresses.Where(x => x.AddressNumber == num).First();
                    tr.City = txtHCity.Text;
                    tr.LengthOfStay = txtHLength.Text;
                    tr.MonthlyFee = val;
                    tr.OwnershipType = ownership;
                    tr.Province = txtHProvince.Text;
                    tr.Street = txtHStreet.Text;
                    ctx.SaveChanges();
                    var ads = from ad in ctx.TempHomeAddresses
                              select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                    dgAddress.ItemsSource = ads.ToList();
                    reset();
                }
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
                        /*using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt16(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text = tr.Name;
                            txtReqDesc.Text = tr.Description;
                        }

                        return;
                        */
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgAddress, 0));
                        var tr = ctx.TempHomeAddresses.Where(x => x.AddressNumber == num).First();
                        txtHCity.Text=tr.City;
                        txtHLength.Text = tr.LengthOfStay;
                        txtRentedVal.Text= tr.MonthlyFee.ToString();
                        txtHProvince.Text = tr.Province;
                        txtHStreet.Text = tr.Street;
                        if (tr.OwnershipType == "Owned(Not Mortgage)")
                        {
                            rdOwned1.IsChecked = true;
                        }
                        else if (tr.OwnershipType == "Owned(Mortgage)")
                        {
                            rdOwned2.IsChecked = true;
                        }
                        else if (tr.OwnershipType == "Used Free")
                        {
                            rdUsed.IsChecked = true;
                        }
                        else
                        { 
                            rdRented.IsChecked=true;
                        }
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

        private void btnDelAddress_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        /*int nums = Convert.ToInt16(getRow(dgReq, 0));
                        var tr1 = ctx.Requirements.Where(x => x.RequirementNum == nums && x.ServiceID == sId).First();
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
                        var reqs2 = from rq in ctx.Requirements
                                    where rq.ServiceID == sId
                                    select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs2.ToList();
                        return;
                         */
                    }

                    int num = Convert.ToInt16(getRow(dgAddress, 0));
                    var th = ctx.TempHomeAddresses.Where(x => x.AddressNumber == num).First();
                    ctx.TempHomeAddresses.Remove(th);
                    ctx.SaveChanges();

                    var adds = from add in ctx.TempHomeAddresses
                               select add;
                    int ctr = 1;
                    foreach (var item in adds)
                    {
                        item.AddressNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var ads = from ad in ctx.TempHomeAddresses
                              select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                    dgAddress.ItemsSource = ads.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddContact.Content.ToString() == "Add")
            {
                grpContact.Visibility = Visibility.Visible;
                btnAddContact.Content = "Save";
                btnEdtContact.Content = "Cancel";
                btnEdtContact.IsEnabled = true;
                btnDelContact.Visibility = Visibility.Hidden;

            }
            else if (btnAddContact.Content.ToString() == "Save")
            {
                //if (txtReqName.Text == "" || txtReqDesc.Text == "")
                //{
                //    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                //for view
                if (status == "View")
                {
                    
                    /*using (var ctx = new SystemContext())
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    var ctr = ctx.TempClientContacts.Count() + 1;
                    bool pr=false;
                    if(ctr==1)
                    {
                        pr=true;
                    }

                    TempClientContact tc = new TempClientContact { ContactNumber = ctr, Contact = txtContact.Text, Primary = pr };
                    ctx.TempClientContacts.Add(tc);
                    ctx.SaveChanges();

                    var cts = from ct in ctx.TempClientContacts
                              select new { ContactNumber = ct.ContactNumber, Contact= ct.Contact , Primary= ct.Primary };
                    dgContact.ItemsSource = cts.ToList();
                    reset();

                }

                reset();
            }
            else //for update
            {
                //for view
                if (status == "View")
                {
                    /*using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgReq, 0));
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
                    return;*/
                }
                

                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt16(getRow(dgContact, 0));
                    var tc = ctx.TempClientContacts.Where(x => x.ContactNumber == num).First();
                    tc.Contact = txtContact.Text;
                    ctx.SaveChanges();
                    var cts = from ct in ctx.TempClientContacts
                              select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                    dgContact.ItemsSource = cts.ToList();
                    reset();
                }
            }
        }

        private void btnEdtContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtContact.Content.ToString() == "Edit")
                {
                    btnEdtContact.Content = "Cancel";
                    btnAddContact.Content = "Update";
                    dgContact.IsEnabled = false;
                    btnDelContact.Visibility = Visibility.Hidden;
                    grpContact.Visibility = Visibility.Visible;

                    //for view
                    if (status == "View")
                    {
                        /*using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt16(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text = tr.Name;
                            txtReqDesc.Text = tr.Description;
                        }

                        return;
                        */
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgContact, 0));
                        var tc = ctx.TempClientContacts.Where(x => x.ContactNumber == num).First();
                        txtContact.Text = tc.Contact;
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

        private void btnDelContact_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        /*int nums = Convert.ToInt16(getRow(dgReq, 0));
                        var tr1 = ctx.Requirements.Where(x => x.RequirementNum == nums && x.ServiceID == sId).First();
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
                        var reqs2 = from rq in ctx.Requirements
                                    where rq.ServiceID == sId
                                    select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs2.ToList();
                        return;
                         */
                    }

                    int num = Convert.ToInt16(getRow(dgContact, 0));
                    var tc = ctx.TempClientContacts.Where(x => x.ContactNumber == num).First();
                    ctx.TempClientContacts.Remove(tc);
                    ctx.SaveChanges();

                    var cts = from ct in ctx.TempClientContacts
                               select ct;
                    int ctr = 1;
                    foreach (var item in cts)
                    {
                        item.ContactNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var cts1 = from ct in ctx.TempClientContacts
                              select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                    dgContact.ItemsSource = cts1.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void btnAddDep_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddDep.Content.ToString() == "Add")
            {
                grpDependents.Visibility = Visibility.Visible;
                btnAddDep.Content = "Save";
                btnEdtDep.Content = "Cancel";
                btnEdtDep.IsEnabled = true;
                btnDelDep.Visibility = Visibility.Hidden;

            }
            else if (btnAddDep.Content.ToString() == "Save")
            {
                //if (txtReqName.Text == "" || txtReqDesc.Text == "")
                //{
                //    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                //for view
                if (status == "View")
                {

                    /*using (var ctx = new SystemContext())
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    var ctr = ctx.TempDependents.Count() + 1;

                    TempDependent tdp = new TempDependent { Birthday = Convert.ToDateTime(dtDBDay.SelectedDate), DependentNumber = ctr, FirstName = txtDFName.Text, LastName = txtDLName.Text, MiddleName = txtDMName.Text, School = txtDSchool.Text, Suffix = txtDSuffix.Text };
                    ctx.TempDependents.Add(tdp);
                    ctx.SaveChanges();

                    var dps = from td in ctx.TempDependents
                              select new { DependentNumber=td.DependentNumber, LastName=td.LastName, FirstName= td.FirstName, MiddleName= td.MiddleName, Suffix= td.Suffix, Birthday=td.Birthday, School=td.School };
                    dgDependents.ItemsSource = dps.ToList();
                    reset();

                }

                reset();
            }
            else //for update
            {
                //for view
                if (status == "View")
                {
                    /*using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgReq, 0));
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt16(getRow(dgDependents, 0));
                    var td = ctx.TempDependents.Where(x => x.DependentNumber == num).First();
                    td.Birthday = Convert.ToDateTime(dtDBDay.SelectedDate);
                    td.FirstName = txtDFName.Text;
                    td.LastName = txtDLName.Text;
                    td.MiddleName = txtDMName.Text;
                    td.School = txtDSchool.Text;
                    td.Suffix = txtDSuffix.Text;
                    ctx.SaveChanges();
                    var dps = from tdp in ctx.TempDependents
                              select new { DependentNumber = tdp.DependentNumber, LastName = tdp.LastName, FirstName = tdp.FirstName, MiddleName = tdp.MiddleName, Suffix = tdp.Suffix, Birthday = tdp.Birthday, School = tdp.School };
                    dgDependents.ItemsSource = dps.ToList();
                    reset();
                }
            }
        }

        private void btnEdtDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtDep.Content.ToString() == "Edit")
                {
                    btnEdtDep.Content = "Cancel";
                    btnAddDep.Content = "Update";
                    dgDependents.IsEnabled = false;
                    btnDelDep.Visibility = Visibility.Hidden;
                    grpDependents.Visibility = Visibility.Visible;

                    //for view
                    if (status == "View")
                    {
                        /*using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt16(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text = tr.Name;
                            txtReqDesc.Text = tr.Description;
                        }

                        return;
                        */
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgDependents, 0));
                        var td = ctx.TempDependents.Where(x => x.DependentNumber == num).First();
                        dtDBDay.SelectedDate = td.Birthday;
                        txtDFName.Text=td.FirstName;
                        txtDLName.Text = td.LastName;
                        txtDMName.Text = td.MiddleName;
                        txtDSchool.Text = td.School;
                        txtDSuffix.Text = td.Suffix;
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

        private void btnDelDep_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        /*int nums = Convert.ToInt16(getRow(dgReq, 0));
                        var tr1 = ctx.Requirements.Where(x => x.RequirementNum == nums && x.ServiceID == sId).First();
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
                        var reqs2 = from rq in ctx.Requirements
                                    where rq.ServiceID == sId
                                    select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs2.ToList();
                        return;
                         */
                    }

                    int num = Convert.ToInt16(getRow(dgDependents, 0));
                    var td = ctx.TempDependents.Where(x => x.DependentNumber == num).First();
                    ctx.TempDependents.Remove(td);
                    ctx.SaveChanges();

                    var dps = from dp in ctx.TempDependents
                              select dp;
                    int ctr = 1;
                    foreach (var item in dps)
                    {
                        item.DependentNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var dps1 = from tdp in ctx.TempDependents
                              select new { DependentNumber = tdp.DependentNumber, LastName = tdp.LastName, FirstName = tdp.FirstName, MiddleName = tdp.MiddleName, Suffix = tdp.Suffix, Birthday = tdp.Birthday, School = tdp.School };
                    dgDependents.ItemsSource = dps1.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void btnAddWork_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddWork.Content.ToString() == "Add")
            {
                grpWork.Visibility = Visibility.Visible;
                btnAddWork.Content = "Save";
                btnEdtWork.Content = "Cancel";
                btnEdtWork.IsEnabled = true;
                btnDelWork.Visibility = Visibility.Hidden;

            }
            else if (btnAddWork.Content.ToString() == "Save")
            {
                //if (txtReqName.Text == "" || txtReqDesc.Text == "")
                //{
                //    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                //for view
                if (status == "View")
                {

                    /*using (var ctx = new SystemContext())
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    var ctr = ctx.TempWorks.Count() + 1;

                    TempWork tw = new TempWork { WorkNumber = ctr, BusinessNumber = txtWBsNumber.Text, BusinessName = txtWName.Text, City = txtWCity.Text, DTI = txtWDTI.Text, Employment = cmbWEmployment.Text, LengthOfStay = txtWLength.Text, MonthlyIncome = Convert.ToDouble(txtWIncome.Text), PLNumber = txtWPLNumber.Text, Position = txtWPosition.Text, Province = txtWProvince.Text, status = cmbWStatus.Text, Street = txtWStreet.Text };
                    ctx.TempWorks.Add(tw);
                    ctx.SaveChanges();

                    var wrk = from wr in ctx.TempWorks
                              select new { WorkNumber=wr.WorkNumber, BusinessName = wr.BusinessName, DTI= wr.DTI, Street= wr.Street, Province= wr.Province, City=wr.City, Employment= wr.Employment, LengthOfStay=wr.LengthOfStay, BusinessNumber=wr.BusinessNumber, Position= wr.Position, MonthlyIncome=wr.MonthlyIncome, PLNumber=wr.PLNumber, Status=wr.status };
                    dgWork.ItemsSource = wrk.ToList();
                    reset();

                }

                reset();
            }
            else //for update
            {
                //for view
                if (status == "View")
                {
                    /*using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgReq, 0));
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt16(getRow(dgWork, 0));
                    var tw = ctx.TempWorks.Where(x => x.WorkNumber == num).First();
                    tw.BusinessNumber = txtWBsNumber.Text;
                    tw.BusinessName = txtWName.Text;
                    tw.City = txtWCity.Text;
                    tw.DTI = txtWDTI.Text;
                    tw.Employment = cmbWEmployment.Text;
                    tw.LengthOfStay = txtWLength.Text;
                    tw.MonthlyIncome = Convert.ToDouble(txtWIncome.Text);
                    tw.PLNumber = txtWPLNumber.Text;
                    tw.Position = txtWPosition.Text;
                    tw.Province = txtWProvince.Text;
                    tw.status = cmbWStatus.Text;
                    tw.Street = txtWStreet.Text; 
                    ctx.SaveChanges();
                    var wrk = from wr in ctx.TempWorks
                              select new { WorkNumber=wr.WorkNumber, BusinessName = wr.BusinessName, DTI= wr.DTI, Street= wr.Street, Province= wr.Province, City=wr.City, Employment= wr.Employment, LengthOfStay=wr.LengthOfStay, BusinessNumber=wr.BusinessNumber, Position= wr.Position, MonthlyIncome=wr.MonthlyIncome, PLNumber=wr.PLNumber, Status=wr.status };
                    dgWork.ItemsSource = wrk.ToList();
                    reset();
                }
            }
        }

        private void btnEdtWork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtWork.Content.ToString() == "Edit")
                {
                    btnEdtWork.Content = "Cancel";
                    btnAddWork.Content = "Update";
                    dgWork.IsEnabled = false;
                    btnDelWork.Visibility = Visibility.Hidden;
                    grpWork.Visibility = Visibility.Visible;

                    //for view
                    if (status == "View")
                    {
                        /*using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt16(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text = tr.Name;
                            txtReqDesc.Text = tr.Description;
                        }

                        return;
                        */
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgWork, 0));
                        var tw = ctx.TempWorks.Where(x => x.WorkNumber == num).First();
                        txtWBsNumber.Text=tw.BusinessNumber;
                        txtWName.Text = tw.BusinessName;
                        txtWCity.Text = tw.City;
                        txtWDTI.Text = tw.DTI;
                        cmbWEmployment.Text = tw.Employment;
                        txtWLength.Text = tw.LengthOfStay;
                        txtWIncome.Text = tw.MonthlyIncome.ToString();
                        txtWPLNumber.Text = tw.PLNumber;
                        txtWPosition.Text = tw.Position;
                        txtWProvince.Text = tw.Province;
                        cmbWStatus.Text = tw.status;
                        txtWStreet.Text = tw.Street; 
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

        private void btnDelWork_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        /*int nums = Convert.ToInt16(getRow(dgReq, 0));
                        var tr1 = ctx.Requirements.Where(x => x.RequirementNum == nums && x.ServiceID == sId).First();
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
                        var reqs2 = from rq in ctx.Requirements
                                    where rq.ServiceID == sId
                                    select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs2.ToList();
                        return;
                         */
                    }

                    int num = Convert.ToInt16(getRow(dgWork, 0));
                    var tw = ctx.TempWorks.Where(x => x.WorkNumber == num).First();
                    ctx.TempWorks.Remove(tw);
                    ctx.SaveChanges();

                    var wrks = from wrk in ctx.TempWorks
                              select wrk;
                    int ctr = 1;
                    foreach (var item in wrks)
                    {
                        item.WorkNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();

                    var wrk1 = from wr in ctx.TempWorks
                              select new { WorkNumber = wr.WorkNumber, BusinessName = wr.BusinessName, DTI = wr.DTI, Street = wr.Street, Province = wr.Province, City = wr.City, Employment = wr.Employment, LengthOfStay = wr.LengthOfStay, BusinessNumber = wr.BusinessNumber, Position = wr.Position, MonthlyIncome = wr.MonthlyIncome, PLNumber = wr.PLNumber, Status = wr.status };
                    dgWork.ItemsSource = wrk1.ToList();

                    reset();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void btnAddRef_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddRef.Content.ToString() == "Add")
            {
                grpReference.Visibility = Visibility.Visible;
                btnAddRef.Content = "Save";
                btnEdtRef.Content = "Cancel";
                btnEdtRef.IsEnabled = true;
                btnDelRef.Visibility = Visibility.Hidden;

            }
            else if (btnAddRef.Content.ToString() == "Save")
            {
                //if (txtReqName.Text == "" || txtReqDesc.Text == "")
                //{
                //    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                //for view
                if (status == "View")
                {

                    /*using (var ctx = new SystemContext())
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    var ctr = ctx.TempReferences.Count() + 1;

                    TempReference tr = new TempReference { ReferenceNumber = ctr, City = txtRCity.Text, Contact = txtRContact.Text, FirstName = txtRFName.Text, LastName = txtRLName.Text, MiddleName = txtRMI.Text, Province = txtRProvince.Text, Street = txtRStreet.Text, Suffix = txtRSuffix.Text };
                    ctx.TempReferences.Add(tr);
                    ctx.SaveChanges();

                    var rfs = from rf in ctx.TempReferences
                              select new { ReferenceNumber= rf.ReferenceNumber, LastName = rf.LastName, FirstName=rf.FirstName, MI=rf.MiddleName, Suffix=rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                    dgReference.ItemsSource = rfs.ToList();
                    reset();

                }

                reset();
            }
            else //for update
            {
                //for view
                if (status == "View")
                {
                    /*using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgReq, 0));
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
                    return;*/
                }


                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt16(getRow(dgReference, 0));
                    var tr = ctx.TempReferences.Where(x => x.ReferenceNumber == num).First();
                    tr.City=txtRCity.Text;
                    tr.Contact=txtRContact.Text;
                    tr.FirstName=txtRFName.Text;
                    tr.LastName=txtRLName.Text;
                    tr.MiddleName=txtRMI.Text;
                    tr.Province=txtRProvince.Text;
                    tr.Street=txtRStreet.Text;
                    tr.Suffix = txtRSuffix.Text;
                    ctx.SaveChanges();
                    var rfs = from rf in ctx.TempReferences
                              select new { ReferenceNumber= rf.ReferenceNumber, LastName = rf.LastName, FirstName=rf.FirstName, MI=rf.MiddleName, Suffix=rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                    dgReference.ItemsSource = rfs.ToList();
                    reset();
                }
            }
        }

        private void btnEdtRef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEdtRef.Content.ToString() == "Edit")
                {
                    btnEdtRef.Content = "Cancel";
                    btnAddRef.Content = "Update";
                    dgReference.IsEnabled = false;
                    btnDelRef.Visibility = Visibility.Hidden;
                    grpReference.Visibility = Visibility.Visible;

                    //for view
                    if (status == "View")
                    {
                        /*using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt16(getRow(dgReq, 0));
                            var tr = ctx.Requirements.Where(x => x.RequirementNum == num && x.ServiceID == sId).First();
                            txtReqName.Text = tr.Name;
                            txtReqDesc.Text = tr.Description;
                        }

                        return;
                        */
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt16(getRow(dgReference, 0));
                        var tr = ctx.TempReferences.Where(x => x.ReferenceNumber == num).First();
                        txtRCity.Text = tr.City;
                        txtRContact.Text = tr.Contact;
                        txtRFName.Text = tr.FirstName;
                        txtRLName.Text = tr.LastName;
                        txtRMI.Text = tr.MiddleName;
                        txtRProvince.Text = tr.Province;
                        txtRStreet.Text = tr.Street;
                        txtRSuffix.Text = tr.Suffix;
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

        private void btnDelRef_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        /*int nums = Convert.ToInt16(getRow(dgReq, 0));
                        var tr1 = ctx.Requirements.Where(x => x.RequirementNum == nums && x.ServiceID == sId).First();
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
                        var reqs2 = from rq in ctx.Requirements
                                    where rq.ServiceID == sId
                                    select new { ReqNumber = rq.RequirementNum, Name = rq.Name, Description = rq.Description };
                        dgReq.ItemsSource = reqs2.ToList();
                        return;
                         */
                    }

                    int num = Convert.ToInt16(getRow(dgReference, 0));
                    var tr = ctx.TempReferences.Where(x => x.ReferenceNumber == num).First();
                    ctx.TempReferences.Remove(tr);
                    ctx.SaveChanges();

                    var rfs = from rf in ctx.TempReferences
                               select rf;
                    int ctr = 1;
                    foreach (var item in rfs)
                    {
                        item.ReferenceNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();

                    var rfs1 = from rf in ctx.TempReferences
                              select new { ReferenceNumber = rf.ReferenceNumber, LastName = rf.LastName, FirstName = rf.FirstName, MI = rf.MiddleName, Suffix = rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                    dgReference.ItemsSource = rfs1.ToList();

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
