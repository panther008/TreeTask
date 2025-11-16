using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTask.Data;
using TreeTask.Models;

namespace TreeTask.Services
{
    public class CategoryTreeEfService : ICategoryTreeService
    {
        private readonly AppDbContext _db;

        public CategoryTreeEfService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> BuildTreeAsync()
        {
            var sw = Stopwatch.StartNew();

            var categories = await _db.Categories.AsNoTracking().ToListAsync();
            var tree = BuildTree(categories);

            sw.Stop();
            Console.WriteLine($"EF Tree Build Time: {sw.ElapsedMilliseconds}ms");

            return tree;
        }

        private List<Category> BuildTree(List<Category> categories)
        {
            var lookup = categories.ToLookup(c => c.ParentId);

            List<Category> Build(Guid? parentId)
            {
                return lookup[parentId]
                    .Select(c =>
                    {
                        c.Children = Build(c.Id);
                        return c;
                    }).ToList();
            }

            return Build(null);
        }
    }
}
