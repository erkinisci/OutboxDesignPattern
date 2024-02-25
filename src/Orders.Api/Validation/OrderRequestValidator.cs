using FluentValidation;
using Orders.Api.Contracts.Requests;

namespace Orders.Api.Validation;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}