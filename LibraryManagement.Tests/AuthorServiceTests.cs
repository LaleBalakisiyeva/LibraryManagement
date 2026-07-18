using AutoMapper;
using FluentAssertions;
using FluentValidation;
using LibraryManagement.Business.DTOs.Author;
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
    public class AuthorServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<AuthorCreateDto>> _mockValidator;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork> { DefaultValue = DefaultValue.Mock };
            _mockMapper = new Mock<IMapper>();
            _mockValidator = new Mock<IValidator<AuthorCreateDto>>();

            _authorService = new AuthorService(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockValidator.Object
            );
        }

        [Fact]
        public async Task GetByIdAsync_WhenAuthorExists_ShouldReturnAuthorDto()
        {
            int authorId = 1;
            var authorEntity = new Author { Id = authorId, Name = "Martin Fowler" };
            var expectedDto = new AuthorDto { Id = authorId, Name = "Martin Fowler" };

            _mockUnitOfWork.Setup(u => u.Authors.GetByIdAsync(authorId))
                           .ReturnsAsync(authorEntity);

            _mockMapper.Setup(m => m.Map<AuthorDto>(authorEntity))
                       .Returns(expectedDto);

            var result = await _authorService.GetByIdAsync(authorId);

            result.Should().NotBeNull();
            result.Id.Should().Be(authorId);
            result.Name.Should().Be("Martin Fowler");
        }

        [Fact]
        public async Task CreateAsync_ShouldCallUnitOfWorkSave()
        {
            var newAuthorDto = new AuthorCreateDto { Name = "Linus Torvalds" };
            var authorEntity = new Author { Name = "Linus Torvalds" };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _mockValidator.Setup(v => v.ValidateAsync(newAuthorDto, default))
                          .ReturnsAsync(validationResult);

            _mockMapper.Setup(m => m.Map<Author>(newAuthorDto))
                       .Returns(authorEntity);

            await _authorService.CreateAsync(newAuthorDto);

            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
