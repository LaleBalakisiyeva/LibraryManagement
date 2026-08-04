using AutoMapper;
using FluentAssertions;
using LibraryManagement.Business.DTOs.Order;
using LibraryManagement.Business.Services.Implementations;
using LibraryManagement.Core.Entities;
using LibraryManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IDbContextTransaction> _mockTransaction;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockTransaction = new Mock<IDbContextTransaction>();

            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);

            _orderService = new OrderService(_mockUnitOfWork.Object, _mockMapper.Object);
        }


        [Fact]
        public async Task CreateOrderAsync_ShouldCommitTransaction_WhenOrderIsCreatedSuccessfully()
        {
            var createOrderDto = new CreateOrderDto {  };
            var mappedOrder = new Order { Id = 1 };

            _mockMapper.Setup(m => m.Map<Order>(createOrderDto)).Returns(mappedOrder);

            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync())
                           .ReturnsAsync(_mockTransaction.Object);

            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);


            await _orderService.CreateOrderAsync(createOrderDto);

            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }


        [Fact]
        public async Task CreateOrderAsync_ShouldTriggerRollback_WhenExceptionIsThrown()
        {
            var createOrderDto = new CreateOrderDto { };
            var mappedOrder = new Order { Id = 2 };

            _mockMapper.Setup(m => m.Map<Order>(createOrderDto)).Returns(mappedOrder);

            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync())
                           .ReturnsAsync(_mockTransaction.Object);


            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>()))
                                .ThrowsAsync(new Exception("Simulated Database Error for Rollback Trick"));

            _mockUnitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);


            Func<Task> act = async () => await _orderService.CreateOrderAsync(createOrderDto);


            await act.Should().ThrowAsync<Exception>().WithMessage("Simulated Database Error for Rollback Trick");

            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);

 
            _mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        }


        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnMappedDtos_WhenCalled()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, TotalAmount = 15.5m }
            };
            var orderDtos = new List<OrderReadDto>
            {
                new OrderReadDto { Id = 1, TotalAmount = 15.5m }
            };

            _mockOrderRepository.Setup(r => r.GetAllWithItemsAsync()).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderReadDto>>(orders)).Returns(orderDtos);


            var result = await _orderService.GetAllOrderAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().BeEquivalentTo(orderDtos);

            _mockOrderRepository.Verify(r => r.GetAllWithItemsAsync(), Times.Once);
        }
    }
}
