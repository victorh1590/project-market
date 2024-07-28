using DotNet.Testcontainers.Containers;

namespace ProjectMarket.Test.Integration;

public interface IDbResource : IAsyncDisposable, IAsyncStartup
{
    IDatabaseContainer DatabaseContainer { get; }
}