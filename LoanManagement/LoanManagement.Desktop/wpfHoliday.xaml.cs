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

using LoanManagement.Domain;
using System.Data.Entity;
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfHoliday.xaml
    /// </summary>
    public partial class wpfHoliday : MetroWindow
    {

        public bool status;
        public int UserID;

        public wpfHoliday()
        {
            InitializeComponent();
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
                //System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush myBrush = new ImageBrush();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
                myBrush.ImageSource = image.Source;
                wdw1.Background = myBrush;
                resetGrid();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public void resetGrid()
        {
            try
            {
                int n;
                try
                {
                    n = Convert.ToInt16(txtSearch.Text);
                }
                catch (Exception)
                {
                    n = 0;
                }
                if (txtSearch.Text != "")
                {
                    using (var ctx = new newContext())
                    {
                        var bank = from h in ctx.Holidays
                                   where h.HolidayID == n || h.HolidayName.Contains(txtSearch.Text)
                                   select h;
                        dgBank.ItemsSource = bank.ToList();

                    }
                }
                else
                {
                    using (var ctx = new newContext())
                    {
                        var bank = from h in ctx.Holidays
                                   select h;
                        dgBank.ItemsSource = bank.ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfHolidayInfo frm = new wpfHolidayInfo();
                frm.status = "Add";
                frm.UserID = UserID;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfHolidayInfo frm = new wpfHolidayInfo();
                frm.status = "View";
                frm.UserID = UserID;
                frm.hId = Convert.ToInt32(getRow(dgBank, 0));
                frm.txtDesc.Text = getRow(dgBank, 2);
                frm.txtName.Text = getRow(dgBank, 1);
                frm.dt.SelectedDate = Convert.ToDateTime(getRow(dgBank, 4));
                //frm.isYearly.IsChecked = Convert.ToBoolean(getRow(dgBank, 3));
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            try
            {
                resetGrid();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                resetGrid();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            resetGrid();
        }
    }

}
