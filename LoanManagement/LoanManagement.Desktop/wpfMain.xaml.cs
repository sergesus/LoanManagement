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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfMain.xaml
    /// </summary>
    public partial class wpfMain : Window
    {
        public wpfMain()
        {
            InitializeComponent();
        }

        private void txtEmployee_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpfEmployee emp = new wpfEmployee();
            emp.ShowDialog();
        }

        private void txtServices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpfServices ser = new wpfServices();
            ser.ShowDialog();
        }

        private void txtEmployee_Click(object sender, RoutedEventArgs e)
        {
            wpfEmployee emp = new wpfEmployee();
            emp.ShowDialog();
        }

        private void txtServices_Click(object sender, RoutedEventArgs e)
        {
            wpfServices ser = new wpfServices();
            ser.ShowDialog();
        }

    }
}
