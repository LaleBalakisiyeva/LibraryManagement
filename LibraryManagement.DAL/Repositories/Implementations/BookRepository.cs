using LibraryManagement.Core.Entities;
using LibraryManagement.DAL.Context;
using LibraryManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL.Repositories.Implementations
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetAllWithAuthorAsync()
        {
            return await _context.Books.Include(b => b.Author).ToListAsync();
        }

        public async Task<Book> GetByIdWithAuthorAsync(int id)
        {
            return await _context.Books
                                 .Include(b => b.Author)
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<(IEnumerable<Book> Books, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool isDescending)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "title" => isDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                    "year" => isDescending ? query.OrderByDescending(x => x.PublishYear) : query.OrderBy(x => x.PublishYear),
                    _ => query.OrderBy(x => x.Id)
                };
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }

            int totalCount = await query.CountAsync();

            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }
    }
}
