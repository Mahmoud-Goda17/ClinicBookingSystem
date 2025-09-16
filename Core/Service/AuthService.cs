using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sevice.Abstactions;
using Shared.IdentityDtos;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,IOptions<JwtOptions> options) : IAuthService
    {

        public async Task<UserResultDto> LogInAsync(LogInDto logInDto)
        {
            var userResult = new UserResultDto();
            var user = await userManager.FindByEmailAsync(logInDto.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, logInDto.Password))
            {
                userResult.IsAuthenticated = false;
                userResult.Message = "No user with this email";
                return userResult;
            }
            var jwtToken = await CreateToken(user);
            var roles = await userManager.GetRolesAsync(user);
            userResult.IsAuthenticated = true;
            userResult.Email = user.Email;
            userResult.DisplayName = user.DisplayName;
            userResult.Roles = roles.ToList();
            userResult.Token = jwtToken;
            if(user.RefreshTokens.Any(RT=>RT.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(RT => RT.IsActive);
                userResult.RefreshToken = activeRefreshToken.Token;
                userResult.RefreshTokenExpiration = activeRefreshToken.ExpireOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                userResult.RefreshToken = refreshToken.Token;
                userResult.RefreshTokenExpiration = refreshToken.ExpireOn;
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }
            return userResult;
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = await userManager.FindByEmailAsync(registerDto.Email);
            if (user is not null)
                return new UserResultDto
                {
                    IsAuthenticated = false,
                    Message = "Email is already registered"
                };
            user = await userManager.FindByNameAsync(registerDto.UserName);
            if (user is not null)
                return new UserResultDto
                {
                    IsAuthenticated = false,
                    Message = "Username is already registered"
                };
            var newUser = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await userManager.CreateAsync(newUser, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new UserResultDto
                {
                    IsAuthenticated = false,
                    Message = errors
                };
            }
            await userManager.AddToRoleAsync(newUser, "User");
            var jwttoken = await CreateToken(newUser);
            var userResult = new UserResultDto
            {
                IsAuthenticated = true,
                Email = newUser.Email,
                DisplayName = newUser.DisplayName,
                Roles = new List<string> { "User" },
                Token = jwttoken
                
            };
             var refreshToken = GenerateRefreshToken();
             userResult.RefreshToken = refreshToken.Token;
             userResult.RefreshTokenExpiration = refreshToken.ExpireOn;
             user.RefreshTokens.Add(refreshToken);
             await userManager.UpdateAsync(user);

            return userResult;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user is null || !await roleManager.RoleExistsAsync(model.Role))
                return "Invalid UserId Or Role";

            var role= await userManager.IsInRoleAsync(user, model.Role);
            if (role)
                return "User Already In This Role";

            var result = await userManager.AddToRoleAsync(user, model.Role);
            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        public async Task<UserResultDto> RefreshTokenAsync(string token)
        {
            var userResult = new UserResultDto();
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user is null)
            {
                userResult.IsAuthenticated = false;
                userResult.Message = "Invalid Token";
                return userResult;
            }
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                userResult.IsAuthenticated = false;
                userResult.Message = "Inactive Token";
                return userResult;
            }
            refreshToken.RevokeOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            var jwtToken = await CreateToken(user);
            var roles = await userManager.GetRolesAsync(user);
            userResult.IsAuthenticated = true;
            userResult.Email = user.Email;
            userResult.DisplayName = user.DisplayName;
            userResult.Roles = roles.ToList();
            userResult.Token = jwtToken;
            userResult.RefreshToken = newRefreshToken.Token;
            userResult.RefreshTokenExpiration = newRefreshToken.ExpireOn;
            return userResult;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null) return false;
            var refreshtoken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshtoken.IsActive) return false;
            refreshtoken.RevokeOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
            return true;
        }

        private async Task<string> CreateToken(AppUser user)
        {
            var jwtSettings = options.Value;
            var userClaims = await userManager.GetClaimsAsync(user);
            var authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            authclaims.AddRange(userClaims);
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authclaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: authclaims,
                expires: DateTime.Now.AddDays(jwtSettings.DurationInDays),
                signingCredentials: signinCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            RandomNumberGenerator.Fill(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }

    }
}
