using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.IdentityDtos;

namespace Sevice.Abstactions
{
    public interface IAuthService
    {
        Task<UserResultDto> LogInAsync(LogInDto logInDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<UserResultDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
