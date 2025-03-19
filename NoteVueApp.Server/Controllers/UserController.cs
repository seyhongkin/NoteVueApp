using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;
using NoteVueApp.Server.Exceptions;
using NoteVueApp.Server.Services;

namespace NoteVueApp.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        public UserController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("Verify")]
        public async Task<IActionResult> VerifyToken()
        {
            string token = _jwtTokenService.ExtractTokenFromHeader(Request.Headers);
            UserResourceDTO? user = await _userService.VerifyToken(token);
            if (user == null)
            {
                return BadRequest("Invalid token");
            }
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            try
            {
                var result = await _userService.AddUser(userDto);
                return Ok(result);
            }
            catch (ResourceDuplicateException ex) {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _userService.Login(loginDto);
            if (result == null)
            {
                return Forbid();
            }
            return Ok(result);
        }
    }
}
