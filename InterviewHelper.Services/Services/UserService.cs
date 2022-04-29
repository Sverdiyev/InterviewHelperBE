using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Helper;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InterviewHelper.Services.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly HashAlgorithm _sha;
    private readonly string _secret;

    public UserService(IUserRepository userRepository, IOptions<AuthenticationSecret> configuration)
    {
        _userRepository = userRepository;
        _sha = SHA256.Create();
        _secret = configuration.Value.Secret;
    }

    public AuthenticateResponseDTO AddUser(UserDTO newUserDto)
    {
        var potentialDbUser = _userRepository.GetUserByEmail(newUserDto.Email);

        if (potentialDbUser != null)
        {
            throw new UnauthorizedOperation();
        }

        var byteVersionPassword = Encoding.ASCII.GetBytes(newUserDto.Password);
        
        newUserDto.Password = Encoding.ASCII.GetString(_sha.ComputeHash(byteVersionPassword));
        
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
        var dbUser = _userRepository.GetUserWithDetails(user.Email, _sha.ComputeHash(Encoding.ASCII.GetBytes(user.Password)));
        
        // return null if user not found
        if (dbUser == null)
        {
            return null;
        }

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
    public string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };    
    
        var token = new JwtSecurityToken("Devbridge.com","Devbridge.com",
            claims,    
            expires: DateTime.Now.AddMinutes(120),    
            signingCredentials: credentials);    
        
        return new JwtSecurityTokenHandler().WriteToken(token);  
    }
}