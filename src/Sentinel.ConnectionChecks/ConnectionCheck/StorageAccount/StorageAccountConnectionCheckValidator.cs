using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;
public class StorageAccountConnectionCheckValidator : AbstractValidator<StorageAccountConnectionCheckRequest>
{
    public StorageAccountConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");
        RuleFor(x => x.Url).NotEqual(".blob.core.windows.net").WithMessage("Enter Storage Account Name");
        RuleFor(X => X.StorageAccountName).NotEmpty().WithMessage("Enter Storage Account Name");

    }
}

