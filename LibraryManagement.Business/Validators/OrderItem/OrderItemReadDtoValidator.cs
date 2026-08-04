using FluentValidation;
using LibraryManagement.Business.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Validators.OrderItem
{
    public class OrderItemReadDtoValidator : AbstractValidator<OrderItemReadDto>
    {
        public OrderItemReadDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Order Item ID must be greater than zero.");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book ID must be valid.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");
        }
    }
}
