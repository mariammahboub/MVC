using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data
{
    public class MVCSession02DbContext : IdentityDbContext<ApplicationUser>
    {
        public MVCSession02DbContext(DbContextOptions<MVCSession02DbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    =>optionsBuilder.UseSqlServer("Server = . ; Database = MVCSession02Db; Trusted_Connection = true; MultipleActiveResultSets = true");

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityUser<int>> MyProperty { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
    }
}
