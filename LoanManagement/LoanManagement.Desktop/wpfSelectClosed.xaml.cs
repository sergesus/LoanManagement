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
    /// Interaction logic for wpfSelectClosed.xaml
    /// </summary>
    public partial class wpfSelectClosed : MetroWindow
    {
        public wpfSelectClosed()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
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

        private void btnVoid1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "VoidClosed";
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Renewal";
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
