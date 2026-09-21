using Microsoft.EntityFrameworkCore;
using c.Models;

namespace c.Data
{
   public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options) 
      {}  

      public DbSet<TaskItem> TaskItems {get; set;}

    } 
}
