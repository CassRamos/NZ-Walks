using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = requestDTO.Username,
                Email = requestDTO.Email,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, requestDTO.Password);

            if (identityResult.Succeeded)
            {
                if (requestDTO.Roles is not null && requestDTO.Roles.Any())
                {

                    identityResult = await _userManager.AddToRolesAsync(identityUser, requestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, request.Password);

                if (checkPasswordResult)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {

                        var token = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = token,
                        };

                        return Ok(response);
                    }

                }
            }

            return BadRequest("The email or password provided is incorrect");
        }

    }
}
