using ARQIDL3.Data;
using ARQIDL3.Models.DTOs;
using ARQIDL3.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ARQIDL3.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filter)
        {
            var query = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .AsQueryable();

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(p => p.Name.Contains(filter.Search));

            var skip = (filter.Page - 1) * filter.Size;

            var products = await query
                .Skip(skip)
                .Take(filter.Size)
                .ToListAsync();

            return products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PriceDiscount = p.PriceDiscount,
                Stock = p.Stock,
                BrandName = p.BrandName,
                StoreName = p.StoreName,
                CategoryName = p.Category.CategoryName,
                ImageUrls = p.Images.Select(i => i.Url).ToList()
            }).ToList();
        }

        public async Task<ProductDto?> GetProductByIdAndStoreAsync(int id, string storeName)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.StoreName == storeName);

            if (product == null) return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PriceDiscount = product.PriceDiscount,
                Stock = product.Stock,
                BrandName = product.BrandName,
                StoreName = product.StoreName,
                CategoryName = product.Category.CategoryName,
                ImageUrls = product.Images.Select(i => i.Url).ToList()
            };
        }
    }
}
