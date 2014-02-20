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
using System.Data.Entity;
using LoanManagement.Domain;
using System.Windows.Forms;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfPositionInfo.xaml
    /// </summary>
    public partial class wpfPositionInfo : MetroWindow
    {
        public string status;
        public int pID;
        public int UserID;

        public wpfPositionInfo()
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
                wdw1.Background = myBrush;

                if (status == "View")
                {
                    using (var ctx = new finalContext())
                    {
                        Domain.Position pos = ctx.Positions.Find(pID);
                        txtPosition.Text = pos.PositionName;
                        txtDesc.Text = pos.Description;
                        btnSave.Content = "Save";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPosition.Text == "Administrator")
                {
                    System.Windows.MessageBox.Show("Administrator cannot be used as position name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                if (status == "Add")
                {
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.Positions.Where(x => x.PositionName == txtPosition.Text).Count();
                        if (ctr > 0)
                        {
                            System.Windows.MessageBox.Show("Position already exists.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        Domain.Position pos = new Domain.Position { PositionName = txtPosition.Text, Description = txtDesc.Text };
                        PositionScope scp = new PositionScope { MAgent = false, MClient = false, MHoliday = false, MBank = false, MEmployee = false, MPosition = false, MRegistration = false, MService = false, TApplication = false, TApproval = false, TCollection = false, TManageClosed = false, TOnlineConfirmation = false, TPaymentAdjustment = false, TPayments = false, TReleasing = false, TResturcture = false, UArchive = false, UBackUp = false, UOnlineSettings = false, UReports = false, UScopes = false, UStatistics = false, UUserAccounts = false };
                        ctx.Positions.Add(pos);


                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Added new Position " + txtPosition.Text };
                        ctx.AuditTrails.Add(at);

                        ctx.SaveChanges();
                        scp.PositionID = pos.PositionID;
                        ctx.PositionScopes.Add(scp);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Record has been successfully added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else
                {
                    using (var ctx = new finalContext())
                    {
                        Domain.Position pos = ctx.Positions.Find(pID);
                        pos.PositionName = txtPosition.Text;
                        pos.Description = txtDesc.Text;
                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Position " + txtPosition.Text };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Record has been successfully updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var ctr = ctx.Employees.Where(x => x.PositionID == pID).Count();
                    if (ctr > 0)
                    {
                        System.Windows.MessageBox.Show("Position is being used by at least one employee. It cannot be deleted","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        Domain.Position h = ctx.Positions.Find(pID);
                        ctx.Positions.Remove(h);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Holiday has been successfuly deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();

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
