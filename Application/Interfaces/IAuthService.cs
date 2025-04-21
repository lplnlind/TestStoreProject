using Application.DTOs.Auth;
using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<UserProfileDto?> GetProfileAsync(ClaimsPrincipal user);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ValidatePasswordAsync(string password, AuthUserDto user);
        string GenerateToken(JwtUserDto user);
    }
}
