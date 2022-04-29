using System.ComponentModel.DataAnnotations;

namespace InterviewHelper.Core.Models.AuthenticationModels;

public class AuthenticateUserRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}