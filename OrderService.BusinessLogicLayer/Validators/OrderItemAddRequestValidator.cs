using FluentValidation;
using OrderService.BusinessLogicLayer.DTO;

namespace OrderService.BusinessLogicLayer.Validators
{
    public class OrderItemAddRequestValidator : AbstractValidator<OrderItemAddRequest>
    {
        public OrderItemAddRequestValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("Product ID can't be blank");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Unit price can't be blank").GreaterThan(0).WithMessage("Unit price can't be less than or equal to zero");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity can't be blank").GreaterThan(0).WithMessage("Quantity can't be less than zero.");
        }
    }
}