using Application.DTOs;

namespace TestStoreProject.Client.Services.Interfaces
{
    public interface ICartClient
    {
        Task<CartDto?> GetCartAsync();
        Task AddItemAsync(int productId, int quantity = 1);
        Task RemoveItemAsync(int productId);
        Task ClearAsync();
    }
}
