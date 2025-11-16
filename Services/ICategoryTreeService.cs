using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTask.Models;

namespace TreeTask.Services
{
    public interface ICategoryTreeService
    {
        Task<List<Category>> BuildTreeAsync();
    }
}
