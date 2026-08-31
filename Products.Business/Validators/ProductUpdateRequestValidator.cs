using FluentValidation;
using Products.Business.RequestDto;

namespace Products.Business.Validators
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            // Validate ProductID
            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Product ID cannot be empty.");

            // Validate ProductName
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            // Validate Price
            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Invalid category.");

            // Validate Price
            RuleFor(x => x.Price)
                .InclusiveBetween(0, decimal.MaxValue).WithMessage("Price must be greater than or equal to 0.")
                .When(x => x.Price.HasValue);

            // Validate QuantityInStock
            RuleFor(x => x.QuantityInStock)
                .InclusiveBetween(0, int.MaxValue).WithMessage("Quantity in stock must be greater than or equal to 0.")
                .When(x => x.QuantityInStock.HasValue);
        }
    }
}
