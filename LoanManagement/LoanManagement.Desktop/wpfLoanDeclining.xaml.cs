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

using System.Net;
using System.Net.Mail;
using System.Data.Entity;
using LoanManagement.Domain;
using MahApps.Metro.Controls;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfLoanDeclining.xaml
    /// </summary>
    public partial class wpfLoanDeclining : MetroWindow
    {
        public int lId;
        public int UserID;
        public string status;
        public wpfLoanDeclining()
        {
            InitializeComponent();
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = "";
                if (cb1.IsChecked == true)
                {
                    str = str + cb1.Content + "\n";
                }

                if (cb2.IsChecked == true)
                {
                    str = str + cb2.Content + "\n";
                }

                if (cb3.IsChecked == true)
                {
                    str = str + cb3.Content + "\n";
                }

                if (cbOthers.IsChecked == true)
                {
                    str = str + txtOthers.Text + "\n";
                }
                //MessageBox.Show(str);

                if (cb1.IsChecked == false && cb2.IsChecked == false && cb3.IsChecked == false && cbOthers.IsChecked == false)
                {
                    MessageBox.Show("Select at least one(1) reason", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lId);
                    lon.Status = "Declined";
                    if (status == "Renewal")
                    {
                        var rn = ctx.LoanRenewals.Where(x => x.newLoanID == lId).First();
                        rn.Status = "Declined";
                    }
                    DeclinedLoan dl = new DeclinedLoan { DateDeclined = DateTime.Today.Date, Reason = str };
                    lon.DeclinedLoan = dl;
                    ctx.SaveChanges();
                    MessageBox.Show("Loan has been successfully declined", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    try
                    {
                        if (cbSend.IsChecked == true)
                        {
                            var cont = ctx.ClientContacts.Where(x => x.ClientID == lon.ClientID && x.Primary == true).First();

                            string con = cont.Contact;
                            MailMessage msg = new MailMessage();
                            msg.To.Add(con + "@m2m.ph");
                            msg.From = new MailAddress("aldrinarciga@gmail.com"); //See the note afterwards...
                            msg.Body = "We are sorry to inform you that your loan application(" + lon.Service.Name + ") has been delined because of the following reason(s):\n\n" + str + "";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.Credentials = new NetworkCredential("aldrinarciga@gmail.com", "312231212131");
                            smtp.Send(msg);
                            MessageBox.Show("Message successfuly sent", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    this.Close();
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
