namespace InterviewHelper.Core.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}