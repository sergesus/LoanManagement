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
using System.Windows.Forms;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;

using Microsoft.VisualBasic;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfHolidayInfo.xaml
    /// </summary>
    public partial class wpfHolidayInfo : MetroWindow
    {
        public string status;
        public int hId;
        public int UserID;

        public wpfHolidayInfo()
        {
            InitializeComponent();
        }

        public void checkNumeric(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                int res;
                String str = txt.Text;
                err = !int.TryParse(str, out res);

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {

                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter numeric values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkDouble(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                double res;
                String str = txt.Text;
                err = !double.TryParse(str, out res);

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter numeric values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkEmail(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                String str = txt.Text;
                str = str.Replace(" ", "");
                str = str.Trim();
                str = str.ToLower();
                txt.Text = str;
                if (!Regex.IsMatch(str, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                    err = true;

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter the correct email format(yourname@site.com)";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkName(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                String str = txt.Text;
                str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ");
                str = str.Trim();
                str = str.ToLower();
                str = Regex.Replace(str, "(?:^|\\s)\\w", new MatchEvaluator(delegate(Match m) { return m.Value.ToUpper(); }));
                txt.Text = str;
                if (!Regex.IsMatch(str, @"^[a-zA-Z ]*$") || str.Length > 25)
                    err = true;

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {

                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter alphabetic values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.ToolTip = "Correct Format";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                    lbl.FontWeight = FontWeights.Normal;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void checkString(System.Windows.Controls.TextBox txt, System.Windows.Controls.Label lbl, bool isRequired)
        {
            try
            {
                if (txt.Text == "")
                {
                    lbl.Content = "";
                    return;
                }
                bool err = false;
                String str = txt.Text;
                str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ");
                str = str.Trim();
                //str = str.ToLower();
                str = Regex.Replace(str, "(?:^|\\s)\\w", new MatchEvaluator(delegate(Match m) { return m.Value.ToUpper(); }));
                txt.Text = str;
                if (!Regex.IsMatch(str, @"^[a-zA-Z0-9 @.]*$"))
                    err = true;

                if (isRequired == true)
                {
                    if (String.IsNullOrWhiteSpace(str))
                        err = true;
                }

                if (err == true)
                {
                    lbl.Content = "?";
                    lbl.ToolTip = "Please enter alphanumeric & symbol values only";
                    lbl.Focus();
                    lbl.Foreground = System.Windows.Media.Brushes.Red;
                    lbl.FontWeight = FontWeights.ExtraBold;
                }
                else
                {
                    lbl.Content = "✔";
                    lbl.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void reset()
        {
            try
            {
                if (status == "View")
                {
                    using (var ctx = new finalContext())
                    {
                        Holiday h = ctx.Holidays.Find(hId);
                        isYearly.IsChecked = h.isYearly;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
                reset();
                return "";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                if (lblName.Content == "?" || lblDesc.Content == "?"
                    || String.IsNullOrWhiteSpace(txtName.Text)|| dt.SelectedDate.Value == null)
                {
                    System.Windows.MessageBox.Show("Please input correct format and/or fill all required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (status == "Add")
                {
                    using (var ctx = new finalContext())
                    {
                        //if (isYearly.IsChecked == true)
                        //{
                            var mC = ctx.MPaymentInfoes.Where(x => x.DueDate.Month == dt.SelectedDate.Value.Month && x.DueDate.Day == dt.SelectedDate.Value.Day).Count();
                            if (mC > 0)
                            {
                                System.Windows.MessageBox.Show("Payments on the given day will be automatically adjusted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                var ps = from x in ctx.MPaymentInfoes
                                         select x;

                                DateTime idt1 = dt.SelectedDate.Value;
                                DateTime idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt1);

                                bool isHoliday = true;
                                while (isHoliday == true || idt.Date.DayOfWeek.ToString() == "Saturday" || idt.Date.DayOfWeek.ToString() == "Sunday")
                                {
                                    if (idt.Date.DayOfWeek.ToString() == "Saturday")
                                    {
                                        idt = DateAndTime.DateAdd(DateInterval.Day, 2, idt);
                                    }
                                    else if (idt.Date.DayOfWeek.ToString() == "Sunday")
                                    {
                                        idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt);
                                    }
                                    var myC = ctx.Holidays.Where(x => x.Date.Month == idt.Date.Month && x.Date.Day == idt.Date.Day && x.isYearly == true).Count();
                                    if (myC > 0)
                                    {
                                        idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt);
                                        isHoliday = true;
                                    }
                                    else
                                    {
                                        myC = ctx.Holidays.Where(x => x.Date.Month == idt.Date.Month && x.Date.Day == idt.Date.Day && x.Date.Year == idt.Date.Year && x.isYearly == !true).Count();
                                        if (myC > 0)
                                        {
                                            idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt);
                                            isHoliday = true;
                                        }
                                        else
                                        {
                                            isHoliday = false;
                                        }
                                    }
                                }
                                foreach (var x in ps)
                                {
                                    if (isYearly.IsChecked == true)
                                    {
                                        if (x.DueDate.Month == idt1.Date.Month && x.DueDate.Day == idt1.Date.Day)
                                        {
                                            x.DueDate = idt;
                                        }
                                    }
                                    else
                                    {
                                        if (x.DueDate.Month == idt1.Date.Month && x.DueDate.Day == idt1.Date.Day && x.DueDate.Year == idt1.Date.Year)
                                        {
                                            x.DueDate = idt;
                                        }
                                    }
                                }
                            }
                       // }


                        var num = ctx.Holidays.Where(x => x.HolidayName == txtName.Text).Count();
                        if (num > 0)
                        {
                            System.Windows.MessageBox.Show("Holiday already exists", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        num = ctx.Holidays.Where(x => x.Date == dt.SelectedDate.Value).Count();
                        if (num > 0)
                        {
                            System.Windows.MessageBox.Show("Holiday already exists", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        Holiday h = new Holiday { HolidayName = txtName.Text, Date = dt.SelectedDate.Value, isYearly = Convert.ToBoolean(isYearly.IsChecked), Description = txtDesc.Text };

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Added Holiday " + txtName.Text };
                        ctx.AuditTrails.Add(at);

                        ctx.Holidays.Add(h);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Holiday has been successfully Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else
                {
                    using (var ctx = new finalContext())
                    {
                        var h = ctx.Holidays.Find(hId);
                        h.HolidayName = txtName.Text;
                        h.Date = dt.SelectedDate.Value;
                        h.isYearly = Convert.ToBoolean(isYearly.IsChecked);
                        h.Description = txtDesc.Text;

                        var mC = ctx.MPaymentInfoes.Where(x => x.DueDate.Month == dt.SelectedDate.Value.Month && x.DueDate.Day == dt.SelectedDate.Value.Day).Count();
                        if (mC > 0)
                        {
                            System.Windows.MessageBox.Show("Payments on the given day will be automatically adjusted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            var ps = from x in ctx.MPaymentInfoes
                                     select x;

                            DateTime idt1 = dt.SelectedDate.Value;
                            DateTime idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt1);

                            bool isHoliday = true;
                            while (isHoliday == true || idt.Date.DayOfWeek.ToString() == "Saturday" || idt.Date.DayOfWeek.ToString() == "Sunday")
                            {
                                if (idt.Date.DayOfWeek.ToString() == "Saturday")
                                {
                                    idt = DateAndTime.DateAdd(DateInterval.Day, 2, idt);
                                }
                                else if (idt.Date.DayOfWeek.ToString() == "Sunday")
                                {
                                    idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt);
                                }
                                var myC = ctx.Holidays.Where(x => x.Date.Month == idt.Date.Month && x.Date.Day == idt.Date.Day && x.isYearly == true).Count();
                                if (myC > 0)
                                {
                                    idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt);
                                    isHoliday = true;
                                }
                                else
                                {
                                    myC = ctx.Holidays.Where(x => x.Date.Month == idt.Date.Month && x.Date.Day == idt.Date.Day && x.Date.Year == idt.Date.Year && x.isYearly == !true).Count();
                                    if (myC > 0)
                                    {
                                        idt = DateAndTime.DateAdd(DateInterval.Day, 1, idt);
                                        isHoliday = true;
                                    }
                                    else
                                    {
                                        isHoliday = false;
                                    }
                                }
                            }
                            foreach (var x in ps)
                            {
                                if (isYearly.IsChecked == true)
                                {
                                    if (x.DueDate.Month == idt1.Date.Month && x.DueDate.Day == idt1.Date.Day)
                                    {
                                        x.DueDate = idt;
                                    }
                                }
                                else
                                {
                                    if (x.DueDate.Month == idt1.Date.Month && x.DueDate.Day == idt1.Date.Day && x.DueDate.Year == idt1.Date.Year)
                                    {
                                        x.DueDate = idt;
                                    }
                                }
                            }
                        }

                        AuditTrail at = new AuditTrail { EmployeeID = UserID, DateAndTime = DateTime.Now, Action = "Updated Holiday " + txtName.Text };
                        ctx.AuditTrails.Add(at);
                        ctx.SaveChanges();
                        System.Windows.MessageBox.Show("Holiday Successfully Updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }

            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new finalContext())
                {
                    DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        Holiday h = ctx.Holidays.Find(hId);
                        ctx.Holidays.Remove(h);
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

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtName,lblName,true);
        }

        private void txtDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            checkString(txtDesc, lblDesc, false);
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
                tbInfo.IsSelected = true;
                reset();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Runtime Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
