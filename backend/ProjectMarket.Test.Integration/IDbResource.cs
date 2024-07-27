using DotNet.Testcontainers.Containers;

namespace ProjectMarket.Test.Integration;

public interface IDbResource : IAsyncLifetime
{
    IDatabaseContainer DatabaseContainer { get; }
}