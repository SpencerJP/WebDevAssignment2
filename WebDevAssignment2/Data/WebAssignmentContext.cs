using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDevAssignment2.Models;

namespace WebDevAssignment2.Data
{
    public class WebAssignmentContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<OwnerInventory> OwnerInventory { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }

        public WebAssignmentContext(DbContextOptions<WebAssignmentContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });
        }
    }
}
