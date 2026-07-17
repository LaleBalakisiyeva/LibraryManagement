using LibraryManagement.Business.DTOs.Book;
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
        Task<BookDto> GetByIdAsync(int id);
        Task CreateAsync(BookCreateDto dto);
        Task UpdateAsync(int id, BookUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
