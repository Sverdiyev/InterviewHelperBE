namespace InterviewHelper.Core.Models;

public class UserDTO
{
    public DateTime CreationDate { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] Password { get; set; }
}