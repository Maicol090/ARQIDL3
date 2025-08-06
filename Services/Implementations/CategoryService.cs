using ARQIDL3.Data;
using ARQIDL3.Models.DTOs;
using ARQIDL3.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ARQIDL3.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToListAsync();
        }
    }
}
