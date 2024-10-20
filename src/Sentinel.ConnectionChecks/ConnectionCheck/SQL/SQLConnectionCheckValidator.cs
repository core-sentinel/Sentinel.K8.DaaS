using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.SQL;
public class SQLConnectionCheckValidator : AbstractValidator<SQLConnectionCheckRequest>
{
    public SQLConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

