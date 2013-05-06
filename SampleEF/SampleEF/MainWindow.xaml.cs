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
using System.Data.Entity;

namespace SampleEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                using (var ctx = new UserContext())
                { 
                  
                    var num = ctx.Users.Where(x => x.fName == txtFName.Text && x.lName == txtLName.Text).Count();
                    if (num > 0)
                    {
                       MessageBox.Show("User Already Exists");
                        return;
                    }
                    User user1 = new User { fName = txtFName.Text, lName = txtLName.Text };
                    Children child1 = new Children();
                    Children child2 = new Children();
                    child1.cId = 1;
                    child1.cName = txtChild1.Text;
                    child2.cId = 2;
                    child2.cName = txtChild2.Text;
                    ctx.Childrens.Add(child1);
                    ctx.Childrens.Add(child2);
                    ctx.Users.Add(user1);
                    ctx.SaveChanges();
                    MessageBox.Show("Successfully added");

                }
            //}catch (Exception ex)
            //{
             //   MessageBox.Show(ex.Message);
            //}
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class User
    {
        
        public int uId { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }

        public virtual ICollection<Children> Children { get; set; }
    }

    public class Children
    {
        public int cId { get; set; }
        public string cName { get; set; }

        public int uId { get; set; }
        public virtual User User { get; set; }
    }

    
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Children> Childrens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasKey(u => u.uId)
                .HasMany(c => c.Children);
            modelBuilder.Entity<Children>()
                .HasKey(c => c.cId);
        }
    }
}
