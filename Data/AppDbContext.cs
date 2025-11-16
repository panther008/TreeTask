using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTask.Models;

namespace TreeTask.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.ParentId).IsRequired(false);
                
                // Ignore the Children property - it's only used for in-memory tree building
                entity.Ignore(e => e.Children);
            });
        }
    }
}
