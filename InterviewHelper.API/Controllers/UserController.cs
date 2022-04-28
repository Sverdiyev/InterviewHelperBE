using System.Net;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;
using InterviewHelper.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;
        
        public UserController(ILogger<UserController> logger,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        
        // add user
        [HttpPost("add-user")]
        public IActionResult AddUser(UserDTO newUser)
        {
            try
            {
                return Ok(_userService.AddUser(newUser));
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        // edit user
        [HttpPut("edit-user")]
        public IActionResult EditUser(User user)
        {
            var currentUser = this.User;
            
            try
            {
                // throws exception if the authenticated user is not the one editing
                _userService.CheckAuthority(currentUser, user.Id);  
                
                _userService.EditUser(user);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequestDTO user)
        {
            var response = _userService.AuthenticateUser(user);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        
    }

   
}
