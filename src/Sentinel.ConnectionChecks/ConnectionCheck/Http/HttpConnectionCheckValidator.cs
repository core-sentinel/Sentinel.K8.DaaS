using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http;
public class HttpConnectionCheckValidator : AbstractValidator<HttpConnectionCheckRequest>
{
    public HttpConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

