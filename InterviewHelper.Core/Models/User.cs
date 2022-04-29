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
        
        public User(UserRequest userRequest)
        {
            this.CreationDate = DateTime.Now;
            this.Name = userRequest.Name;
            this.Email = userRequest.Email;
            this.Password = userRequest.Password;
        }
    }
}