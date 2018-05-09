using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNETAssignment.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
            public DbSet<Product> Product { get; set; }
            public DbSet<Store> Store { get; set; }
            public DbSet<OwnerInventory> OwnerInventory { get; set; }
            public DbSet<StockRequest> StockRequest { get; set; }
            public DbSet<StoreInventory> StoreInventory { get; set; }
            public DbSet<ShoppingCart> ShoppingCart { get; set; }
            public DbSet<Item> Item { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });

            base.OnModelCreating(modelBuilder);
        }


    }
}
