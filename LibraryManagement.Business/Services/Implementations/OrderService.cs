using AutoMapper;
using LibraryManagement.Business.DTOs.Order;
using LibraryManagement.Business.Services.Interfaces;
using LibraryManagement.Core.Entities;
using LibraryManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            using var transaction=await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = _mapper.Map<Order>(dto);

                order.TotalAmount = dto.OrderItems.Sum(x => x.Quantity * x.UnitPrice);
                order.OrderDate = DateTime.UtcNow;

                await _unitOfWork.Orders.AddAsync(order);

                await _unitOfWork.CommitAsync();

                return order.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<int> CreateOrderWithFailureAsync(CreateOrderDto dto)
        {
            using var transaction=await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order=_mapper.Map<Order>(dto);
                order.TotalAmount=dto.OrderItems.Sum(x=>x.Quantity * x.UnitPrice);
                order.OrderDate = DateTime.UtcNow;

                await _unitOfWork.Orders.AddAsync(order);

                await _unitOfWork.SaveChangesAsync();

                throw new InvalidOperationException("Simulated transaction failure for testing rollback!");
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public async Task<IEnumerable<OrderReadDto>> GetAllOrderAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllWithItemsAsync();
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }
    }
}
