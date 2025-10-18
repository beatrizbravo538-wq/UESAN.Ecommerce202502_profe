using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UESAN.Ecommerce.CORE.Core.DTOs;
using UESAN.Ecommerce.CORE.Core.Interfaces;

namespace UESAN.Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserCreateDTO userCreateDTO)
        {
            if (userCreateDTO == null)
                return BadRequest();

            try
            {
                var newId = await _userService.SignUp(userCreateDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = newId }, userCreateDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDTO userSignInDTO)
        {
            if (userSignInDTO == null)
                return BadRequest(new { message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(userSignInDTO.Email) || string.IsNullOrWhiteSpace(userSignInDTO.Password))
                return BadRequest(new { message = "Email and Password are required." });

            try
            {
                var user = await _userService.SignIn(userSignInDTO);
                if (user == null)
                    return Unauthorized(new { message = "Invalid email or password." });

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            if (userCreateDTO == null) return BadRequest();
            var newId = await _userService.InsertUser(userCreateDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = newId }, userCreateDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }
    }
}
