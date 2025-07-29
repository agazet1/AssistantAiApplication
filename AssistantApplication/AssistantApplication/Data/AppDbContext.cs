using Microsoft.EntityFrameworkCore;
using AssistantApplication.Data.Models;

namespace AssistantApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}