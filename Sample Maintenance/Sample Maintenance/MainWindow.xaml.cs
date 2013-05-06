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
using System.Windows.Navigation;
using System.Windows.Shapes;
//
using System.Windows.Forms;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Sample_Maintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        public string selectedFileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private static byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitMap = new Bitmap(fileName);
            ImageFormat bmpFormat = bitMap.RawFormat;
            var imageToConvert = System.Drawing.Image.FromFile(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, bmpFormat);
                return ms.ToArray();
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedFileName = dlg.FileName;
                //FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                img1.Source = bitmap;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(var ctx = new MyContext())
                {
                    var num=ctx.Employees.Where(x=> x.empId==txtEmpId.Text).Count();
                    if (num > 0)
                    {
                        System.Windows.MessageBox.Show("Employee ID already exists");
                        return;
                    }
                    if (txtEmpId.Text == "" || txtFName.Text == "" || txtLName.Text== "" || txtMName.Text=="")
                    {
                        System.Windows.MessageBox.Show("Please Complete the Information");
                        return;
                    }

                    Employee emp = new Employee { empId = txtEmpId.Text, FName = txtFName.Text, MI = txtMName.Text, LName = txtLName.Text, Photo = ConvertImageToByteArray(selectedFileName) };
                    ctx.Employees.Add(emp);
                    ctx.SaveChanges();
                    System.Windows.MessageBox.Show("Employee added successfully");
                    img1.Source = null;
                    resetGrid();
                }
            }
            catch(Exception ex)
            { 
            
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            resetGrid();
        }

        private void resetGrid()
        {
            dg1.IsReadOnly = true;
            
            try
            {
                List<Employee> list = new List<Employee>();
                using (var ctx = new MyContext())
                {
                    var Emp = from em in ctx.Employees
                              select em;
                    foreach (var item in Emp)
                    {
                        list.Add(item);
                    }
                    dg1.ItemsSource = ctx.Employees.ToList();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            object item = dg1.SelectedItem;
            string ID = (dg1.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            System.Windows.Forms.DialogResult r = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this record?","Question",MessageBoxButtons.YesNo);

            if (r == System.Windows.Forms.DialogResult.Yes)
            {
                using (var ctx = new MyContext())
                {
                    Employee emp = new Employee();
                    emp = ctx.Employees.Where(x => x.empId == ID).First();
                    ctx.Employees.Remove(emp);
                    ctx.SaveChanges();
                    System.Windows.MessageBox.Show("Employee successfully deleted");
                    resetGrid();
                }
            }
        }


    }

    public class Employee
    {
        public string empId { get; set; }
        public string FName { get; set; }
        public string MI { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        [MaxLength]
        public byte[] Photo { get; set; }
    }

    public class MyContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.empId);
        }
    }



}
