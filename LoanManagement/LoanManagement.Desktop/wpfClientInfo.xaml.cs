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
    /// Interaction logic for wpfClientInfo.xaml
    /// </summary>
    public partial class wpfClientInfo : MetroWindow
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
                    num1 = ctx.HomeAddresses.Where(x => x.ClientID == cId).Count();
                    num2 = ctx.ClientContacts.Where(x => x.ClientID == cId).Count();
                    num3 = ctx.Dependents.Where(x => x.ClientID == cId).Count();
                    num4 = ctx.Works.Where(x => x.ClientID == cId).Count();
                    num5 = ctx.References.Where(x => x.ClientID == cId).Count();
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
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;
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

                selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                bitmap.EndInit();
                img.Source = bitmap;
            }
            else
            {
                using (var ctx = new SystemContext())
                {
                    var clt = ctx.Clients.Find(cId);
                    txtLName.Text = clt.LastName;
                    txtFName.Text = clt.FirstName;
                    txtMName.Text = clt.MiddleName;
                    txtSuffix.Text = clt.Suffix;
                    cmbSex.Text = clt.Sex;
                    txtSSS.Text = clt.SSS;
                    txtTIN.Text = clt.TIN;
                    txtEmail.Text = clt.Email;
                    dtBDay.SelectedDate = clt.Birthday;
                    cmbStatus.Text = clt.Status;

                    if (clt.Status == "Married")
                    {
                        var sps = ctx.Spouses.Where(x => x.ClientID == cId).First();
                        dtSBday.SelectedDate = sps.Birthday; 
                        txtSWName.Text=sps.BusinessName; 
                        txtSBsNumber.Text = sps.BusinessNumber; 
                        txtSCity.Text = sps.City; 
                        txtSDTI.Text = sps.DTI; 
                        cmbSEmployment.Text = sps.Employment; 
                        txtSFName.Text = sps.FirstName; 
                        txtSLName.Text = sps.LastName; 
                        txtSLength.Text = sps.LengthOfStay; 
                        txtSMName.Text = sps.MiddleName; 
                        txtSIncome.Text = sps.MonthlyIncome.ToString(); 
                        txtSPLNumber.Text = sps.PLNumber; 
                        txtSPosition.Text = sps.Position; 
                        txtSProvince.Text = sps.Province; 
                        cmbSStatus.Text = sps.status; 
                        txtSStreet.Text = sps.Street;
                        txtSuffix.Text = sps.Suffix;
                    }

                    byte[] imageArr;
                    imageArr = clt.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;

                    var ads = from ad in ctx.HomeAddresses
                              where ad.ClientID==cId
                              select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                    dgAddress.ItemsSource = ads.ToList();

                    var cts = from ct in ctx.ClientContacts
                              where ct.ClientID==cId
                              select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                    dgContact.ItemsSource = cts.ToList();

                    var dps = from td in ctx.Dependents
                              where td.ClientID==cId
                              select new { DependentNumber = td.DependentNumber, LastName = td.LastName, FirstName = td.FirstName, MiddleName = td.MiddleName, Suffix = td.Suffix, Birthday = td.Birthday, School = td.School };
                    dgDependents.ItemsSource = dps.ToList();

                    var wrk = from wr in ctx.Works
                              where wr.ClientID==cId
                              select new { WorkNumber = wr.WorkNumber, BusinessName = wr.BusinessName, DTI = wr.DTI, Street = wr.Street, Province = wr.Province, City = wr.City, Employment = wr.Employment, LengthOfStay = wr.LengthOfStay, BusinessNumber = wr.BusinessNumber, Position = wr.Position, MonthlyIncome = wr.MonthlyIncome, PLNumber = wr.PLNumber, Status = wr.status };
                    dgWork.ItemsSource = wrk.ToList();

                    var rfs = from rf in ctx.References
                              where rf.ClientID==cId
                              select new { ReferenceNumber = rf.ReferenceNumber, LastName = rf.LastName, FirstName = rf.FirstName, MI = rf.MiddleName, Suffix = rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                    dgReference.ItemsSource = rfs.ToList();
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
                    using (var ctx = new SystemContext())
                    {
                        var ctr = ctx.HomeAddresses.Where(x=> x.ClientID==cId).Count() + 1;

                        HomeAddress th = new HomeAddress {ClientID=cId, AddressNumber = ctr, City = txtHCity.Text, LengthOfStay = txtHLength.Text, MonthlyFee = val, OwnershipType = ownership, Province = txtHProvince.Text, Street = txtHStreet.Text };
                        ctx.HomeAddresses.Add(th);
                        ctx.SaveChanges();

                        var ads = from ad in ctx.HomeAddresses
                                  where ad.ClientID==cId
                                  select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                        dgAddress.ItemsSource = ads.ToList();
                    }
                    reset();
                    return;
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
                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgAddress, 0));
                        var tr = ctx.HomeAddresses.Where(x => x.AddressNumber == num && x.ClientID==cId).First();
                        tr.City = txtHCity.Text;
                        tr.LengthOfStay = txtHLength.Text;
                        tr.MonthlyFee = val;
                        tr.OwnershipType = ownership;
                        tr.Province = txtHProvince.Text;
                        tr.Street = txtHStreet.Text;
                        ctx.SaveChanges();
                        var ads = from ad in ctx.HomeAddresses
                                  where ad.ClientID == cId
                                  select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                        dgAddress.ItemsSource = ads.ToList();
                        reset();
                    }
                    return;
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
                    int num = Convert.ToInt32(getRow(dgAddress, 0));
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
                        using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt32(getRow(dgAddress, 0));
                            var tr = ctx.HomeAddresses.Where(x => x.AddressNumber == num && x.ClientID==cId).First();
                            txtHCity.Text = tr.City;
                            txtHLength.Text = tr.LengthOfStay;
                            txtRentedVal.Text = tr.MonthlyFee.ToString();
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
                                rdRented.IsChecked = true;
                            }
                        }

                        return;
                        
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgAddress, 0));
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
                        int num = Convert.ToInt32(getRow(dgAddress, 0));
                        var th = ctx.HomeAddresses.Where(x => x.AddressNumber == num && x.ClientID==cId).First();
                        ctx.HomeAddresses.Remove(th);
                        ctx.SaveChanges();

                        var adds = from add in ctx.HomeAddresses
                                   where add.ClientID==cId
                                   select add;
                        int ctr = 1;
                        foreach (var item in adds)
                        {
                            item.AddressNumber = ctr;
                            ctr++;
                        }
                        ctx.SaveChanges();
                        var ads = from ad in ctx.HomeAddresses
                                  where ad.ClientID == cId
                                  select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                        dgAddress.ItemsSource = ads.ToList();
                        return;
                         
                    }

                    int num1 = Convert.ToInt32(getRow(dgAddress, 0));
                    var th1 = ctx.TempHomeAddresses.Where(x => x.AddressNumber == num1).First();
                    ctx.TempHomeAddresses.Remove(th1);
                    ctx.SaveChanges();

                    var adds1 = from add in ctx.TempHomeAddresses
                               select add;
                    int ctr1 = 1;
                    foreach (var item in adds1)
                    {
                        item.AddressNumber = ctr1;
                        ctr1++;
                    }
                    ctx.SaveChanges();
                    var ads1 = from ad in ctx.TempHomeAddresses
                              select new { AddressNumber = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                    dgAddress.ItemsSource = ads1.ToList();
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

                    using (var ctx = new SystemContext())
                    {
                        var ctr = ctx.ClientContacts.Where(x=> x.ClientID==cId).Count() + 1;
                        bool pr = false;
                        if (ctr == 1)
                        {
                            pr = true;
                        }

                        ClientContact tc = new ClientContact {ClientID=cId, ContactNumber = ctr, Contact = txtContact.Text, Primary = pr };
                        ctx.ClientContacts.Add(tc);
                        ctx.SaveChanges();

                        var cts = from ct in ctx.ClientContacts
                                  where ct.ClientID==cId
                                  select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                        dgContact.ItemsSource = cts.ToList();
                        reset();

                    }
                    return;
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
                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgContact, 0));
                        var tc = ctx.ClientContacts.Where(x => x.ContactNumber == num && x.ClientID==cId).First();
                        tc.Contact = txtContact.Text;
                        ctx.SaveChanges();
                        var cts = from ct in ctx.ClientContacts
                                  where ct.ClientID == cId
                                  select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                        dgContact.ItemsSource = cts.ToList();
                        reset();
                    }
                    return;
                }
                

                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt32(getRow(dgContact, 0));
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
                        using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt32(getRow(dgContact, 0));
                            var tc = ctx.ClientContacts.Where(x => x.ContactNumber == num && x.ClientID==cId).First();
                            txtContact.Text = tc.Contact;
                        }

                        return;
                        
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgContact, 0));
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
                        int num1 = Convert.ToInt32(getRow(dgContact, 0));
                        var tc1 = ctx.ClientContacts.Where(x => x.ContactNumber == num1 && x.ClientID==cId).First();
                        ctx.ClientContacts.Remove(tc1);
                        ctx.SaveChanges();

                        var cts1 = from ct in ctx.ClientContacts
                                   where ct.ClientID==cId
                                  select ct;
                        int ctr1 = 1;
                        foreach (var item in cts1)
                        {
                            item.ContactNumber = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();
                        var cts2 = from ct in ctx.ClientContacts
                                   where ct.ClientID == cId
                                   select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                        dgContact.ItemsSource = cts2.ToList();
                        reset();
                        return;
                         
                    }

                    int num = Convert.ToInt32(getRow(dgContact, 0));
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
                    var cts4 = from ct in ctx.TempClientContacts
                              select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                    dgContact.ItemsSource = cts4.ToList();
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

                    using (var ctx = new SystemContext())
                    {
                        var ctr = ctx.Dependents.Where(x=> x.ClientID==cId).Count() + 1;

                        Dependent tdp = new Dependent {ClientID=cId, Birthday = Convert.ToDateTime(dtDBDay.SelectedDate).Date, DependentNumber = ctr, FirstName = txtDFName.Text, LastName = txtDLName.Text, MiddleName = txtDMName.Text, School = txtDSchool.Text, Suffix = txtDSuffix.Text };
                        ctx.Dependents.Add(tdp);
                        ctx.SaveChanges();

                        var dps = from td in ctx.Dependents
                                  where td.ClientID == cId
                                  select new { DependentNumber = td.DependentNumber, LastName = td.LastName, FirstName = td.FirstName, MiddleName = td.MiddleName, Suffix = td.Suffix, Birthday = td.Birthday, School = td.School };
                        dgDependents.ItemsSource = dps.ToList();
                        reset();

                    }
                    return;
                }


                using (var ctx = new SystemContext())
                {
                    var ctr = ctx.TempDependents.Count() + 1;

                    TempDependent tdp = new TempDependent { Birthday = Convert.ToDateTime(dtDBDay.SelectedDate).Date, DependentNumber = ctr, FirstName = txtDFName.Text, LastName = txtDLName.Text, MiddleName = txtDMName.Text, School = txtDSchool.Text, Suffix = txtDSuffix.Text };
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
                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgDependents, 0));
                        var td = ctx.Dependents.Where(x => x.DependentNumber == num && x.ClientID==cId).First();
                        td.Birthday = Convert.ToDateTime(dtDBDay.SelectedDate).Date;
                        td.FirstName = txtDFName.Text;
                        td.LastName = txtDLName.Text;
                        td.MiddleName = txtDMName.Text;
                        td.School = txtDSchool.Text;
                        td.Suffix = txtDSuffix.Text;
                        ctx.SaveChanges();
                        var dps = from tdp in ctx.Dependents
                                  where tdp.ClientID == cId
                                  select new { DependentNumber = tdp.DependentNumber, LastName = tdp.LastName, FirstName = tdp.FirstName, MiddleName = tdp.MiddleName, Suffix = tdp.Suffix, Birthday = tdp.Birthday, School = tdp.School };
                        dgDependents.ItemsSource = dps.ToList();
                        reset();
                    }
                    return;
                }


                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt32(getRow(dgDependents, 0));
                    var td = ctx.TempDependents.Where(x => x.DependentNumber == num).First();
                    td.Birthday = Convert.ToDateTime(dtDBDay.SelectedDate).Date;
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
                        using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt32(getRow(dgDependents, 0));
                            var td = ctx.Dependents.Where(x => x.DependentNumber == num && x.ClientID==cId).First();
                            dtDBDay.SelectedDate = td.Birthday;
                            txtDFName.Text = td.FirstName;
                            txtDLName.Text = td.LastName;
                            txtDMName.Text = td.MiddleName;
                            txtDSchool.Text = td.School;
                            txtDSuffix.Text = td.Suffix;
                        }

                        return;
                        
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgDependents, 0));
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
                        int num1 = Convert.ToInt32(getRow(dgDependents, 0));
                        var td1 = ctx.Dependents.Where(x => x.DependentNumber == num1 && x.ClientID==cId).First();
                        ctx.Dependents.Remove(td1);
                        ctx.SaveChanges();

                        var dps1 = from dp in ctx.Dependents
                                   where dp.ClientID==cId
                                  select dp;
                        int ctr1 = 1;
                        foreach (var item in dps1)
                        {
                            item.DependentNumber = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();
                        var dps2 = from tdp in ctx.Dependents
                                   where tdp.ClientID == cId
                                   select new { DependentNumber = tdp.DependentNumber, LastName = tdp.LastName, FirstName = tdp.FirstName, MiddleName = tdp.MiddleName, Suffix = tdp.Suffix, Birthday = tdp.Birthday, School = tdp.School };
                        dgDependents.ItemsSource = dps2.ToList();
                        reset();
                        return;
                         
                    }

                    int num = Convert.ToInt32(getRow(dgDependents, 0));
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
                    var dps4 = from tdp in ctx.TempDependents
                              select new { DependentNumber = tdp.DependentNumber, LastName = tdp.LastName, FirstName = tdp.FirstName, MiddleName = tdp.MiddleName, Suffix = tdp.Suffix, Birthday = tdp.Birthday, School = tdp.School };
                    dgDependents.ItemsSource = dps4.ToList();
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

                    using (var ctx = new SystemContext())
                    {
                        var ctr = ctx.Works.Where(x=> x.ClientID==cId).Count() + 1;

                        Work tw = new Work {ClientID=cId, WorkNumber = ctr, BusinessNumber = txtWBsNumber.Text, BusinessName = txtWName.Text, City = txtWCity.Text, DTI = txtWDTI.Text, Employment = cmbWEmployment.Text, LengthOfStay = txtWLength.Text, MonthlyIncome = Convert.ToDouble(txtWIncome.Text), PLNumber = txtWPLNumber.Text, Position = txtWPosition.Text, Province = txtWProvince.Text, status = cmbWStatus.Text, Street = txtWStreet.Text };
                        ctx.Works.Add(tw);
                        ctx.SaveChanges();

                        var wrk = from wr in ctx.Works
                                  where wr.ClientID == cId
                                  select new { WorkNumber = wr.WorkNumber, BusinessName = wr.BusinessName, DTI = wr.DTI, Street = wr.Street, Province = wr.Province, City = wr.City, Employment = wr.Employment, LengthOfStay = wr.LengthOfStay, BusinessNumber = wr.BusinessNumber, Position = wr.Position, MonthlyIncome = wr.MonthlyIncome, PLNumber = wr.PLNumber, Status = wr.status };
                        dgWork.ItemsSource = wrk.ToList();
                        reset();

                    }
                    return;
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
                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgWork, 0));
                        var tw = ctx.Works.Where(x => x.WorkNumber == num && x.ClientID==cId).First();
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
                        var wrk = from wr in ctx.Works
                                  where wr.ClientID == cId
                                  select new { WorkNumber=wr.WorkNumber, BusinessName = wr.BusinessName, DTI= wr.DTI, Street= wr.Street, Province= wr.Province, City=wr.City, Employment= wr.Employment, LengthOfStay=wr.LengthOfStay, BusinessNumber=wr.BusinessNumber, Position= wr.Position, MonthlyIncome=wr.MonthlyIncome, PLNumber=wr.PLNumber, Status=wr.status };
                        dgWork.ItemsSource = wrk.ToList();
                        reset();
                    }
                    return;
                }


                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt32(getRow(dgWork, 0));
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
                        using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt32(getRow(dgWork, 0));
                            var tw = ctx.Works.Where(x => x.WorkNumber == num && x.ClientID==cId).First();
                            txtWBsNumber.Text = tw.BusinessNumber;
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

                        return;
                        
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgWork, 0));
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
                        int num1 = Convert.ToInt32(getRow(dgWork, 0));
                        var tw1 = ctx.Works.Where(x => x.WorkNumber == num1 && x.ClientID==cId).First();
                        ctx.Works.Remove(tw1);
                        ctx.SaveChanges();

                        var wrks1 = from wrk in ctx.Works
                                   where wrk.ClientID==cId
                                   select wrk;
                        int ctr1 = 1;
                        foreach (var item in wrks1)
                        {
                            item.WorkNumber = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();

                        var wrk2 = from wr in ctx.Works
                                   where wr.ClientID == cId
                                   select new { WorkNumber = wr.WorkNumber, BusinessName = wr.BusinessName, DTI = wr.DTI, Street = wr.Street, Province = wr.Province, City = wr.City, Employment = wr.Employment, LengthOfStay = wr.LengthOfStay, BusinessNumber = wr.BusinessNumber, Position = wr.Position, MonthlyIncome = wr.MonthlyIncome, PLNumber = wr.PLNumber, Status = wr.status };
                        dgWork.ItemsSource = wrk2.ToList();

                        reset();
                        
                        return;
                         
                    }

                    int num = Convert.ToInt32(getRow(dgWork, 0));
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
                    using (var ctx = new SystemContext())
                    {
                        var ctr = ctx.References.Where(x=> x.ClientID==cId).Count() + 1;

                        Reference tr = new Reference {ClientID=cId, ReferenceNumber = ctr, City = txtRCity.Text, Contact = txtRContact.Text, FirstName = txtRFName.Text, LastName = txtRLName.Text, MiddleName = txtRMI.Text, Province = txtRProvince.Text, Street = txtRStreet.Text, Suffix = txtRSuffix.Text };
                        ctx.References.Add(tr);
                        ctx.SaveChanges();

                        var rfs = from rf in ctx.References
                                  where rf.ClientID == cId
                                  select new { ReferenceNumber = rf.ReferenceNumber, LastName = rf.LastName, FirstName = rf.FirstName, MI = rf.MiddleName, Suffix = rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                        dgReference.ItemsSource = rfs.ToList();
                        reset();

                    }
                    
                    return;
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
                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgReference, 0));
                        var tr = ctx.References.Where(x => x.ReferenceNumber == num && x.ClientID==cId).First();
                        tr.City=txtRCity.Text;
                        tr.Contact=txtRContact.Text;
                        tr.FirstName=txtRFName.Text;
                        tr.LastName=txtRLName.Text;
                        tr.MiddleName=txtRMI.Text;
                        tr.Province=txtRProvince.Text;
                        tr.Street=txtRStreet.Text;
                        tr.Suffix = txtRSuffix.Text;
                        ctx.SaveChanges();
                        var rfs = from rf in ctx.References
                                  where rf.ClientID == cId
                                  select new { ReferenceNumber= rf.ReferenceNumber, LastName = rf.LastName, FirstName=rf.FirstName, MI=rf.MiddleName, Suffix=rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                        dgReference.ItemsSource = rfs.ToList();
                        reset();
                    }
                    return;
                }


                using (var ctx = new SystemContext())
                {
                    int num = Convert.ToInt32(getRow(dgReference, 0));
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
                        using (var ctx = new SystemContext())
                        {
                            int num = Convert.ToInt32(getRow(dgReference, 0));
                            var tr = ctx.References.Where(x => x.ReferenceNumber == num && x.ClientID==cId).First();
                            txtRCity.Text = tr.City;
                            txtRContact.Text = tr.Contact;
                            txtRFName.Text = tr.FirstName;
                            txtRLName.Text = tr.LastName;
                            txtRMI.Text = tr.MiddleName;
                            txtRProvince.Text = tr.Province;
                            txtRStreet.Text = tr.Street;
                            txtRSuffix.Text = tr.Suffix;
                        }

                        return;
                        
                    }

                    using (var ctx = new SystemContext())
                    {
                        int num = Convert.ToInt32(getRow(dgReference, 0));
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
                        int num1 = Convert.ToInt32(getRow(dgReference, 0));
                        var tr1 = ctx.References.Where(x => x.ReferenceNumber == num1 && x.ClientID==cId).First();
                        ctx.References.Remove(tr1);
                        ctx.SaveChanges();

                        var rfs1 = from rf in ctx.References
                                  where rf.ClientID==cId
                                  select rf;
                        int ctr1 = 1;
                        foreach (var item in rfs1)
                        {
                            item.ReferenceNumber = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();

                        var rfs3 = from rf in ctx.References
                                   where rf.ClientID == cId
                                   select new { ReferenceNumber = rf.ReferenceNumber, LastName = rf.LastName, FirstName = rf.FirstName, MI = rf.MiddleName, Suffix = rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                        dgReference.ItemsSource = rfs3.ToList();

                        reset();
                        return;
                         
                    }

                    int num = Convert.ToInt32(getRow(dgReference, 0));
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

                    var rfs4 = from rf in ctx.TempReferences
                              select new { ReferenceNumber = rf.ReferenceNumber, LastName = rf.LastName, FirstName = rf.FirstName, MI = rf.MiddleName, Suffix = rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                    dgReference.ItemsSource = rfs4.ToList();

                    reset();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (status == "Add")
                {
                    DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to add this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }

                    using (var ctx = new SystemContext())
                    {
                        var num = ctx.Clients.Where(x => x.FirstName == txtFName.Text && x.LastName == txtLName.Text && x.MiddleName == txtMName.Text && x.Birthday == dtBDay.SelectedDate).Count();
                        if (num > 0)
                        {
                            System.Windows.MessageBox.Show("Client already exists");
                            return;
                        }

                        Client clt = new Client { Birthday = Convert.ToDateTime(dtBDay.SelectedDate).Date, Active = true, MiddleName = txtMName.Text, LastName = txtLName.Text, FirstName = txtFName.Text, Email = txtEmail.Text, Sex = cmbSex.Text, SSS = txtSSS.Text, Suffix = txtSuffix.Text, TIN = txtTIN.Text, Status = cmbStatus.Text, Photo = ConvertImageToByteArray(selectedFileName) };

                        var ads = from ad in ctx.TempHomeAddresses
                                  select ad;
                        foreach (var item in ads)
                        {
                            HomeAddress add = new HomeAddress { AddressNumber = item.AddressNumber, City = item.City, LengthOfStay = item.LengthOfStay, MonthlyFee = item.MonthlyFee, OwnershipType = item.OwnershipType, Province = item.Province, Street = item.Street };
                            ctx.HomeAddresses.Add(add);
                        }

                        var cts = from ct in ctx.TempClientContacts
                                  select ct;
                        foreach (var item in cts)
                        {
                            ClientContact con = new ClientContact { Contact = item.Contact, ContactNumber = item.ContactNumber, Primary = item.Primary };
                            ctx.ClientContacts.Add(con);
                        }

                        var dps = from dp in ctx.TempDependents
                                  select dp;
                        foreach (var item in dps)
                        {
                            Dependent dep = new Dependent { Birthday = item.Birthday, DependentNumber = item.DependentNumber, FirstName = item.FirstName, LastName = item.LastName, MiddleName = item.LastName, School = item.School, Suffix = item.Suffix };
                            ctx.Dependents.Add(dep);
                        }
                        var wks = from wk in ctx.TempWorks
                                  select wk;
                        foreach (var item in wks)
                        {
                            Work wrk = new Work { BusinessName = item.BusinessName, BusinessNumber = item.BusinessNumber, City = item.City, DTI = item.DTI, Employment = item.Employment, LengthOfStay = item.LengthOfStay, MonthlyIncome = item.MonthlyIncome, PLNumber = item.PLNumber, Position = item.Position, Province = item.Province, status = item.status, Street = item.Street, WorkNumber = item.WorkNumber };
                            ctx.Works.Add(wrk);
                        }
                        var rfs = from rf in ctx.TempReferences
                                  select rf;
                        foreach (var item in rfs)
                        {
                            Reference rfr = new Reference { City = item.City, Contact = item.Contact, FirstName = item.FirstName, LastName = item.LastName, MiddleName = item.MiddleName, Province = item.Province, ReferenceNumber = item.ReferenceNumber, Street = item.Street, Suffix = item.Suffix };
                            ctx.References.Add(rfr);
                        }

                        if (cmbStatus.Text == "Married")
                        {
                            Spouse sps = new Spouse { Birthday = Convert.ToDateTime(dtSBday.SelectedDate).Date, BusinessName = txtSWName.Text, BusinessNumber = txtSBsNumber.Text, City = txtSCity.Text, DTI = txtSDTI.Text, Employment = cmbSEmployment.Text, FirstName = txtSFName.Text, LastName = txtSLName.Text, LengthOfStay = txtSLength.Text, MiddleName = txtSMName.Text, MonthlyIncome = Convert.ToDouble(txtSIncome.Text), PLNumber = txtSPLNumber.Text, Position = txtSPosition.Text, Province = txtSProvince.Text, status = cmbSStatus.Text, Street = txtSStreet.Text, Suffix = txtSuffix.Text };
                            ctx.Spouses.Add(sps);
                        }

                        ctx.Clients.Add(clt);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Client successfuly added");
                        this.Close();
                    }
                }
                else
                {
                    DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to update this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    using (var ctx = new SystemContext())
                    {
                        var clt = ctx.Clients.Find(cId);
                        clt.Birthday = Convert.ToDateTime(dtBDay.SelectedDate).Date;
                        clt.MiddleName = txtMName.Text;
                        clt.LastName = txtLName.Text;
                        clt.FirstName = txtFName.Text;
                        clt.Email = txtEmail.Text;
                        clt.Sex = cmbSex.Text;
                        clt.SSS = txtSSS.Text;
                        clt.Suffix = txtSuffix.Text;
                        clt.TIN = txtTIN.Text;
                        clt.Status = cmbStatus.Text;
                        if (isChanged == true)
                        {
                            clt.Photo = ConvertImageToByteArray(selectedFileName);
                        }
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("User Updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnPrimary_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Add")
            {
                using (var ctx = new SystemContext())
                {
                    if (ctx.TempClientContacts.Count() < 2)
                    {
                        return;
                    }
                    else
                    {
                        int n = Convert.ToInt32(getRow(dgContact, 0));
                        var cts = from ct in ctx.TempClientContacts
                                  select ct;
                        foreach (var item in cts)
                        {
                            if (item.ContactNumber != n)
                            {
                                item.Primary = false;
                            }
                            else
                            {
                                item.Primary = true;
                            }
                        }
                        ctx.SaveChanges();
                        var cts3 = from ct in ctx.TempClientContacts
                                  select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                        dgContact.ItemsSource = cts3.ToList();
                    }
                }
            }
            else
            {
                int n = Convert.ToInt32(getRow(dgContact, 0));
                using (var ctx = new SystemContext())
                {
                    var cts = from ct in ctx.ClientContacts
                              where ct.ClientID == cId
                              select ct;
                    foreach (var item in cts)
                    {
                        if (item.ContactNumber != n)
                        {
                            item.Primary = false;
                        }
                        else
                        {
                            item.Primary = true;
                        }
                    }
                    ctx.SaveChanges();
                    var cts1 = from ct in ctx.ClientContacts
                               select new { ContactNumber = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                    dgContact.ItemsSource = cts1.ToList();
                }
            }
        }
    }
}
