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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfBranch.xaml
    /// </summary>
    public partial class wpfBranch : Window
    {
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
                System.Windows.MessageBox.Show("Please select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            resetGrid();
        }

        public void resetGrid()
        {
            using (var ctx = new SystemContext())
            {
                var bank = from bn in ctx.Banks
                           where bn.Active == true
                           select new { BankID = bn.BankID, BankName= bn.BankName, Description=bn.Description };
                dgBank.ItemsSource = bank.ToList();

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            wpfBranchInfo frm = new wpfBranchInfo();
            frm.status = "Add";
            frm.ShowDialog();
            
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wpfBranchInfo frm = new wpfBranchInfo();
                frm.status = "View";
                frm.bId = Convert.ToInt16(getRow(dgBank, 0));
                frm.txtDesc.Text = getRow(dgBank, 2);
                frm.txtName.Text = getRow(dgBank, 1);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            resetGrid();
        }

        
    }
}
