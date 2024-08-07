using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Apis.DTOs;
using OrderManagement.Apis.Errors;
using OrderManagement.Core.Services.Contracts;

namespace OrderManagement.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public UsersController(SignInManager<IdentityUser> signInManager ,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<AppUserDto>> Register(RegisterDto dto)
        {
            if (CheckEmailExists(dto.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "This Email is already request !"));
            var user = new IdentityUser()
            {
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.UserName,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            await _userManager.AddToRoleAsync(user, "Customer");//Default Role
            var returnedUser = new AppUserDto()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(returnedUser);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<AppUserDto>> Login(LoginDto dto)
        {
            var user =  await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new AppUserDto()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }





    }
}
