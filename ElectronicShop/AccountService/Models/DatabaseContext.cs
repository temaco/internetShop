using Microsoft.EntityFrameworkCore;
using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountService.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=ACER-PC\SQLEXPRESS;Initial Catalog=InternetShop;Integrated Security=True");
        }
    }
}