using InterviewHelper.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace InterviewHelper.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


                if (environment != "Production")
                {
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string dbPath = Directory.GetParent(currentDirectory).ToString();
                    optionsBuilder.UseSqlite($"DataSource={dbPath}/InterviewHelper.DataAccess/Data/app.db");
                }
            }
        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}