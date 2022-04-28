namespace InterviewHelper.Core.Models.AuthenticationModels;

public class AuthenticateResponseDTO
{
    private int Id { get; set; }
    private string Name { get; set; }
    private string Email { get; set; }
    private string Token { get; set; }


    public AuthenticateResponseDTO(User user, string token)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Token = token;
    }
}