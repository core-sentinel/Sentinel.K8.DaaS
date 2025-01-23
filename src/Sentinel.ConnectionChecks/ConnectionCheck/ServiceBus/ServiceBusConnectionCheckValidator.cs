using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.ServiceBus;
public class ServiceBusConnectionCheckValidator : AbstractValidator<ServiceBusConnectionCheckRequest>
{
    public ServiceBusConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");
        RuleFor(X => X.ConnectionString).NotEmpty().WithMessage("Connection String Required");
        RuleFor(X => X.QueueName).NotEmpty().WithMessage("Queue Name is Required");

    }
}

