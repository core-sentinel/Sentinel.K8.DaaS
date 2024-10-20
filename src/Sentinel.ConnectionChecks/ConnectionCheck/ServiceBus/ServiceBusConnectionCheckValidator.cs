using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.ServiceBus;
public class ServiceBusConnectionCheckValidator : AbstractValidator<ServiceBusConnectionCheckRequest>
{
    public ServiceBusConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

