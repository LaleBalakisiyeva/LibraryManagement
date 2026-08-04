using LibraryManagement.Core.Entities;
using LibraryManagement.DAL.Context;
using LibraryManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllWithItemsAsync()
        {
            return await _context.Orders
               .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.Book)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
