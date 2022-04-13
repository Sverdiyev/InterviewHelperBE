using InterviewHelper.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace InterviewHelper.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string dbPath = Directory.GetParent(currentDirectory).ToString();
            optionsBuilder.UseSqlite($"DataSource={dbPath}/InterviewHelper.DataAccess/Data/app.db");
        }
        
        public DbSet<AppUser> AppUsers { get; set; }

    }
}