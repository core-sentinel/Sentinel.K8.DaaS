using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.TcpPing;
public class TcpPingConnectionCheckValidator : AbstractValidator<TcpPingConnectionCheckRequest>
{
    public TcpPingConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

