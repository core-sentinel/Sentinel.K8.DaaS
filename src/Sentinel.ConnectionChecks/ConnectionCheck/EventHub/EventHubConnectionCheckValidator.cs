using FluentValidation;

namespace Sentinel.ConnectionChecks.ConnectionCheck.EventHub;
public class EventHubConnectionCheckValidator : AbstractValidator<EventHubConnectionCheckRequest>
{
    public EventHubConnectionCheckValidator()
    {
        RuleFor(x => x.Url).NotEmpty().WithMessage("URL string is required.");

    }
}

