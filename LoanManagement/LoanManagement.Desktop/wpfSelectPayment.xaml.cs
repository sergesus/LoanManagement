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
    /// Interaction logic for wpfSelectPayment.xaml
    /// </summary>
    public partial class wpfSelectPayment : MetroWindow
    {
        public wpfSelectPayment()
        {
            InitializeComponent();
        }

        private void wdw1_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;
        }

        private void btnHold_Click(object sender, RoutedEventArgs e)
        {
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.status = "Holding";
            this.Close();
            frm.ShowDialog();
        }

        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            wpfDepositCheques frm = new wpfDepositCheques();
            this.Close();
            frm.ShowDialog();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            wpfChequeClearing frm = new wpfChequeClearing();
            this.Close();
            frm.ShowDialog();
        }
    }
}
