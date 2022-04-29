using System.Text.Json.Serialization;

namespace InterviewHelper.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {
        }
    }
}