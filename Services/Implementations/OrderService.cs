using ARQIDL3.Data;
using ARQIDL3.Models.DTOs;
using ARQIDL3.Models.Entities;
using ARQIDL3.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ARQIDL3.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return null;

            return MapToDto(order);
        }
        public async Task<PagedResponse<OrderDto>> GetOrdersAsync(int page, int size)
        {
            var skip = (page - 1) * size;

            var totalElements = await _context.Orders.CountAsync();

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Skip(skip)
                .Take(size)
                .ToListAsync();
            var totalPages = (int)Math.Ceiling(totalElements / (double)size);

            return new PagedResponse<OrderDto>
            {
                Content = orders.Select(MapToDto).ToList(),
                TotalElements = totalElements,
                Number = page,
                Size = size,
                TotalPages = totalPages
            };
        }

        public async Task<bool> MarkAsDeliveredAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null || order.Status == "Entregado") return false;

            order.Status = "Entregado";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto dto)
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Status = "Pendiente",
                Total = 0,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var item in dto.Products)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);

                if (product == null)
                    throw new Exception($"Producto con ID {item.ProductId} no encontrado.");

                var finalPrice = product.PriceDiscount > 0 ? product.PriceDiscount : product.Price;

                var detail = new OrderDetail
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Price = finalPrice, 
                    Subtotal = finalPrice * item.Quantity
                };

                order.Total += detail.Subtotal;

                product.Stock -= item.Quantity; 

                order.OrderDetails.Add(detail);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }


        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.OrderId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Total = order.Total,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    Price = d.Price,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
    }
}
