using DotNet.Testcontainers.Containers;

namespace ProjectMarket.Test.Integration.Database.Interfaces;

public interface IDbResource : IAsyncDisposable, IAsyncStartup
{
    IDatabaseContainer DatabaseContainer { get; }
}