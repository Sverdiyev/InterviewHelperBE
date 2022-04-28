using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Helper;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InterviewHelper.Services.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly HashAlgorithm _sha;
    private readonly AppSettings _appSettings;

    public UserService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
    {
        _userRepository = userRepository;
        _sha = SHA256.Create();
        _appSettings = appSettings.Value;
    }

    public AuthenticateResponseDTO AddUser(UserDTO newUserDto)
    {
        newUserDto.Password = _sha.ComputeHash(newUserDto.Password);
        var newUser = new User(newUserDto);

        _userRepository.AddUser(newUser);

        var token = GenerateJwtToken(newUser);

        return new AuthenticateResponseDTO(newUser, token);
    }

    public void EditUser(User user)
    {
        _userRepository.EditUserDetails(user);
    }

    public AuthenticateResponseDTO AuthenticateUser(AuthenticateRequestDTO user)
    {
        var dbUser = _userRepository.GetUserWithDetails(user.Email, _sha.ComputeHash(user.Password));

        // return null if user not found
        if (dbUser == null) return null;

        // authentication successful so generate jwt token
        var token = GenerateJwtToken(dbUser);

        return new AuthenticateResponseDTO(dbUser, token);
    }

    public void CheckAuthority(ClaimsPrincipal authenticatedUser, int userId)
    {
        var dbUser = _userRepository.GetUserByEmail(authenticatedUser.FindFirst(ClaimTypes.Email).Value);

        if (dbUser.Id != userId)
        {
            throw new UnauthorizedOperation();
        }
    }

    // helper function to generate jwt token 
    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", user.Id.ToString())}),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}