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

    public AuthenticateUserResponse AddUser(UserRequest newUserRequest)
    {
        CheckIfEmailExists(newUserRequest.Email);

        var byteVersionPassword = Encoding.ASCII.GetBytes(newUserRequest.Password);

        newUserRequest.Password = Encoding.ASCII.GetString(_sha.ComputeHash(byteVersionPassword));

        var newUser = new User
        {
            CreationDate = DateTime.Now,
            Email = newUserRequest.Email,
            Name = newUserRequest.Name,
            Password = newUserRequest.Password
        };

        _userRepository.AddUser(newUser);

        var token = GenerateJwtToken(newUser);

        return new AuthenticateUserResponse
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Email = newUser.Email,
            Token = token
        };
    }

    public void EditUser(UserUpdateRequest user)
    {
        CheckIfEmailExists(user.Email);
        _userRepository.EditUserDetails(user);
    }

    public AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest user)
    {
        var dbUser =
            _userRepository.GetUserWithDetails(user.Email, _sha.ComputeHash(Encoding.ASCII.GetBytes(user.Password)));

        // return null if user not found
        if (dbUser == null)
        {
            return null;
        }

        // authentication successful so generate jwt token
        var token = GenerateJwtToken(dbUser);

        return new AuthenticateUserResponse
        {
            Id = dbUser.Id,
            Name = dbUser.Name,
            Email = dbUser.Email,
            Token = token
        };
    }

    public void CheckAuthority(ClaimsPrincipal authenticatedUser, int userId)
    {
        var dbUser = _userRepository.GetUserByEmail(authenticatedUser.FindFirst(ClaimTypes.Email).Value);

        if (dbUser.Id != userId)
        {
            throw new UnauthorizedOperationException();
        }
    }

    // helper function to generate jwt token 
    public string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var token = new JwtSecurityToken(null, null,
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void CheckIfEmailExists(string userEmail)
    {
        var userAlreadyExists = _userRepository.GetUserByEmail(userEmail);

        if (userAlreadyExists != null)
        {
            throw new UnauthorizedOperationException();
        }
    }
}