using Application.DTOs;

namespace TestStoreProject.Client.Services.Interfaces
{
    public interface IUserClient
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserRoleAsync(int userId, string newRole);
        Task ToggleUserStatusAsync(int userId, bool isActive);
    }
}
