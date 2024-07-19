using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Validators;

public class NotNullValidator<T> : AbstractValidator<T>
{
    public NotNullValidator()
    {
        RuleFor(x => x).NotNull().WithMessage($"Error validating {nameof(T)}: {typeof(T).Name} object must not be null.");
    }
}