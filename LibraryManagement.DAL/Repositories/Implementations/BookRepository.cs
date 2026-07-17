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
    }
}
