using System.Text.Json.Serialization;

namespace InterviewHelper.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string Password { get; set; }

        public User()
        {
        }

        // custom DTO constructor
        public User(UserDTO user)
        {
            this.CreationDate = DateTime.Now;
            this.Name = user.Name;
            this.Email = user.Email;
            this.Password = user.Password;
        }
    }
}