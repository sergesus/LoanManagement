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
    /// Interaction logic for wpfAgentInfo.xaml
    /// </summary>
    public partial class wpfAgentInfo : MetroWindow
    {

        public string status;
        public int aId;
        string selectedFileName;
        public bool isChanged = false;
        public int UserID;

        public wpfAgentInfo()
        {
            InitializeComponent();
        }

        public void checkNumeric(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                bool err = false;
                int res;
                String str = txt.Text;
                err = int.TryParse(str, out res);

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "**";
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
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
                bool err = false;
                double res;
                String str = txt.Text;
                err = double.TryParse(str, out res);

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "**";
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
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
                    lbl.Content = "**";
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
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
                    lbl.Content = "**";
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
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

        public void checkString(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
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
                    lbl.Content = "**";
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
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

                btnAddContact.Content = "Add";
                btnEdtContact.Content = "Edit";
                dgContact.IsEnabled = true;
                btnDelContact.Visibility = Visibility.Visible;
                grpContact.Visibility = Visibility.Hidden;
                txtContact.Text = "";
                int num1 = 0;
                int num2 = 0;
                if (status == "Add")
                {
                    using (var ctx = new iContext())
                    {
                        num1 = ctx.TempAgentAddresses.Count();
                        num2 = ctx.TempAgentContact.Count();
                    }
                }
                else
                {
                    using (var ctx = new iContext())
                    {
                        num1 = ctx.AgentAddresses.Where(x => x.AgentID == aId).Count();
                        num2 = ctx.AgentContacts.Where(x => x.AgentID == aId).Count();
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
                if (lblFName.Content=="**" || lblLName.Content=="**" || lblMI.Content=="**" || lblEmail.Content=="**" || lblSuffix.Content=="**"
                    || String.IsNullOrWhiteSpace(txtFName.Text) || String.IsNullOrWhiteSpace(txtLName.Text))
                {
                    System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields");
                    return;
                }

                if (status == "Add")
                {

                    DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to add this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }

                    if (txtFName.Text == "" || txtLName.Text == "")
                    {
                        System.Windows.MessageBox.Show("Please complete the required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!Regex.IsMatch(txtFName.Text, @"^[a-zA-Z]+$"))
                    {

                    }


                    using (var ctx = new iContext())
                    {
                        Agent agt = new Agent { FirstName = txtFName.Text, LastName = txtLName.Text,  Suffix = txtSuffix.Text, MI = txtMI.Text, Active = true,  Email = txtEmail.Text, Photo = ConvertImageToByteArray(selectedFileName) };

                        var query = from con in ctx.TempAgentAddresses
                                    select con;
                        foreach (var item in query)
                        {
                            AgentAddress add = new AgentAddress { AddressNumber=item.AddressNumber, Street = item.Street, City = item.City, Province = item.Province };
                            ctx.AgentAddresses.Add(add);
                        }

                        var query1 = from con in ctx.TempAgentContact
                                     select con;
                        foreach (var item in query1)
                        {
                            AgentContact con = new AgentContact { CNumber=item.CNumber, Contact = item.Contact };
                            ctx.AgentContacts.Add(con);
                        }
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Added new Agent " + agt.FirstName + " " + agt.MI + " " + agt.LastName + " " + agt.Suffix};
                        ctx.AuditTrails.Add(at);
                        ctx.Agents.Add(agt);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Added New Agent");
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
                    using (var ctx = new iContext())
                    {
                        var agt = ctx.Agents.Find(aId);
                        agt.FirstName = txtFName.Text;
                        agt.LastName = txtLName.Text;
                        agt.Suffix = txtSuffix.Text;
                        agt.MI = txtMI.Text;
                        agt.Email = txtEmail.Text;
                        if (isChanged == true)
                        {
                            agt.Photo = ConvertImageToByteArray(selectedFileName);
                        }
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Agent " + agt.FirstName + " " + agt.MI + " " + agt.LastName + " " + agt.Suffix };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Agent Updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new iContext())
                    {
                        var agt = ctx.Agents.Find(aId);
                        agt.Active = false;
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Deleted");
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

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
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
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;

                selectedFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif";
                tbInfo.IsSelected = true;
                reset();
                if (status == "Add")
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\myImg.gif");
                    bitmap.EndInit();
                    img.Source = bitmap;

                    using (var ctx = new iContext())
                    {
                        ctx.Database.ExecuteSqlCommand("delete from dbo.TempAgentAddresses");
                        ctx.Database.ExecuteSqlCommand("delete from dbo.TempAgentContacts");
                    }
                    reset();
                    btnDel.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnSave.Content = "Update";
                    //btnClear.Visibility = Visibility.Hidden;

                    using (var ctx = new iContext())
                    {
                        var ctr = ctx.Loans.Where(x => x.AgentID == aId && x.Status == "Released").Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Only selected values be updated and Record cannot be deleted at this moment", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            btnDel.IsEnabled = false;
                            txtLName.IsEnabled = false;
                            txtFName.IsEnabled = false;
                            txtMI.IsEnabled = false;
                            txtSuffix.IsEnabled = false;
                        }

                        var agt = ctx.Agents.Find(aId);
                        txtFName.Text = agt.FirstName;
                        txtLName.Text = agt.LastName;
                        txtEmail.Text = agt.Email;
                        txtMI.Text = agt.MI;
                        txtSuffix.Text = agt.Suffix;

                        byte[] imageArr;
                        imageArr = agt.Photo;
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CreateOptions = BitmapCreateOptions.None;
                        bi.CacheOption = BitmapCacheOption.Default;
                        bi.StreamSource = new MemoryStream(imageArr);
                        bi.EndInit();
                        img.Source = bi;

                        var add = from cn in ctx.AgentAddresses
                                  where cn.AgentID == aId
                                  select new { cn.AddressNumber, cn.Street, cn.Province, cn.City };
                        dgAddress.ItemsSource = add.ToList();

                        var cont = from cn in ctx.AgentContacts
                                   where cn.AgentID == aId
                                   select new { cn.CNumber, cn.Contact };
                        dgContact.ItemsSource = cont.ToList();

                        grpAddress.Visibility = Visibility.Hidden;
                        grpContact.Visibility = Visibility.Hidden;

                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAddAddress.Content.ToString() != "Add")
                {
                    if (lblStreet.Content == "**" || lblProvince.Content == "**" || lblCity.Content == "**"
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
                            int ctr = ctx.AgentAddresses.Count() + 1;
                            AgentAddress add = new AgentAddress { AgentID = aId, AddressNumber = ctr, Street = txtStreet.Text, Province = txtProvince.Text, City = txtCity.Text };
                            ctx.AgentAddresses.Add(add);
                            ctx.SaveChanges();
                            var adr = from ad in ctx.AgentAddresses
                                      where ad.AgentID == aId
                                      select new { ad.AddressNumber, ad.Street, ad.Province, ad.City };
                            dgAddress.ItemsSource = adr.ToList();
                        }

                        reset();
                        return;

                    }

                    using (var ctx = new iContext())
                    {
                        int ctr = ctx.TempAgentAddresses.Count() + 1;
                        TempAgentAddress add = new TempAgentAddress { AddressNumber = ctr, Street = txtStreet.Text, Province = txtProvince.Text, City = txtCity.Text };
                        ctx.TempAgentAddresses.Add(add);
                        ctx.SaveChanges();
                        var adr = from ad in ctx.TempAgentAddresses
                                  select new { ad.AddressNumber, ad.Street, ad.Province, ad.City };
                        dgAddress.ItemsSource = adr.ToList();
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
                            int num = Convert.ToInt32(getRow(dgAddress, 0));
                            var add = ctx.AgentAddresses.Where(x => x.AddressNumber == num && x.AgentID == aId).First();
                            add.City = txtCity.Text;
                            add.Province = txtProvince.Text;
                            add.Street = txtStreet.Text;
                            ctx.SaveChanges();
                            var adr = from ad in ctx.AgentAddresses
                                      where ad.AgentID == aId
                                      select new { ad.AddressNumber, ad.Street, ad.Province, ad.City };
                            dgAddress.ItemsSource = adr.ToList();

                        }
                        reset();
                        return;
                    }


                    using (var ctx = new iContext())
                    {
                        int num = Convert.ToInt32(getRow(dgAddress, 0));
                        var add = ctx.TempAgentAddresses.Where(x => x.AddressNumber == num).First();
                        add.City = txtCity.Text;
                        add.Province = txtProvince.Text;
                        add.Street = txtStreet.Text;
                        ctx.SaveChanges();
                        var adr = from ad in ctx.TempAgentAddresses
                                  select new { ad.AddressNumber, ad.Street, ad.Province, ad.City };
                        dgAddress.ItemsSource = adr.ToList();
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
                            int num = Convert.ToInt32(getRow(dgAddress, 0));
                            var add = ctx.AgentAddresses.Where(x => x.AddressNumber == num && x.AgentID==aId).First();
                            txtCity.Text = add.City;
                            txtProvince.Text = add.Province;
                            txtStreet.Text = add.Street;
                        }

                        return;
                    }

                    using (var ctx = new iContext())
                    {
                        int num = Convert.ToInt32(getRow(dgAddress, 0));
                        var add = ctx.TempAgentAddresses.Where(x => x.AddressNumber == num).First();
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
                        int num1 = Convert.ToInt32(getRow(dgAddress, 0));
                        var add1 = ctx.AgentAddresses.Where(x => x.AddressNumber == num1 && x.AgentID==aId).First();
                        ctx.AgentAddresses.Remove(add1);
                        ctx.SaveChanges();

                        var reqs1 = from req in ctx.AgentAddresses
                                   where req.AgentID==aId
                                   select req;
                        int ctr1 = 1;
                        foreach (var item in reqs1)
                        {
                            item.AddressNumber = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();
                        var adr1 = from ad in ctx.AgentAddresses
                                   where ad.AgentID == aId
                                  select new { ad.AddressNumber, ad.Street, ad.Province, ad.City };
                        dgAddress.ItemsSource = adr1.ToList();
                        reset();
                        return;
                         
                    }

                    int num = Convert.ToInt32(getRow(dgAddress, 0));
                    var add = ctx.TempAgentAddresses.Where(x => x.AddressNumber == num).First();
                    ctx.TempAgentAddresses.Remove(add);
                    ctx.SaveChanges();

                    var reqs = from req in ctx.TempAgentAddresses
                               select req;
                    int ctr = 1;
                    foreach (var item in reqs)
                    {
                        item.AddressNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var adr = from ad in ctx.TempAgentAddresses
                              select new { ad.AddressNumber, ad.Street, ad.Province, ad.City };
                    dgAddress.ItemsSource = adr.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAddContact.Content.ToString() != "Add")
                {
                    if (lblContact.Content == "**" || String.IsNullOrWhiteSpace(txtContact.Text))
                    {
                        System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields");
                        return;
                    }
                }

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


                    //for view
                    if (status == "View")
                    {
                        using (var ctx = new iContext())
                        {
                            int ctr = ctx.AgentContacts.Count() + 1;
                            AgentContact con = new AgentContact { AgentID = aId, CNumber = ctr, Contact = txtContact.Text };
                            ctx.AgentContacts.Add(con);
                            ctx.SaveChanges();
                            var cts = from ct in ctx.AgentContacts
                                      where ct.AgentID == aId
                                      select new { ContactNumberID = ct.CNumber, Contact = ct.Contact };
                            dgContact.ItemsSource = cts.ToList();
                        }

                        reset();
                        return;

                    }

                    using (var ctx = new iContext())
                    {
                        int ctr = ctx.TempAgentContact.Count() + 1;
                        TempAgentContact con = new TempAgentContact { CNumber = ctr, Contact = txtContact.Text };
                        ctx.TempAgentContact.Add(con);
                        ctx.SaveChanges();
                        var cts = from ct in ctx.TempAgentContact
                                  select new { ContactNumberID = ct.CNumber, Contact = ct.Contact };
                        dgContact.ItemsSource = cts.ToList();
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

                            int num = Convert.ToInt32(getRow(dgContact, 0));
                            var con = ctx.AgentContacts.Where(x => x.CNumber == num && x.AgentID == aId).First();
                            con.Contact = txtContact.Text;
                            ctx.SaveChanges();
                            var cts = from ct in ctx.AgentContacts
                                      where ct.AgentID == aId
                                      select new { ContactNumberID = ct.CNumber, Contact = ct.Contact };
                            dgContact.ItemsSource = cts.ToList();

                        }
                        reset();
                        return;
                    }


                    using (var ctx = new iContext())
                    {
                        int num = Convert.ToInt32(getRow(dgContact, 0));
                        var con = ctx.TempAgentContact.Where(x => x.CNumber == num).First();
                        con.Contact = txtContact.Text;
                        ctx.SaveChanges();
                        var cts = from ct in ctx.TempAgentContact
                                  select new { ContactNumberID = ct.CNumber, Contact = ct.Contact };
                        dgContact.ItemsSource = cts.ToList();
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
                        using (var ctx = new iContext())
                        {
                            int num = Convert.ToInt32(getRow(dgContact, 0));
                            var con = ctx.AgentContacts.Where(x => x.CNumber == num && x.AgentID==aId).First();
                            txtContact.Text = con.Contact;
                        }

                        return;
                    }

                    using (var ctx = new iContext())
                    {
                        int num = Convert.ToInt32(getRow(dgContact, 0));
                        var con = ctx.TempAgentContact.Where(x => x.CNumber == num).First();
                        txtContact.Text = con.Contact;
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

        private void btnDelContact_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new iContext())
            {
                try
                {
                    //for view
                    if (status == "View")
                    {
                        int num1 = Convert.ToInt32(getRow(dgContact, 0));
                        var con1 = ctx.AgentContacts.Where(x => x.CNumber == num1 && x.AgentID==aId).First();
                        ctx.AgentContacts.Remove(con1);
                        ctx.SaveChanges();

                        var reqs1 = from req in ctx.AgentContacts
                                   where req.AgentID==aId
                                   select req;
                        int ctr1 = 1;
                        foreach (var item in reqs1)
                        {
                            item.CNumber = ctr1;
                            ctr1++;
                        }
                        ctx.SaveChanges();
                        var cts1 = from ct in ctx.AgentContacts
                                  where ct.AgentID == aId
                                  select new { ContactNumberID = ct.CNumber, Contact = ct.Contact };
                        dgContact.ItemsSource = cts1.ToList();
                        reset();
                        return;
                         
                    }

                    int num = Convert.ToInt32(getRow(dgContact, 0));
                    var con = ctx.TempAgentContact.Where(x => x.CNumber == num).First();
                    ctx.TempAgentContact.Remove(con);
                    ctx.SaveChanges();

                    var reqs = from req in ctx.TempAgentContact
                               select req;
                    int ctr = 1;
                    foreach (var item in reqs)
                    {
                        item.CNumber = ctr;
                        ctr++;
                    }
                    ctx.SaveChanges();
                    var cts = from ct in ctx.TempAgentContact
                              select new { ContactNumberID = ct.CNumber, Contact = ct.Contact };
                    dgContact.ItemsSource = cts.ToList();
                    reset();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void txtLName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtLName, lblLName, true);
        }

        private void txtFName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtFName, lblFName, true);
        }

        private void Label_LostFocus_1(object sender, RoutedEventArgs e)
        {
            checkName(txtMI, lblMI, !true);
        }

        private void txtSuffix_LostFocus(object sender, RoutedEventArgs e)
        {
            checkName(txtSuffix, lblSuffix, !true);
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            checkEmail(txtEmail, lblEmail, false);
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
            checkName(txtCity, lblCity, true);
        }

        private void txtContact_LostFocus(object sender, RoutedEventArgs e)
        {
            checkNumeric(txtContact, lblContact, true);
        }
        
    }
}
