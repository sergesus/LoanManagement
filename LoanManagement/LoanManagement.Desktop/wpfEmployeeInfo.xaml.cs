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

//

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
    /// Interaction logic for wpfEmployeeInfo.xaml
    /// </summary>
    public partial class wpfEmployeeInfo : MetroWindow
    {
        public string status;
        public int uId;
        string selectedFileName;
        public bool isChanged = false;
        public bool cont = false;

        public wpfEmployeeInfo()
        {
            InitializeComponent();
            dgAddress.IsReadOnly = true;
            dgContact.IsReadOnly = true;
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
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                reset();
                return "";
            }
        }

        public void reset()
        {
            btnAddAddress.Content = "Add";
            btnEdtAddress.Content = "Edit";
            dgAddress.IsEnabled = true;
            btnDelAddress.Visibility = Visibility.Visible;
            grpAddress.Visibility = Visibility.Hidden;
            txtCity.Text = "";
            txtProvince.Text = "";
            txtStreet.Text = "";

            btnAddContact.Content = "Add";
            btnEdtContact.Content = "Edit";
            dgContact.IsEnabled = true;
            btnDelContact.Visibility = Visibility.Visible;
            grpContact.Visibility = Visibility.Hidden;
            txtContact.Text = "";
            int num1=0;
            int num2 = 0;
            if (status == "Add")
            {
                using (var ctx = new SystemContext())
                {
                    num1 = ctx.TempAdresses.Count();
                    num2 = ctx.TempContacts.Count();
                }
            }
            else
            { 
                using (var ctx = new SystemContext())
                {
                    num1 = ctx.EmployeeAddresses.Where(x=> x.EmployeeID==uId).Count();
                    num2 = ctx.EmployeeContacts.Where(x=> x.EmployeeID==uId).Count();
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

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //grpAddress.Visibility = Visibility.Hidden;
            //grpContact.Visibility = Visibility.Hidden;

            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            wdw1.Background = myBrush;

            selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";

            reset();
            if (status == "Add")
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                bitmap.EndInit();
                img.Source = bitmap;

                using (var ctx = new SystemContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempAddresses");
                    ctx.Database.ExecuteSqlCommand("delete from dbo.TempContacts");
                }
                reset();
                btnDel.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSave.Content = "Update";
                btnClear.Visibility = Visibility.Hidden;
                if (uId != 1)
                {
                    btnDel.Visibility = Visibility.Visible;
                }
                else
                {
                    btnDel.Visibility = Visibility.Hidden;
                }
                

                using (var ctx = new SystemContext())
                {
                    var emp = ctx.Employees.Find(uId);
                    txtFName.Text = emp.FirstName;
                    txtLName.Text = emp.LastName;
                    txtEmail.Text = emp.Email;
                    txtMI.Text = emp.MI;
                    txtSuffix.Text = emp.Suffix;
                    cmbDept.Text = emp.Department;
                    cmbPosition.Text = emp.Position;

                    byte[] imageArr;
                    imageArr = emp.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;

                    var add = from cn in ctx.EmployeeAddresses
                              where cn.EmployeeID == uId
                              select new { cn.EmpAddID, cn.Street, cn.Province, cn.City};
                    dgAddress.ItemsSource = add.ToList();

                    var cont = from cn in ctx.EmployeeContacts
                               where cn.EmployeeID == uId
                               select new { cn.EmpContactID, cn.Contact};
                    dgContact.ItemsSource = cont.ToList();

                    grpAddress.Visibility = Visibility.Hidden;
                    grpContact.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
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
                    using (var ctx = new SystemContext())
                    {
                        EmployeeAddress empAdd= new EmployeeAddress { Street = txtStreet.Text, Province = txtProvince.Text, City = txtCity.Text, EmployeeID = uId };
                        ctx.EmployeeAddresses.Add(empAdd);
                        ctx.SaveChanges();
                        var add = from cn in ctx.EmployeeAddresses
                                  where cn.EmployeeID == uId
                                  select new { cn.EmpAddID, cn.Street, cn.Province, cn.City };
                        dgAddress.ItemsSource = add.ToList();
                    }
                    reset();
                    return;
                }

                using (var ctx = new SystemContext())
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
                    using (var ctx = new SystemContext())
                    {
                        var adds = ctx.EmployeeAddresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        adds.City = txtCity.Text;
                        adds.Province = txtProvince.Text;
                        adds.Street = txtStreet.Text;
                        ctx.SaveChanges();
                        var add = from cn in ctx.EmployeeAddresses
                                  where cn.EmployeeID == uId
                                  select new { cn.EmpAddID, cn.Street, cn.Province, cn.City };
                        dgAddress.ItemsSource = add.ToList();
                    }
                    reset();
                    return;
                }
                

                using (var ctx = new SystemContext())
                {
                    var add = ctx.TempAdresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                    add.City=txtCity.Text;
                    add.Province = txtProvince.Text;
                    add.Street = txtStreet.Text;
                    ctx.SaveChanges();
                    dgAddress.ItemsSource = ctx.TempAdresses.ToList();
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
                            var add = ctx.EmployeeAddresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                            txtCity.Text = add.City;
                            txtProvince.Text = add.Province;
                            txtStreet.Text = add.Street;
                        }
                        
                        return;
                    }

                    using (var ctx = new SystemContext())
                    {
                        var add=ctx.TempAdresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
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
                return;
            }
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddContact.Content.ToString() == "Add")
            {
                grpContact.Visibility = Visibility.Visible;
                btnAddContact.Content = "Save";
                btnEdtContact.Content = "Cancel";
                btnDelContact.Visibility = Visibility.Hidden;
                btnEdtContact.IsEnabled = true;

            }
            else if (btnAddContact.Content.ToString() == "Save")
            {
                if (txtContact.Text == "")
                {
                    System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //for view
                if (status == "View")
                {
                    using (var ctx = new SystemContext())
                    {
                        EmployeeContact empCont = new EmployeeContact { Contact=txtContact.Text, EmployeeID = uId };
                        ctx.EmployeeContacts.Add(empCont);
                        ctx.SaveChanges();
                        var cont = from cn in ctx.EmployeeContacts
                                   where cn.EmployeeID == uId
                                   select new { cn.EmpContactID, cn.Contact };
                        dgContact.ItemsSource = cont.ToList();
                    }
                    reset();
                    return;
                }

                using (var ctx = new SystemContext())
                {
                    TempContact con = new TempContact { Contact = txtContact.Text };
                    ctx.TempContacts.Add(con);
                    ctx.SaveChanges();
                    dgContact.ItemsSource = ctx.TempContacts.ToList();
                }

                reset();
            }
            else 
            {
                //for view
                if (status == "View")
                {
                    using (var ctx = new SystemContext())
                    {
                        var conts = ctx.EmployeeContacts.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        conts.Contact = txtContact.Text;
                        ctx.SaveChanges();
                        var cont = from cn in ctx.EmployeeContacts
                                   where cn.EmployeeID == uId
                                   select new { cn.EmpContactID, cn.Contact };
                        dgContact.ItemsSource = cont.ToList();
                    }
                    reset();
                    return;
                }

                using (var ctx = new SystemContext())
                {
                    var con = ctx.TempContacts.Find(Convert.ToInt32(getRow(dgContact, 0)));
                    con.Contact = txtContact.Text;
                    ctx.SaveChanges();
                    dgContact.ItemsSource = ctx.TempContacts.ToList();
                    reset();

                }
            }
        }

        private void btnEdtContact_Click(object sender, RoutedEventArgs e)
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
                        var cont= ctx.EmployeeContacts.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        txtContact.Text = cont.Contact;
                    }

                    return;
                }

                using (var ctx = new SystemContext())
                {
                    var add = ctx.TempContacts.Find(Convert.ToInt32(getRow(dgContact, 0)));
                    txtContact.Text = add.Contact;
                }
            }
            else
            {
                reset();
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
                        var conts = ctx.EmployeeContacts.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        ctx.EmployeeContacts.Remove(conts);
                        ctx.SaveChanges();
                        var cont = from cn in ctx.EmployeeContacts
                                   where cn.EmployeeID == uId
                                   select new { cn.EmpContactID, cn.Contact };
                        dgContact.ItemsSource = cont.ToList();
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
                    return;
                }
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
                        var adds = ctx.EmployeeAddresses.Find(Convert.ToInt32(getRow(dgAddress, 0)));
                        ctx.EmployeeAddresses.Remove(adds);
                        ctx.SaveChanges();
                        var addr = from cn in ctx.EmployeeAddresses
                                   where cn.EmployeeID == uId
                                   select new { cn.EmpAddID, cn.Street, cn.Province, cn.City };
                        dgAddress.ItemsSource = addr.ToList();
                        reset();
                        return;
                    }

                    var con = ctx.TempContacts.Find(Convert.ToInt32(getRow(dgContact, 0)));
                    ctx.TempContacts.Remove(con);
                    ctx.SaveChanges();
                    dgContact.ItemsSource = ctx.TempContacts.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to clear all fields?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            
            txtFName.Text = "";
            txtLName.Text = "";
            txtMI.Text = "";
            txtSuffix.Text = "";
            cmbPosition.Text = "";
            cmbDept.Text = "";
            txtEmail.Text = "";
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

                    if (txtFName.Text == "" || txtLName.Text == "" || cmbPosition.Text == "" || cmbDept.Text == "")
                    {
                        System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!Regex.IsMatch(txtFName.Text, @"^[a-zA-Z]+$"))
                    { 
                        
                    }
                    

                    using (var ctx = new SystemContext())
                    {
                        Employee emp = new Employee { FirstName = txtFName.Text, LastName = txtLName.Text, Position = cmbPosition.Text, Suffix = txtSuffix.Text, MI = txtMI.Text, Active = true, Department = cmbDept.Text, Email = txtEmail.Text, Photo = ConvertImageToByteArray(selectedFileName) };

                        var query = from con in ctx.TempAdresses
                                    select con;
                        foreach (var item in query)
                        {
                            EmployeeAddress add = new EmployeeAddress { Street = item.Street, City = item.City, Province = item.Province };
                            ctx.EmployeeAddresses.Add(add);
                        }

                        var query1 = from con in ctx.TempContacts
                                     select con;
                        foreach (var item in query1)
                        {
                            EmployeeContact con = new EmployeeContact { Contact = item.Contact };
                            ctx.EmployeeContacts.Add(con);
                        }
                        ctx.Employees.Add(emp);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Added New Employee");
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
                        var emp = ctx.Employees.Find(uId);
                        emp.FirstName = txtFName.Text;
                        emp.LastName = txtLName.Text;
                        emp.Position = cmbPosition.Text;
                        emp.Suffix = txtSuffix.Text;
                        emp.MI = txtMI.Text;
                        emp.Department = cmbDept.Text;
                        emp.Email = txtEmail.Text;
                        if (isChanged == true)
                        {
                            emp.Photo = ConvertImageToByteArray(selectedFileName);
                        }
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("User Updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this record?","Question",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    Employee emp = ctx.Employees.Find(uId);
                    emp.Active = false;
                    ctx.SaveChanges();
                    System.Windows.Forms.MessageBox.Show("User successfuly deleted");
                    this.Close();

                }

            }
        }






    }
}
