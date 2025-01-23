using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http;
public class HttpConnectionCheckValidator : AbstractValidator<HttpConnectionCheckRequest>
{
    public HttpConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");
        RuleFor(x => x.Url).Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)).WithMessage("URL string is not a valid URI.");

    }
}

