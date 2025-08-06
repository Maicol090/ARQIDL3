using ARQIDL3.Models.DTOs;
using ARQIDL3.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFilteredProducts(
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? category,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        var filter = new ProductFilterDto
        {
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            CategoryId = category,
            Search = search,
            Page = page,
            Size = size
        };

        var result = await _service.GetFilteredProductsAsync(filter);
        return Ok(result);
    }

    [HttpGet("byIdStore")]
    public async Task<IActionResult> GetProductByIdAndStore(int id, string storeName)
    {
        var product = await _service.GetProductByIdAndStoreAsync(id, storeName);
        if (product == null) return NotFound();
        return Ok(product);
    }
}
