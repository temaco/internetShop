using Microsoft.EntityFrameworkCore;
using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopEngine.Models
{
    public class DatabaseContextEngine : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionGoods> TransactionGoods { get; set; }
        public DbSet<Adress> Adresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=ACER-PC\SQLEXPRESS;Initial Catalog=InternetShop;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}