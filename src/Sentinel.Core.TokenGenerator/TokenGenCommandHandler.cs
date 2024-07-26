using MediatR;

namespace Sentinel.Core.TokenGenerator
{
    internal class TokenGenCommandHandler : IRequestHandler<TokenGenCommand, TokenGenResponse>
    {
        public Task<TokenGenResponse> Handle(TokenGenCommand request, CancellationToken cancellationToken)
        {
            TokenGenResponse tokenGenResponse = new TokenGenResponse();


            return Task.FromResult(tokenGenResponse);
        }
    }
}
