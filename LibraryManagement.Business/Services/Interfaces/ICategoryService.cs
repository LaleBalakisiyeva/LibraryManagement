using LibraryManagement.Business.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryGetDto>> GetAllAsync();
        Task<CategoryGetDto> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateDto dto);
        Task UpdateAsync(CategoryUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
