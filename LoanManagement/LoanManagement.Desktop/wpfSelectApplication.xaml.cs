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
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfSelectApplication.xaml
    /// </summary>
    public partial class wpfSelectApplication : MetroWindow
    {
        public string status;

        public wpfSelectApplication()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;
            if (status == "Approval")
            {
                btnNew.Content = "View all Applied Loans";
                btnView.Content = "View/Update Approved and Declined Loans";
            }
            else if (status == "Releasing")
            {
                btnNew.Content = "View all Approved Loans";
                btnView.Content = "View/Update Released Loans";
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Application")
            {
                this.Close();
                wpfSelectClient frm = new wpfSelectClient();
                frm.ShowDialog();
            }
            else
            {
                this.Close();
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = status;
                frm.ShowDialog();
            }

        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (status == "Application")
            {
                this.Close();
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Application";
                frm.ShowDialog();
            }
            else if (status == "Approval")
            {
                this.Close();
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "UApproval";
                frm.ShowDialog();
            }
            else if (status == "Releasing")
            {
                this.Close();
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "UReleasing";
                frm.ShowDialog();
            }
        }
    }
}
