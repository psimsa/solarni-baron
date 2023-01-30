using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace SolarniBaron.Api.IntegrationTests;

internal static class TestHostBuilder
{
    public static HttpClient GetClient(Action<IServiceCollection>? testServiceSetup = null)
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                if (testServiceSetup != null)
                {
                    builder.ConfigureTestServices(testServiceSetup);
                }
            });

        var client = application.CreateClient();
        return client;
    }
}
