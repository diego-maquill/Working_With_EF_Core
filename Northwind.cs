using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packt.Shared
{
    // this manages the connection to the database
    public class Northwind : DbContext
    {
        // these properties map to tables in the database 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(
          DbContextOptionsBuilder optionsBuilder)
        {
            string path = System.IO.Path.Combine(
              System.Environment.CurrentDirectory, "Northwind.db");
            //optionsBuilder.UseSqlite($"Filename={path}");
            optionsBuilder.UseLazyLoadingProxies().UseSqlite($"Filename={path}");
        }

        protected override void OnModelCreating(
          ModelBuilder modelBuilder)
        {
            // example of using Fluent API instead of attributes
            // to limit the length of a category name to under 15
            modelBuilder.Entity<Category>()
              .Property(category => category.CategoryName)
              .IsRequired() // NOT NULL
              .HasMaxLength(15);

            // global filter to remove discontinued products
            modelBuilder.Entity<Product>()
              .HasQueryFilter(p => !p.Discontinued);

            modelBuilder.Entity<Product>()
            .Property(e => e.Cost)
            .HasConversion<double>();
        }
    }
}