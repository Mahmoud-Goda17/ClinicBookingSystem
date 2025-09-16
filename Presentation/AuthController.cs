using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevice.Abstactions;
using Shared.IdentityDtos;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("login")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> LogIn([FromBody] LogInDto logInDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await service.LogInAsync(logInDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        [HttpPost("register")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await service.RegisterAsync(registerDto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        [HttpPost("addrole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await service.AddRoleAsync(model);
            if(!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }
        [HttpPost("refreshtoken")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh Token is missing");

            var result = await service.RefreshTokenAsync(refreshToken);
            if (!result.IsAuthenticated)
                return BadRequest(result);
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        [HttpPost("revoketoken")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenModel model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required");
            var result = await service.RevokeTokenAsync(token);
            if (!result)
                return NotFound("Token not found");
            return Ok("Token revoked");
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expireOn)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expireOn.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }   
    }
}
