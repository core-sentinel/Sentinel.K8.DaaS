using FluentValidation;

namespace Sentinel.Core.HealthProbe.Http
{
    public class HttpHealthProbeValidator : AbstractValidator<HttpHealthProbeCommand>
    {

        public HttpHealthProbeValidator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage("Url is required.");
            RuleFor(x => x.Method).NotEmpty().WithMessage("Method is required");

        }
    }
}
