using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.Tests;
public interface ICommonHandler { }
public interface ICommonHandler<T> : ICommonHandler where T : IBasicCheckAccessRequest
{
    Task<TestNetConnectionResponse> Handle(T request, CancellationToken cancellationToken);
}


public interface ICommonHandler<T, K> : ICommonHandler where T : IBasicCheckAccessRequest where K : new()
{
    Task<TestNetConnectionResponse<K>> Handle(T request, CancellationToken cancellationToken);
}