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
    /// Interaction logic for wpfLoanApplication.xaml
    /// </summary>
    public partial class wpfLoanApplication : Window
    {
        public int cId;

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
        }

        private void cmbServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var ctx = new SystemContext())
            {
                ComboBoxItem typeItem = (ComboBoxItem)cmbServices.SelectedItem;
                string value = typeItem.Content.ToString();
                var ser = ctx.Services.Where(x => x.Name == value).First();
                txtCat.Text = ser.Type;

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


    }
}
