using InterviewHelper.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewHelper.DataAccess.Data
{
    public class InterviewHelperContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasMany(c => c.Tags).WithMany(c => c.Questions);
        }
        
        private readonly string _connectionString;

        public InterviewHelperContext()
        {
            _connectionString = "DataSource=../InterviewHelper.DataAccess/Data/app.db";
        }

        public InterviewHelperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Init()
        {
            Database.Migrate();
        }

        public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(_connectionString);

        
    }
}