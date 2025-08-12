using ARQIDL3.Models.DTOs;
using ARQIDL3.Models.Entities;

namespace ARQIDL3.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<PagedResponse<OrderDto>> GetOrdersAsync(int page, int size);
        Task<bool> MarkAsDeliveredAsync(int id);
        Task<Order> CreateOrderAsync(OrderCreateDto dto);
    }

}
