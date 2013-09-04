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
using LoanManagement.Domain;
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfNewCheque.xaml
    /// </summary>
    public partial class wpfNewCheque : MetroWindow
    {

        public int fId;

        public wpfNewCheque()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new MyLoanContext())
            {
                FPaymentInfo fp = ctx.FPaymentInfo.Find(fId);
                DepositedCheque dp = ctx.DepositedCheques.Find(fId);
                MessageBoxResult mr = MessageBox.Show("You sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mr == MessageBoxResult.Yes)
                {
                    fp.PaymentStatus = "Deposited";
                    fp.ChequeInfo = txtId.Text;
                    dp.DepositDate = DateTime.Today.Date;
                    ctx.SaveChanges();
                    MessageBox.Show("Okay");
                }
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
