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
using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfCollateralInfo.xaml
    /// </summary>
    public partial class wpfCollateralInfo : MetroWindow
    {
        public int UserID;
        public int lID;
        public int myNum;
        public TextBox[] textarray = new TextBox[0];
        public Label[] labelarray = new Label[0];
        public string[] myStr = new string[0];
        public wpfCollateralInfo()
        {
            InitializeComponent();
        }

        public void reset()
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    var lon = ctx.Loans.Find(lID);
                    myNum = ctx.CollateralInformations.Where(x => x.ServiceID == lon.ServiceID).Count();

                    textarray = new TextBox[myNum];
                    labelarray = new Label[myNum];
                    string[] myStr = new string[myNum];
                    StackPanel[] sp = new StackPanel[myNum];
                    stck.Children.Clear();
                    int ictr = 0;
                    var f = from x in ctx.CollateralInformations
                            where x.ServiceID == lon.ServiceID
                            select x;
                    foreach(var item in f)
                    {
                        labelarray[ictr] = new Label();
                        labelarray[ictr].Height = 30;
                        //labelarray[ctr].Width = 50;
                        labelarray[ictr].FontSize = 16;
                        //labelarray[ctr].Content = "Cheque No. " + (ctr + 1).ToString();
                        textarray[ictr] = new TextBox();
                        textarray[ictr].Height = 25;
                        textarray[ictr].Width = 300;
                        textarray[ictr].FontSize = 16;
                        textarray[ictr].MaxLength = 6;
                        sp[ictr] = new StackPanel();
                        sp[ictr].Width = 300;
                        sp[ictr].Height = 60;
                        sp[ictr].Children.Add(labelarray[ictr]);
                        sp[ictr].Children.Add(textarray[ictr]);
                        stck.Children.Add(sp[ictr]);
                        labelarray[ictr].Content = item.Field + ":";
                        myStr[ictr] = item.Field;
                        ictr++;
                    }
                    for (int ctr = 0; ctr < myNum; ctr++)
                    {
                        string str = myStr[ctr];
                        var cn = ctx.CollateralLoanInfoes.Where(x => x.LoanID == lID && x.CollateralInformation.Field == str).Count();
                        if (cn > 0)
                        {
                            var c = ctx.CollateralLoanInfoes.Where(x => x.LoanID == lID && x.CollateralInformation.Field == str).First();
                            textarray[ctr].Text = c.Value;
                        }
                    }
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
                reset();
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
                if (System.Windows.MessageBox.Show("Are you sure you want to save this information?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var ctx = new finalContext())
                    {
                        //System.Windows.MessageBox.Show(myStr.Count().ToString());
                        int ctr = 0;
                        var lon = ctx.Loans.Find(lID);
                        var f = from x in ctx.CollateralInformations
                                where x.ServiceID == lon.ServiceID
                                select x;
                        foreach (var item in f)
                        {
                            var cn = ctx.CollateralLoanInfoes.Where(x => x.LoanID == lID && x.CollateralInformationID == item.CollateralInformationID).Count();
                            if (cn > 0)
                            {
                                var c = ctx.CollateralLoanInfoes.Where(x => x.LoanID == lID && x.CollateralInformationID == item.CollateralInformationID).First();
                                c.Value = textarray[ctr].Text;
                            }
                            else
                            {
                                var c = ctx.CollateralInformations.Where(x => x.ServiceID == lon.ServiceID && x.Field == item.Field).First();
                                CollateralLoanInfo cli = new CollateralLoanInfo { LoanID = lID, CollateralInformationID = c.CollateralInformationID, Value = textarray[ctr].Text };
                                ctx.CollateralLoanInfoes.Add(cli);
                            }
                            ctr++;
                        }
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Information has been successfully saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
