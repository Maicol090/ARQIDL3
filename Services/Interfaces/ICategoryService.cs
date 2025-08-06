using ARQIDL3.Models.DTOs;

namespace ARQIDL3.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
    }

}
