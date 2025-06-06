using Application.DTOs;

namespace TestStoreProject.Client.Services.Interfaces
{
    public interface ICategoryClient
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task CreateAsync(CategoryDto dto);
        Task UpdateAsync(CategoryDto dto);
        Task DeleteAsync(int id);
    }
}
