using ARQIDL3.Models.DTOs;
using ARQIDL3.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _service.GetOrderByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var orders = await _service.GetOrdersAsync(page, size);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
    {
        var success = await _service.CreateOrderAsync(dto);
        if (!success) return BadRequest("Error al crear el pedido. Verifica stock o datos.");
        return Ok(new { message = "Pedido creado con éxito" });
    }

    [HttpPut("Entregado/{id}")]
    public async Task<IActionResult> MarkOrderAsDelivered(int id)
    {
        var success = await _service.MarkAsDeliveredAsync(id);
        if (!success) return NotFound("Pedido no encontrado o ya entregado");
        return Ok(new { message = "Pedido marcado como entregado" });
    }
}
