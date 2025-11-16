using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeTask.Models;

namespace TreeTask.Services
{
    public class ConsoleTreePrinter
    {
        public void Print(List<Category> nodes, int level = 0)
        {
            foreach (var node in nodes)
            {
                Console.WriteLine($"{new string(' ', level * 2)}- {node.Name}");
                Print(node.Children, level + 1);
            }
        }
    }
}
