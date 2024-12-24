using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser
            {
                Email = registerRequestDto.Email,
                UserName = registerRequestDto.UserName
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerRequestDto.Password);
            if (!createdUser.Succeeded)
            {
                var errors = createdUser.Errors.Select(e => e.Description);
                return StatusCode(500, new { Errors = errors });
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description);
                return StatusCode(500, new { Errors = errors });
            };

            return CreatedAtAction(nameof(Register), new { id = appUser.Id }, "User created successfully");

        }
    }
}