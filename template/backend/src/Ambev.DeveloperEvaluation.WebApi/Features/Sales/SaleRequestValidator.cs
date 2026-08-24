using Ambev.DeveloperEvaluation.Application.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

public sealed class SaleRequestValidator : AbstractValidator<SaleRequest>
{
    public SaleRequestValidator()
    {
        RuleFor(request => request.SaleNumber)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50);

        RuleFor(request => request.SaleDate)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(request => request.CustomerId)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(request => request.CustomerName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(request => request.BranchId)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(request => request.BranchName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(request => request.Items)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleForEach(request => request.Items)
            .SetValidator(new SaleItemRequestValidator());
    }
}

public sealed class SaleItemRequestValidator : AbstractValidator<SaleItemRequest>
{
    public SaleItemRequestValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(item => item.Quantity)
            .InclusiveBetween(1, 20);

        RuleFor(item => item.UnitPrice)
            .GreaterThanOrEqualTo(0);
    }
}


