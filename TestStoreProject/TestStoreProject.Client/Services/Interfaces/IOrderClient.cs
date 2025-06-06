using Application.DTOs;

namespace TestStoreProject.Client.Services.Interfaces
{
    public interface IOrderClient
    {
        Task<int> CreateOrderAsync(CreateOrderRequest request);
        Task<List<OrderDto>> GetOrdersAsync();
    }
}
