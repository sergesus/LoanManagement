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
    /// Interaction logic for wpfBranch.xaml
    /// </summary>
    public partial class wpfBranch : MetroWindow
    {
        public bool status;
        public int UserID;

        public wpfBranch()
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

                if (status == false)//maintenance
                {
                    btnView.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnRet.Visibility = Visibility.Visible;
                    myLbL.Content = "Bank Retreival";
                }
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
                using (var ctx = new iContext())
                {
                    var bank = from bn in ctx.Banks
                               where bn.Active == status
                               select new { BankID = bn.BankID, BankName = bn.BankName, Description = bn.Description };
                    dgBank.ItemsSource = bank.ToList();

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
                wpfBranchInfo frm = new wpfBranchInfo();
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
                wpfBranchInfo frm = new wpfBranchInfo();
                frm.status = "View";
                frm.UserID = UserID;
                frm.bId = Convert.ToInt32(getRow(dgBank, 0));
                frm.txtDesc.Text = getRow(dgBank, 2);
                frm.txtName.Text = getRow(dgBank, 1);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnRet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = Convert.ToInt32(getRow(dgBank, 0));
                MessageBoxResult mr = System.Windows.MessageBox.Show("Are you sure you want to retreive this record?", "Question", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    using (var ctx = new iContext())
                    {
                        var agt = ctx.Banks.Find(n);
                        agt.Active = true;
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Retrieved Bank " + getRow(dgBank,1) };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Record has been successfully retreived", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        resetGrid();
                    }
                }
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
                using (var ctx = new iContext())
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
                    var bank = from bn in ctx.Banks
                               where (bn.Active == status) && (bn.BankName.Contains(txtSearch.Text) || bn.BankID == n)
                               select new { BankID = bn.BankID, BankName = bn.BankName, Description = bn.Description };
                    dgBank.ItemsSource = bank.ToList();

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        
    }
}
