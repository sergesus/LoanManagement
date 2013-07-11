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
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            wpfSelectClient frm = new wpfSelectClient();
            frm.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.ShowDialog();
        }
    }
}
