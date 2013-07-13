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
    /// Interaction logic for wpfAppliedLoanInfo.xaml
    /// </summary>
    public partial class wpfAppliedLoanInfo : MetroWindow
    {

        public int lId;
        public wpfAppliedLoanInfo()
        {
            InitializeComponent();
        }

        public void reset()
        {
            using (var ctx = new SystemContext())
            {
                var lon = ctx.Loans.Find(lId);

                byte[] imageArr;
                imageArr = lon.Client.Photo;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CreateOptions = BitmapCreateOptions.None;
                bi.CacheOption = BitmapCacheOption.Default;
                bi.StreamSource = new MemoryStream(imageArr);
                bi.EndInit();
                img.Source = bi;

                lblTOL.Content = lon.TypeOfLoan;
                lblType.Content = lon.Type;
                lblMode.Content = lon.Mode;
                lblInt.Content = lon.Interest + "%";
                lblPen.Content = lon.Penalty + "%";
                lblCom.Content = lon.Commission + "%";
                lblTerm.Content = lon.Term;
                lblDed.Content = lon.Deduction + "%";
                lblAmt.Content = "Php " + lon.LoanApplication.AmountApplied.ToString("N0");
                lblDt.Content = lon.LoanApplication.DateApplied.ToString();

                lblName.Content = lon.Client.LastName + ", " + lon.Client.FirstName + " " + lon.Client.MiddleName;

            }
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            wpfAgentInfo frm = new wpfAgentInfo();
            frm.status = "View";
            using (var ctx = new SystemContext())
            {
                var lon = ctx.Loans.Find(lId);
                frm.aId = lon.AgentID;
            }
            frm.ShowDialog();
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanApproval frm = new wpfLoanApproval();
            frm.lId = lId;
            frm.ShowDialog();
        }
    }
}
