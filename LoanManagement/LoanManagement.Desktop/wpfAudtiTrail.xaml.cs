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

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfAudtiTrail.xaml
    /// </summary>
    public partial class wpfAudtiTrail : MetroWindow
    {
        public wpfAudtiTrail()
        {
            InitializeComponent();
        }

        private void rg()
        {
            using (var ctx = new iContext())
            {
                var adt = from ad in ctx.AuditTrails
                          //where ad.DateAndTime == dt.SelectedDate
                          select new { Action = ad.Employee.FirstName + " " + ad.Employee.MI + " " + ad.Employee.LastName + " -> " + ad.Action, DateAndTime = ad.DateAndTime};
                dg.ItemsSource = adt.ToList();
            }
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(
                new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Icons\\bg5.png"));
            myBrush.ImageSource = image.Source;
            //Grid grid = new Grid();
            wdw1.Background = myBrush;
            dt.SelectedDate = DateTime.Now.Date;
            rg();
        }

        private void DatePicker_CalendarClosed_1(object sender, RoutedEventArgs e)
        {
            rg();
        }

        private void dt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rg();
        }
    }
}
