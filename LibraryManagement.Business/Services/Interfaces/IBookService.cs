using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<PaginatedResult<BookDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool isDescending);

        Task<BookDto> GetByIdAsync(int id);
        Task CreateAsync(BookCreateDto dto);
        Task UpdateAsync(int id, BookUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
