using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserCreateDTO userCreateDTO)
        {
            if (userCreateDTO == null)
                return BadRequest();

            try
            {
                var newUserId = await _userService.SignUp(userCreateDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, userCreateDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDTO userSignInDTO)
        {
            if (userSignInDTO == null)
                return BadRequest();

            var user = await _userService.SignIn(userSignInDTO.Email, userSignInDTO.Password);
            if (user == null)
                return Unauthorized();

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
