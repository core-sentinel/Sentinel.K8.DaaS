using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;
public class CosmosDBConnectionCheckValidator : AbstractValidator<CosmosDBConnectionCheckRequest>
{
    public CosmosDBConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

