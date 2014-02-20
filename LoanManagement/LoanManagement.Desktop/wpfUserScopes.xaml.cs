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

using System.Data.Entity;
using LoanManagement.Domain;
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfUserScopes.xaml
    /// </summary>
    public partial class wpfUserScopes : MetroWindow
    {
        public int ID;
        public int UserID;
        public wpfUserScopes()
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

                using (var ctx = new finalContext())
                {
                    var sc = ctx.PositionScopes.Find(ID);
                    cClient.IsChecked = sc.MClient;
                    cAgents.IsChecked = sc.MAgent;
                    cServices.IsChecked = sc.MService;
                    cBanks.IsChecked = sc.MBank;
                    cEmployee.IsChecked = sc.MEmployee;
                    cPosition.IsChecked = sc.MPosition;
                    cHoliday.IsChecked = sc.MHoliday;
                    cOnlineReg.IsChecked = sc.MRegistration;

                    cApplication.IsChecked = sc.TApplication;
                    cApproval.IsChecked = sc.TApproval;
                    cReleasing.IsChecked = sc.TReleasing;
                    cPayments.IsChecked = sc.TPayments;
                    cManageClosed.IsChecked = sc.TManageClosed;
                    cRestructure.IsChecked = sc.TResturcture;
                    cAdjustment.IsChecked = sc.TPaymentAdjustment;
                    cOnlineApp.IsChecked = sc.TOnlineConfirmation;
                    cCollection.IsChecked = sc.TCollection;
                    cRenewal.IsChecked = sc.TRenewal;

                    cArchive.IsChecked = sc.UArchive;
                    cBackup.IsChecked = sc.UBackUp;
                    cUsers.IsChecked = sc.UUserAccounts;
                    cReports.IsChecked = sc.UReports;
                    cStatistics.IsChecked = sc.UStatistics;
                    cUserScopes.IsChecked = sc.UScopes;
                    cOnlineSettings.IsChecked = sc.UOnlineSettings;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnCont_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("Are you sure you want to save this record", "Question", MessageBoxButton.YesNo);
            if (mr == MessageBoxResult.Yes)
            {
                using (var ctx = new finalContext())
                {
                    var sc = ctx.PositionScopes.Find(ID);
                    sc.MClient = Convert.ToBoolean(cClient.IsChecked);
                    sc.MAgent = Convert.ToBoolean(cAgents.IsChecked);
                    sc.MService = Convert.ToBoolean(cServices.IsChecked);
                    sc.MBank = Convert.ToBoolean(cBanks.IsChecked);
                    sc.MEmployee = Convert.ToBoolean(cEmployee.IsChecked);
                    sc.MPosition = Convert.ToBoolean(cPosition.IsChecked);
                    sc.MHoliday = Convert.ToBoolean(cHoliday.IsChecked);
                    sc.MRegistration = Convert.ToBoolean(cOnlineReg.IsChecked);

                    sc.TApplication = Convert.ToBoolean(cApplication.IsChecked);
                    sc.TApproval = Convert.ToBoolean(cApproval.IsChecked);
                    sc.TReleasing = Convert.ToBoolean(cReleasing.IsChecked);
                    sc.TPayments = Convert.ToBoolean(cPayments.IsChecked);
                    sc.TManageClosed = Convert.ToBoolean(cManageClosed.IsChecked);
                    sc.TResturcture = Convert.ToBoolean(cRestructure.IsChecked);
                    sc.TPaymentAdjustment = Convert.ToBoolean(cAdjustment.IsChecked);
                    sc.TOnlineConfirmation = Convert.ToBoolean(cOnlineApp.IsChecked);
                    sc.TCollection = Convert.ToBoolean(cCollection.IsChecked);
                    sc.TRenewal = Convert.ToBoolean(cRenewal.IsChecked);

                    sc.UArchive = Convert.ToBoolean(cArchive.IsChecked);
                    sc.UBackUp = Convert.ToBoolean(cBackup.IsChecked);
                    sc.UUserAccounts = Convert.ToBoolean(cUsers.IsChecked);
                    sc.UReports = Convert.ToBoolean(cReports.IsChecked);
                    sc.UStatistics = Convert.ToBoolean(cStatistics.IsChecked);
                    sc.UScopes = Convert.ToBoolean(cUserScopes.IsChecked);
                    sc.UOnlineSettings = Convert.ToBoolean(cOnlineSettings.IsChecked);

                    ctx.SaveChanges();
                    MessageBox.Show("Receord has been successfully saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }
    }
}
