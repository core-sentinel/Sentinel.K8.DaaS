using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.AzureAppConfig;
public class AzureAppConfigConnectionCheckValidator : AbstractValidator<AzureAppConfigConnectionCheckRequest>
{
    public AzureAppConfigConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

