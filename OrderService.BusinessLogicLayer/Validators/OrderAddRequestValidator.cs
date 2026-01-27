using FluentValidation;
using OrderService.BusinessLogicLayer.DTO;

namespace OrderService.BusinessLogicLayer.Validators
{
    public class OrderAddRequestValidator : AbstractValidator<OrderAddRequest>
    {
        public OrderAddRequestValidator()
        {
            RuleFor(x => x.UserID).NotEmpty().WithMessage("User ID can't be blank");
            RuleFor(x => x.OrderDate).NotEmpty().WithMessage("Order date can't be blank");
            RuleFor(x => x.OrderItems).NotEmpty().WithMessage("Order items can't be empty");
            
        }
    }
}