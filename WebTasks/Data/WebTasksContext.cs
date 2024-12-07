using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTasks.Models;

namespace WebTasks.Data
{
    public class WebTasksContext : DbContext
    {
        internal IEnumerable<object> ToDoTasks;

        public WebTasksContext (DbContextOptions<WebTasksContext> options)
            : base(options)
        {
        }

        public DbSet<WebTasks.Models.ToDoTask> ToDoTask { get; set; } = default!;
    }
}
