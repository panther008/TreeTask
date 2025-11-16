using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTask.Models;

namespace TreeTask.Data
{
    public static class DbInitializer
    {
        public static void EnsureDbCreated(AppDbContext db)
        {
            db.Database.EnsureCreated();
            SeedData(db);
        }

        private static void SeedData(AppDbContext db)
        {
            // Check if data already exists
            if (db.Categories.Any())
                return;

            // Create at least 4 levels of category hierarchy
            var electronics = new Category { Id = Guid.NewGuid(), Name = "Electronics", ParentId = null };
            db.Categories.Add(electronics);

            var computers = new Category { Id = Guid.NewGuid(), Name = "Computers", ParentId = electronics.Id };
            db.Categories.Add(computers);

            var laptops = new Category { Id = Guid.NewGuid(), Name = "Laptops", ParentId = computers.Id };
            db.Categories.Add(laptops);

            var gamingLaptops = new Category { Id = Guid.NewGuid(), Name = "Gaming Laptops", ParentId = laptops.Id };
            db.Categories.Add(gamingLaptops);

            var ultrabooks = new Category { Id = Guid.NewGuid(), Name = "Ultrabooks", ParentId = laptops.Id };
            db.Categories.Add(ultrabooks);

            var desktops = new Category { Id = Guid.NewGuid(), Name = "Desktops", ParentId = computers.Id };
            db.Categories.Add(desktops);

            var phones = new Category { Id = Guid.NewGuid(), Name = "Phones", ParentId = electronics.Id };
            db.Categories.Add(phones);

            var smartphones = new Category { Id = Guid.NewGuid(), Name = "Smartphones", ParentId = phones.Id };
            db.Categories.Add(smartphones);

            var android = new Category { Id = Guid.NewGuid(), Name = "Android", ParentId = smartphones.Id };
            db.Categories.Add(android);

            var flagships = new Category { Id = Guid.NewGuid(), Name = "Flagships", ParentId = android.Id };
            db.Categories.Add(flagships);

            var budget = new Category { Id = Guid.NewGuid(), Name = "Budget", ParentId = android.Id };
            db.Categories.Add(budget);

            var ios = new Category { Id = Guid.NewGuid(), Name = "iOS", ParentId = smartphones.Id };
            db.Categories.Add(ios);

            var clothing = new Category { Id = Guid.NewGuid(), Name = "Clothing", ParentId = null };
            db.Categories.Add(clothing);

            var mens = new Category { Id = Guid.NewGuid(), Name = "Men's", ParentId = clothing.Id };
            db.Categories.Add(mens);

            var shirts = new Category { Id = Guid.NewGuid(), Name = "Shirts", ParentId = mens.Id };
            db.Categories.Add(shirts);

            var casual = new Category { Id = Guid.NewGuid(), Name = "Casual Shirts", ParentId = shirts.Id };
            db.Categories.Add(casual);

            var formal = new Category { Id = Guid.NewGuid(), Name = "Formal Shirts", ParentId = shirts.Id };
            db.Categories.Add(formal);

            db.SaveChanges();
        }
    }
}
