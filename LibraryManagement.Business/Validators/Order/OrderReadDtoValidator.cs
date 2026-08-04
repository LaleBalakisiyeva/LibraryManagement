using FluentValidation;
using LibraryManagement.Business.DTOs.Order;
using LibraryManagement.Business.Validators.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Validators.Order
{
    public class OrderReadDtoValidator : AbstractValidator<OrderReadDto>
    {
        public OrderReadDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Order ID must be greater than zero.");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date cannot be empty.");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount cannot be negative.");

            RuleForEach(x => x.OrderItems).SetValidator(new OrderItemReadDtoValidator());
        }
    }
}
