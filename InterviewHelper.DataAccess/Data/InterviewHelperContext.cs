using InterviewHelper.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace InterviewHelper.DataAccess.Data
{
    public class InterviewHelperContext : DbContext
    {
        private readonly string _connectionString;

        public InterviewHelperContext()
        {
            string currentDirectory = Directory.GetCurrentDirectory(); //API
            string dbPath = Directory.GetParent(currentDirectory).ToString();
            //check relative path 
            _connectionString = $"DataSource={dbPath}/InterviewHelper.DataAccess/Data/app.db";
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