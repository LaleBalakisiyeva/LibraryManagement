using AutoMapper;
using FluentValidation;
using LibraryManagement.Business.DTOs.Author;
using LibraryManagement.Business.Helpers.Exceptions;
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
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorCreateDto> _createValidator;
        private readonly IValidator<AuthorUpdateDto> _updateValidator;

        public AuthorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<AuthorCreateDto> createValidator,
            IValidator<AuthorUpdateDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException($"{id} Author with ID not found.");

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task CreateAsync(AuthorCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var author = _mapper.Map<Core.Entities.Author>(dto);
            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, AuthorUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException($"{id} Author with ID not found.");

            _mapper.Map(dto, author);
            _unitOfWork.Authors.Update(author);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException($"{id} Author with ID not found.");

            _unitOfWork.Authors.Remove(author);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
