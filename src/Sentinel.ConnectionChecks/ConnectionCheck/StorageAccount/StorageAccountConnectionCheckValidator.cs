using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;
public class StorageAccountConnectionCheckValidator : AbstractValidator<StorageAccountConnectionCheckRequest>
{
    public StorageAccountConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

