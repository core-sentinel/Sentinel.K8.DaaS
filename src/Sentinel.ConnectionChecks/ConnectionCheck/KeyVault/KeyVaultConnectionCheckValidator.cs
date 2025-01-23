using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.KeyVault;
public class KeyVaultConnectionCheckValidator : AbstractValidator<KeyVaultConnectionCheckRequest>
{
    public KeyVaultConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");
        RuleFor(x => x.KeyVaultName).NotEmpty().WithMessage("Key vault name is required.");
    }
}

