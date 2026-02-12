using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Account;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/account")]
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,

            };
            
            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                if (roleResult.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                }
                else
                {
                    return BadRequest(roleResult.Errors);
                }
            }
            else
            {
                return BadRequest(createdUser.Errors);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(
            new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });

    }
}