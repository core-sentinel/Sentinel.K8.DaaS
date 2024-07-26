using FluentValidation;

namespace Sentinel.NetworkUtils.Redis;
public class RedisConnectionCheckValidator : AbstractValidator<RedisConnectionCheckCommand>
{
    public RedisConnectionCheckValidator()
    {
        RuleFor(x => x.ConnectionString).NotEmpty().WithMessage("Connection string is required.");
        RuleFor(x => x.ConnectionString).Must(x => x.Contains("password")).WithMessage("Connection string must contain password. when MSI or SP not used").When(x => (!x.UseMSI || x.Principal == null));
        RuleFor(x => x.ConnectionString).Must(x => !x.Contains("password")).WithMessage("Connection string must not contain password. when MSI or SP used").When(x => (x.UseMSI || x.Principal != null));

    }
}

