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

using System.Data.Entity;
using MahApps.Metro.Controls;
using LoanManagement.Domain;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfPosition.xaml
    /// </summary>
    public partial class wpfPosition : MetroWindow
    {
        public int UserID;
        public wpfPosition()
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

        public void resetGrid()
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var post = from ps in ctx.Positions
                               select new { PositionID = ps.PositionID, Position = ps.PositionName, Description = ps.Description };
                    dgBank.ItemsSource = post.ToList();

                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                wdw1.Background = myBrush;
                resetGrid();

                using (var ctx = new finalContext())
                {
                    
                    var usr = ctx.Employees.Find(UserID);
                    var pos = ctx.PositionScopes.Find(usr.PositionID);
                    if (pos.UScopes == true)
                    {
                        btnScope.IsEnabled = true;
                    }
                    else
                    {
                        btnScope.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfPositionInfo frm = new wpfPositionInfo();
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
                int n = Convert.ToInt32(getRow(dgBank, 0));
                if (n == 1)
                {
                    MessageBox.Show("Unable to process", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                wpfPositionInfo frm = new wpfPositionInfo();
                frm.status = "View";
                frm.UserID = UserID;
                frm.pID = n;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void wdw1_Activated(object sender, EventArgs e)
        {
            resetGrid();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
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
                    var post = from ps in ctx.Positions
                               where (ps.PositionName.Contains(txtSearch.Text) || ps.PositionID == n)
                               select new { PositionID = ps.PositionID, Position = ps.PositionName, Description = ps.Description };
                    dgBank.ItemsSource = post.ToList();

                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnScope_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var n2 = ctx.Employees.Find(UserID);
                    int n = Convert.ToInt32(getRow(dgBank, 0));
                    var emp = ctx.Positions.Find(n);
                    if (emp.PositionName == "Administrator" || n2.PositionID == emp.PositionID)
                    {
                        System.Windows.MessageBox.Show("Unable to edit Administrator/Current Position", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        wpfPassword frm = new wpfPassword();
                        frm.status = "scope";
                        frm.ID = UserID;
                        frm.eID = n;
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
