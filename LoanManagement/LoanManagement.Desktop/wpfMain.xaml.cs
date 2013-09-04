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
    /// Interaction logic for wpfMain.xaml
    /// </summary>
    public partial class wpfMain : MetroWindow
    {


        public wpfMain()
        {
            InitializeComponent();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("asd");
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                reset();
                if (lb1.SelectedIndex == 0)
                {
                    tb1.IsSelected = true;
                }
                else if (lb1.SelectedIndex == 1)
                {
                    tb2.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MetroWindow_Activated_1(object sender, EventArgs e)
        {
            try
            {
                //lbM.UnselectAll();
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void reset()
        {
            try
            {
                itm1.IsSelected = false;
                itm2.IsSelected = false;
                itm3.IsSelected = false;
                itm4.IsSelected = false;
                itm5.IsSelected = false;
                itm6.IsSelected = false;
                itm7.IsSelected = false;
                itm8.IsSelected = false;
                itm9.IsSelected = false;
                itm10.IsSelected = false;
                itm11.IsSelected = false;
                itm12.IsSelected = false;
                itm13.IsSelected = false;
                itm14.IsSelected = false;
                itm15.IsSelected = false;
                itm16.IsSelected = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLogin frm = new wpfLogin();
                frm.Show();
                this.Close();
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
                //Grid grid = new Grid();
                wdw1.Background = myBrush;
                //grdLo.Background = myBrush;
                //grdM.Background = myBrush;
                //grdBG.Background = myBrush;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void ListBoxItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfBranch frm = new wpfBranch();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_4(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBoxItem_MouseUp_5(object sender, MouseButtonEventArgs e)
        {
            try
            {
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm8_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm9_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm9_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm10_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void itm10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectPayment frm = new wpfSelectPayment();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm12_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectClosed frm = new wpfSelectClosed();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm13_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.iDept = "Financing";
                frm.status = "Adjustment";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm11_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Restructure";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void itm14_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfClient frm = new wpfClient();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfServices frm = new wpfServices();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnBank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfBranch frm = new wpfBranch();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfEmployee frm = new wpfEmployee();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAgents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfAgent frm = new wpfAgent();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoanAppllication_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Releasing";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectPayment frm = new wpfSelectPayment();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectClosed frm = new wpfSelectClosed();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Restructure";
                frm.iDept = "Financing";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.iDept = "Financing";
                frm.status = "Adjustment";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnLoanAppllication_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Application";
                frm.iDept = "Micro Business";
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnClients_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfSelectApplication frm = new wpfSelectApplication();
                frm.status = "Approval";
                frm.iDept = "Micro Business";
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
