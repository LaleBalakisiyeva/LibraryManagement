using FluentValidation;
using LibraryManagement.Business.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Validators.Book
{
    public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
    {
        public BookUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The book title must be included.")
                .MaximumLength(150).WithMessage("The book title cannot exceed 150 characters.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Please enter a valid author ID.");
        }
    }
}
