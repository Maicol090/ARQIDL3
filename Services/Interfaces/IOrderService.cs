using ARQIDL3.Models.DTOs;

namespace ARQIDL3.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersAsync(int page, int size);
        Task<bool> MarkAsDeliveredAsync(int id);
        Task<bool> CreateOrderAsync(OrderCreateDto dto);
    }

}
