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
using MahApps.Metro.Controls;
namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfViewClientInfo.xaml
    /// </summary>
    public partial class wpfViewClientInfo : MetroWindow
    {
        public wpfViewClientInfo()
        {
            InitializeComponent();
        }
        public int cID;
        public string status;

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
                //System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return "";
            }
        }

        public void rg()
        {
            try
            {
                using (var ctx = new iContext())
                {
                    if (rdAdd.IsChecked == true)
                    {
                        var ads = from ad in ctx.HomeAddresses
                                  where ad.ClientID == cID
                                  select new { No = ad.AddressNumber, Street = ad.Street, Province = ad.Province, City = ad.City, OwnershipType = ad.OwnershipType, MonthlyFee = ad.MonthlyFee, LengthOfStay = ad.LengthOfStay };
                        dgAddCon.ItemsSource = ads.ToList();
                    }
                    else if (rdCon.IsChecked == true)
                    {
                        var cts = from ct in ctx.ClientContacts
                                  where ct.ClientID == cID
                                  select new { No = ct.ContactNumber, Contact = ct.Contact, Primary = ct.Primary };
                        dgAddCon.ItemsSource =  cts.ToList();
                    }
                    else if (rdDep.IsChecked == true)
                    {
                        var dps = from td in ctx.Dependents
                                  where td.ClientID == cID
                                  select new { No = td.DependentNumber, LastName = td.LastName, FirstName = td.FirstName, MiddleName = td.MiddleName, Suffix = td.Suffix, Birthday = td.Birthday, School = td.School };
                        dgAddCon.ItemsSource = dps.ToList();
                    }
                    else if (rdWorks.IsChecked == true)
                    {
                        var wrk = from wr in ctx.Works
                                  where wr.ClientID == cID
                                  select new { No = wr.WorkNumber, BusinessName = wr.BusinessName, DTI = wr.DTI, Street = wr.Street, Province = wr.Province, City = wr.City, Employment = wr.Employment, LengthOfStay = wr.LengthOfStay, BusinessNumber = wr.BusinessNumber, Position = wr.Position, MonthlyIncome = wr.MonthlyIncome, PLNumber = wr.PLNumber, Status = wr.status };
                        dgAddCon.ItemsSource = wrk.ToList();
                    }
                    else if (rdRef.IsChecked == true)
                    {
                        var rfs = from rf in ctx.References
                                  where rf.ClientID == cID
                                  select new { No = rf.ReferenceNumber, LastName = rf.LastName, FirstName = rf.FirstName, MI = rf.MiddleName, Suffix = rf.Suffix, Street = rf.Street, Province = rf.Province, City = rf.City, Contact = rf.Contact };
                        dgAddCon.ItemsSource = rfs.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void reset()
        {
            try
            {
                using (var ctx = new iContext())
                {
                    var clt = ctx.Clients.Find(cID);
                    byte[] imageArr;
                    imageArr = clt.Photo;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.Default;
                    bi.StreamSource = new MemoryStream(imageArr);
                    bi.EndInit();
                    img.Source = bi;

                    lblBday.Content = clt.Birthday.ToString().Split(' ')[0];
                    var ctr = ctx.ClientContacts.Where(x => x.ClientID == clt.ClientID).Count();
                    if (ctr > 0)
                    {
                        ctr = ctx.ClientContacts.Where(x => x.ClientID == clt.ClientID && x.Primary == true).Count();
                        if (ctr > 0)
                        {
                            var con = ctx.ClientContacts.Where(x => x.ClientID == clt.ClientID && x.Primary == true).First();
                            lblContact.Content = con.Contact;
                        }
                        else
                        {
                            var con = ctx.ClientContacts.Where(x => x.ClientID == clt.ClientID).First();
                            lblContact.Content = con.Contact;
                        }
                    }
                    else
                    {
                        lblContact.Content = "N/A";
                    }

                    lblEmail.Content = clt.Email;
                    lblGender.Content = clt.Sex;
                    lblName.Content = clt.LastName + ", " + clt.FirstName + " " + clt.MiddleName + " " + clt.Suffix;
                    lblSSS.Content = clt.SSS;
                    lblStatus.Content = clt.Status;
                    lblTIN.Content = clt.TIN;

                    var lon = from ln in ctx.Loans
                              where ln.ClientID == cID
                              select new { LoanID = ln.LoanID, TypeOfLoan = ln.Service.Name, Type = ln.Service.Type, Status = ln.Status};
                    dgLoans.ItemsSource = lon.ToList();

                    if (status == "View2")
                    {
                        btnView.Visibility = Visibility.Hidden;
                    }
                    rg();
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
                rdAdd.IsChecked = true;
                reset();
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                //Grid grid = new Grid();
                wdw1.Background = myBrush;
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void rdAdd_Checked(object sender, RoutedEventArgs e)
        {
            rg();
        }

        private void rdCon_Checked(object sender, RoutedEventArgs e)
        {
            rg();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new iContext())
                {
                    int n = Convert.ToInt32(getRow(dgLoans, 0));
                    var lon = ctx.Loans.Find(n);
                    if (lon.Status == "Applied" || lon.Status == "Declined" || lon.Status == "Approved")
                    {
                        wpfAppliedLoanInfo frm = new wpfAppliedLoanInfo();
                        frm.lId = n;
                        frm.status = "View";
                        frm.Height = 605.5;
                        frm.ShowDialog();
                    }
                    else
                    {
                        wpfReleasedLoanInfo frm = new wpfReleasedLoanInfo();
                        frm.lId = n;
                        frm.status = "View";
                        frm.Height = 605.5;
                        frm.ShowDialog();
                    }
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
