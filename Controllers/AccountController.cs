using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [Route("api/v{apiVersion:apiVersion}/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(user =>
                user.Email == loginRequestDto.Email.ToLower());

            if (user == null) return Unauthorized("Invalid email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Incorrect email or password");

            return Ok(new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("register")]
        [SwaggerResponse(StatusCodes.Status200OK, "Created", typeof(NewUserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation Error Occured", typeof(void))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(void))]
        public async Task<IActionResult> Register([FromBody] RegisterAccountRequestDto registerAccountRequestDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerAccountRequestDto.Username,
                    Email = registerAccountRequestDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerAccountRequestDto.Password);

                if (!createdUser.Succeeded) return StatusCode(500, createdUser.Errors);
                
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                return roleResult.Succeeded ? Ok(new NewUserDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser)
                }) : StatusCode(500, createdUser.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
