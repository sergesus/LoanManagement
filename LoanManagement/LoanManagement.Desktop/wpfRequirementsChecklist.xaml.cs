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
using MahApps.Metro.Controls;
using System.Data.Entity;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfRequirementsChecklist.xaml
    /// </summary>
    public partial class wpfRequirementsChecklist : MetroWindow
    {
        public int UserID;
        public int lID;

        public wpfRequirementsChecklist()
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

        public void rg()
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lID);

                    var chq = from ch in ctx.Requirements
                              where ch.ServiceID == lon.ServiceID
                              && !(from o in ctx.RequirementChecklists where o.LoanID==lID select o.RequirementId).Contains(ch.RequirementId)
                              select new { ReqNum = ch.RequirementNum, Requirement = ch.Name };
                    dg1.ItemsSource = chq.ToList();

                    var chq1 = from ch in ctx.RequirementChecklists
                               where ch.LoanID == lID
                               select new { ReqNum = ch.Requirement.RequirementNum, Requirement = ch.Requirement.Name, ConfirmedBy = ch.Employee.LastName + ", " + ch.Employee.FirstName, DateConfirmed = ch.DateConfirmed };
                    dg2.ItemsSource = chq1.ToList();
                }
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

                using (var ctx = new finalContext())
                {
                    ctx.Database.ExecuteSqlCommand("delete  from dbo.TempClearings");
                }
                rg();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.Windows.MessageBox.Show("Are you sure you want to confirm this requirement?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = ctx.Loans.Find(lID);
                        int n = Convert.ToInt32(getRow(dg1,0));
                        var rq = ctx.Requirements.Where(x=> x.ServiceID == lon.ServiceID && x.RequirementNum == n ).First();
                        RequirementChecklist rc = new RequirementChecklist { DateConfirmed = DateTime.Now.Date, EmployeeID = UserID, LoanID = lID, RequirementId = rq.RequirementId };
                        ctx.RequirementChecklists.Add(rc);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Requirement confirmation successfull", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        rg();
                        return;
                    }
                }
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
                if (System.Windows.MessageBox.Show("Are you sure you want to remove the confirmation?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var ctx = new finalContext())
                    {
                        var lon = ctx.Loans.Find(lID);
                        int n = Convert.ToInt32(getRow(dg2, 0));
                        var rq = ctx.RequirementChecklists.Where(x => x.LoanID == lID && x.Requirement.RequirementNum == n).First();
                        ctx.RequirementChecklists.Remove(rq);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Requirement confirmation has been removed successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        rg();
                        return;
                    }
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
