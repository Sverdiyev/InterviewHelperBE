using System.Text.Json.Serialization;

namespace InterviewHelper.Core.Models.AuthenticationModels;

public class AuthenticateUserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }


    public AuthenticateUserResponse(User user, string token)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Token = token;
    }
}