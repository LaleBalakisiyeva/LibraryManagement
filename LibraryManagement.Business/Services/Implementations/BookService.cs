using AutoMapper;
using FluentValidation;
using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.DTOs.Pagination;
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
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<BookCreateDto> _createValidator;
        private readonly IValidator<BookUpdateDto> _updateValidator;

        public BookService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<BookCreateDto> createValidator,
            IValidator<BookUpdateDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _unitOfWork.Books.GetAllWithAuthorAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<PaginatedResult<BookDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool isDescending)
        {
            var (books, totalCount) = await _unitOfWork.Books.GetAllPagedAsync(pageNumber, pageSize, sortBy, isDescending);

            var mappedBooks = _mapper.Map<IEnumerable<BookDto>>(books);

            return new PaginatedResult<BookDto>
            {
                Data = mappedBooks,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BookDto> GetByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdWithAuthorAsync(id);

            if (book == null)
                throw new NotFoundException($"{id} The book with id was not found.");

            return _mapper.Map<BookDto>(book);
        }

        public async Task CreateAsync(BookCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var book = _mapper.Map<Core.Entities.Book>(dto);
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, BookUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
                throw new NotFoundException($"{id} The book with id was not found.");

            _mapper.Map(dto, book);
            _unitOfWork.Books.Update(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
                throw new NotFoundException($"{id} The book with id was not found.");

            _unitOfWork.Books.Remove(book);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
