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

using System.Data.Entity;
using LoanManagement.Domain;

using Microsoft.VisualBasic;
namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfSelectCPayments.xaml
    /// </summary>
    public partial class wpfSelectCPayments : MetroWindow
    {
        public int UserID;

        public wpfSelectCPayments()
        {
            InitializeComponent();
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfLoanSearch frm = new wpfLoanSearch();
                frm.status = "Voiding2";
                frm.UserID = UserID;
                this.Close();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            wpfCPayments frm = new wpfCPayments();
            frm.UserID = UserID;
            this.Close();
            frm.ShowDialog();
        }
    }
}
