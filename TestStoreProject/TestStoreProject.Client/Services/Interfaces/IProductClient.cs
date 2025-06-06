using Application.DTOs;

namespace TestStoreProject.Client.Services.Interfaces
{
    public interface IProductClient
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
    }
}
