using FluentValidation;
using LibraryManagement.Business.DTOs.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Validators.Author
{
    public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
    {
        public AuthorCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The author's name must be included.")
                .MinimumLength(2).WithMessage("The author name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("The author name cannot exceed 100 characters.");
        }
    }
}
