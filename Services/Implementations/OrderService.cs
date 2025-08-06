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

        public async Task<List<OrderDto>> GetOrdersAsync(int page, int size)
        {
            var skip = (page - 1) * size;

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Skip(skip)
                .Take(size)
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<bool> MarkAsDeliveredAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null || order.Status == "Entregado") return false;

            order.Status = "Entregado";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateOrderAsync(OrderCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    OrderDate = DateTime.UtcNow,
                    Status = "Pendiente",
                    OrderDetails = new List<OrderDetail>()
                };

                decimal total = 0;

                foreach (var item in dto.Products)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null || product.Stock < item.Quantity)
                        throw new Exception("Producto inválido o sin stock");
                    var priceToUse = item.PriceDiscount != 0 ? item.PriceDiscount : item.Price;
                    var subtotal = priceToUse * item.Quantity;

                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        PriceDiscount = item.PriceDiscount,
                        Subtotal = subtotal
                    });

                    product.Stock -= item.Quantity;
                    total += subtotal;
                }

                order.Total = total;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Total = order.Total,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity,
                    Price = d.Price,
                    PriceDiscount = d.PriceDiscount,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
    }
}
