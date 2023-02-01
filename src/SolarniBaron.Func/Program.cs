using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolarniBaron.Caching;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Persistence;

HostBuilder builder = new HostBuilder();
builder.ConfigureServices((context, services)=>
{
    services.AddDomain();
    services.AddPersistence();
    services.AddHttpClient();

    services.RegisterCache(context.Configuration);
});

var host = builder
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
