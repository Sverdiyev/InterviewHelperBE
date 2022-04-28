using System.ComponentModel.DataAnnotations;

namespace InterviewHelper.Core.Models.AuthenticationModels;

public class AuthenticateRequestDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public byte[] Password { get; set; }
}