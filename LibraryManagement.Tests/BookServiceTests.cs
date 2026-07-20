using AutoMapper;
using FluentAssertions;
using FluentValidation;
using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.Helpers.Exceptions;
using LibraryManagement.Business.Services.Implementations;
using LibraryManagement.Core.Entities;
using LibraryManagement.DAL.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBookRepository> _mockBookRepo;
        private readonly Mock<IAuthorRepository> _mockAuthorRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<BookCreateDto>> _mockCreateValidator;
        private readonly Mock<IValidator<BookUpdateDto>> _mockUpdateValidator; 
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBookRepo = new Mock<IBookRepository>();
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateValidator = new Mock<IValidator<BookCreateDto>>();
            _mockUpdateValidator = new Mock<IValidator<BookUpdateDto>>(); 

            _mockUnitOfWork.Setup(u => u.Books).Returns(_mockBookRepo.Object);
            _mockUnitOfWork.Setup(u => u.Authors).Returns(_mockAuthorRepo.Object);

          
            _bookService = new BookService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object
            );
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookExists_ShouldReturnBookDto()
        {
            int bookId = 1;
            var bookEntity = new Book { Id = bookId, Title = "Test Book", AuthorId = 1 };
            var expectedDto = new BookDto { Id = bookId, Title = "Test Book" };

            _mockBookRepo.Setup(r => r.GetByIdWithAuthorAsync(It.IsAny<int>()))
                         .ReturnsAsync(bookEntity);

            _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>()))
                       .Returns(expectedDto);

            var result = await _bookService.GetByIdAsync(bookId);

            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.Title.Should().Be("Test Book");
        }

        
        [Fact]
        public async Task GetByIdAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
        {
            int nonExistentId = 999;
            _mockBookRepo.Setup(r => r.GetByIdWithAuthorAsync(nonExistentId))
                         .ReturnsAsync((Book)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _bookService.GetByIdAsync(nonExistentId));

            exception.Message.Should().Be($"{nonExistentId} The book with id was not found.");
        }

        [Fact]
        public async Task CreateAsync_ShouldCallUnitOfWorkSave()
        {
            var newBookDto = new BookCreateDto { Title = "New Architecture", AuthorId = 2 };
            var bookEntity = new Book { Title = "New Architecture", AuthorId = 2 };
            var authorEntity = new Author { Id = 2, Name = "Test Author" };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _mockCreateValidator.Setup(v => v.ValidateAsync(It.IsAny<BookCreateDto>(), default))
                          .ReturnsAsync(validationResult);

            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<BookCreateDto>()))
                       .Returns(bookEntity);

            _mockAuthorRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(authorEntity);

            await _bookService.CreateAsync(newBookDto);

            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
