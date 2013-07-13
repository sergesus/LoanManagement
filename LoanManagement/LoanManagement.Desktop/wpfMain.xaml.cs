﻿using System;
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
            MessageBox.Show("asd");
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
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

        private void MetroWindow_Activated_1(object sender, EventArgs e)
        {
            //lbM.UnselectAll();
            reset();
        }

        private void reset()
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

        }


        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            wpfLogin frm = new wpfLogin();
            frm.Show();
            this.Close();
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
            //grdLo.Background = myBrush;
            //grdM.Background = myBrush;
            //grdBG.Background = myBrush;
        }


        private void ListBoxItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            wpfClient frm = new wpfClient();
            frm.ShowDialog();
        }

        private void ListBoxItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {

            wpfServices frm = new wpfServices();
            frm.ShowDialog();
        }

        private void ListBoxItem_MouseUp_3(object sender, MouseButtonEventArgs e)
        {

            wpfBranch frm = new wpfBranch();
            frm.ShowDialog();
        }

        private void ListBoxItem_MouseUp_4(object sender, MouseButtonEventArgs e)
        {
            wpfEmployee frm = new wpfEmployee();
            frm.ShowDialog();
        }

        private void TabItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            reset();
        }

        private void TabItem_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            reset();
        }

        private void ListBoxItem_MouseUp_5(object sender, MouseButtonEventArgs e)
        {
            reset();
        }

        private void itm5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpfAgent frm = new wpfAgent();
            frm.ShowDialog();
        }

        private void itm6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpfSelectApplication frm = new wpfSelectApplication();
            //frm.status = "Application";
            frm.ShowDialog();
        }

        private void itm8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wpfLoanSearch frm = new wpfLoanSearch();
            frm.status = "Approval";
            frm.ShowDialog();
        }







    }
}
