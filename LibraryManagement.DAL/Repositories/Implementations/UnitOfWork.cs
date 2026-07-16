using LibraryManagement.DAL.Context;
using LibraryManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IBookRepository _bookRepository;
        private IAuthorRepository _authorRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IBookRepository Books => _bookRepository ??= new BookRepository(_context);

        public IAuthorRepository Authors => _authorRepository ??= new AuthorRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
