using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BlogContext())
            {
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();
                try
                {
                    var blog = new Blog { Name = name };
                    db.Blogs.Add(blog);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                }

                var user = new User();
                var emp = new Employee();
                user.Employee = emp;

                var query = db.Blogs.Where(x => x.Name == "First Blog").Count();

                Console.WriteLine(query);
                Console.ReadKey();
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
       
    }
}
