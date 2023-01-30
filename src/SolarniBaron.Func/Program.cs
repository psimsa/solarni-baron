using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Persistence;

HostBuilder builder = new HostBuilder();
builder.ConfigureServices(services =>
{
    services.AddDomain();
    services.AddPersistence();
    services.AddHttpClient();
    services.AddDistributedMemoryCache();
});

var host = builder
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
