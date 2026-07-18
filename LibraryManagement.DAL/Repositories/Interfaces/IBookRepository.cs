using LibraryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllWithAuthorAsync();
        Task<Book> GetByIdWithAuthorAsync(int id);
        Task<(IEnumerable<Book> Books, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool isDescending);
    }
}
