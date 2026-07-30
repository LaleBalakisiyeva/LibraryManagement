using AutoMapper;
using FluentValidation;
using LibraryManagement.Business.DTOs.Auth;
using LibraryManagement.Business.Services.Implementations;
using LibraryManagement.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<RegisterDto>> _registerValidatorMock;
        private readonly Mock<IValidator<LoginDto>> _loginValidatorMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _configMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _registerValidatorMock = new Mock<IValidator<RegisterDto>>();
            _loginValidatorMock = new Mock<IValidator<LoginDto>>();

            _authService = new AuthService(
                _unitOfWorkMock.Object,
                _configMock.Object,
                _mapperMock.Object,
                _registerValidatorMock.Object,
                _loginValidatorMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_WhenUsernameOrEmailExists_ThrowsInvalidOperationException()
        {
       
            var dto = new RegisterDto { Username = "testuser", Email = "test@test.com" };

          
            _registerValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<RegisterDto>>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _unitOfWorkMock.Setup(u => u.Users.IsUsernameOrEmailExistsAsync(dto.Username, dto.Email))
                .ReturnsAsync(true);

          
            await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(dto));
        }
    }
}
