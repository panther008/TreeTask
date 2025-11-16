using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTask.Models;

namespace TreeTask.Services
{
    public class CategoryTreeSpService : ICategoryTreeService
    {
        private readonly string _connectionString;

        public CategoryTreeSpService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Category>> BuildTreeAsync()
        {
            var sw = Stopwatch.StartNew();

            var dt = new DataTable();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("GetCategoryTree", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            dt.Load(await cmd.ExecuteReaderAsync());

            var categories = dt.AsEnumerable()
                .Select(r => new Category
                {
                    Id = r.Field<Guid>("Id"),
                    Name = r.Field<string>("Name"),
                    ParentId = r.Field<Guid?>("ParentId")
                }).ToList();

            var tree = BuildTree(categories);

            sw.Stop();
            Console.WriteLine($"Stored Procedure Tree Build Time: {sw.ElapsedMilliseconds}ms");

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
