using InterviewHelper.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewHelper.DataAccess.Data
{
    public class InterviewHelperContext : DbContext
    {
        private readonly string _connectionString;

        public InterviewHelperContext()
        {
            _connectionString =
                "Server=20.86.255.178;Database=InterviewHelperStaging;User Id=sa;Password=763594HAZS28LQxc;";
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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Favourite> Favourites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_connectionString);
    }
}