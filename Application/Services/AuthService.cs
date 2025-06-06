using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Setting;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository,
            IOptions<JwtSettings> jwtOptions,
            IPasswordHasher<User> passwordHasher,
            IUserContextService userContextService)
        {
            _jwtSettings = jwtOptions.Value;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.UserName);
            if (existingUser is not null)
                throw new Exception("نام کاربری قبلاً ثبت شده است.");

            var hashedPassword = _passwordHasher.HashPassword(new User(), request.Password);

            var user = new User
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Email = new Email(request.Email),
                PasswordHash = hashedPassword,
                Role = UserRole.Customer, // پیش‌فرض
                Address = new Address("نامشخص", "نامشخص", "0000")
            };

            await _userRepository.AddAsync(user);

            var token = GenerateToken(new JwtUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email.Value,
                Role = user.Role.ToString()
            });

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserName);

            if (user == null)
                throw new Exception("نام کاربری یا رمز عبور اشتباه است.");

            if (!await ValidatePasswordAsync(request.Password, user.UserName))
                throw new Exception("نام کاربری یا رمز عبور اشتباه است.");

            var jwtUser = new JwtUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email.Value,
                Role = user.Role.ToString()
            };

            var token = GenerateToken(jwtUser);

            return new AuthResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }

        public async Task<UserProfileDto?> GetProfileAsync(ClaimsPrincipal user)
        {
            var username = user.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username))
                return null;

            var dbUser = await _userRepository.GetByUsernameAsync(username);

            if (dbUser == null)
                return null;

            return new UserProfileDto
            {
                FullName = dbUser.FullName,
                UserName = dbUser.UserName,
                Email = dbUser.Email.Value,
                Role = dbUser.Role.ToString(),
                Street = dbUser.Address?.Street ?? "",
                City = dbUser.Address?.City ?? "",
                ZipCode = dbUser.Address?.ZipCode ?? ""
            };
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            if (user == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return false;

            return true;
        }

        public string GenerateToken(JwtUserDto user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task UpdateProfileAsync(UpdateProfileRequest request)
        {
            var user = await _userRepository.GetByIdAsync(_userContextService.GetUserId());
            if (user is null)
                throw new Exception("کاربر یافت نشد");

            var address = new Address(request.Address.Street, request.Address.City, request.Address.ZipCode);
            user.UpdateProfile(request.FullName, request.Email, address); // متد دامین
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(_userContextService.GetUserId());
            if (user is null)
                throw new Exception("کاربر یافت نشد");


            if (!await ValidatePasswordAsync(request.CurrentPassword, user.UserName))
                throw new Exception("رمز فعلی نادرست است");

            user.ChangePassword(_passwordHasher.HashPassword(user, request.NewPassword)); // متد دامین
            await _userRepository.UpdateAsync(user);
        }
    }
}
