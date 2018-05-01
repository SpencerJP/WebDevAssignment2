using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASPNETAssignment.Models;

namespace ASPNETAssignment.Data
{
    public class ASPNetAssignmentContext : DbContext
    {

        public ASPNetAssignmentContext(DbContextOptions<ASPNetAssignmentContext> options) : base(options)
        { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<OwnerInventory> OwnerInventory { get; set; }
        public DbSet<StockRequest> StockRequest { get; set; }
        public DbSet<StoreInventory> StoreInventory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });
        }
    }
}
