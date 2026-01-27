using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTests.Infra;

public class TestApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // LocalDB ejemplo:
    private const string TestConn =
        @"Server=DESKTOP-M69TKK8\\SQLEXPRESS03;Database=TeamTasksSample;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

    public async Task InitializeAsync()
    {
        var sqlPath = Path.Combine(AppContext.BaseDirectory, "Sql", "DBSetup_TeamTasks.sql");
        await SqlScriptRunner.RunScriptAsync(TestConn, sqlPath);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:TeamTasksSampleContexto"] = TestConn
            };

            config.AddInMemoryCollection(settings);
        });
    }
}