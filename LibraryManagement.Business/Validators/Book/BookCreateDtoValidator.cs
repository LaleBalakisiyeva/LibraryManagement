using FluentValidation;
using LibraryManagement.Business.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Validators.Book
{
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Book name cannot be empty.")
                .MaximumLength(150).WithMessage("Maximum 150 characters.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Choose the right author");
        }
    }
}
