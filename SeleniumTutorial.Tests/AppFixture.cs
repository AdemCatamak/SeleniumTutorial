using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

namespace SeleniumTutorial.Tests;

public class AppFixture : IAsyncDisposable
{
    private readonly IFutureDockerImage _appContainer;
    private readonly IContainer _container;

    private const int Port = 80;
    public object BaseAddress => $"http://localhost:{Port}/";

    public AppFixture()
    {
        string imageName = "seleniumtutorial";

        _appContainer = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Dockerfile")
            .WithName(imageName)
            .WithDeleteIfExists(true)
            .Build();

        _appContainer
            .CreateAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        _container = new ContainerBuilder()
            .WithImage(imageName)
            .WithPortBinding(Port, 8080)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            .Build();

        _container.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
        await _appContainer.DisposeAsync();
    }
}

[CollectionDefinition(AppFixtureKey)]
public class AppFixtureDefinition : ICollectionFixture<AppFixture>
{
    public const string AppFixtureKey = "AppFixtureKey";
}