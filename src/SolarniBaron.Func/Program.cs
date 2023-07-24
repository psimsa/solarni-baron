using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SolarniBaron.Caching;
using SolarniBaron.Domain;
using SolarniBaron.Domain.Extensions;
using SolarniBaron.Persistence;

var serviceName = "SolarniBaron";
var serviceVersion = "1.0.0";

var otlpOptions = (OtlpExporterOptions options) =>
{
    options.Endpoint = new Uri("https://otlp.eu01.nr-data.net");
    options.Protocol = OtlpExportProtocol.Grpc;
    options.Headers = "api-key=eu01xxefc1a87820b35d1becb5efd5c5FFFFNRAL";
};

Action<ResourceBuilder> setupResource = (ResourceBuilder rb) =>
{
    rb.AddService(serviceName: serviceName, serviceVersion: serviceVersion);
};

HostBuilder builder = new HostBuilder();
builder.ConfigureServices((context, services) =>
{
    services.AddDomain();
    services.AddPersistence();
    services.AddHttpClient();

    services.AddSingleton(sp =>
        new CommonSerializationContext(new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}));

    services.RegisterCache(context.Configuration);

    services.AddOpenTelemetry()
        .WithTracing(builder =>
        {
            builder.AddConsoleExporter()
            // .AddSource(serviceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(otlpOptions)
            .ConfigureResource(setupResource);
        })
        /*.WithMetrics(builder =>
        {
            builder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(otlpOptions)
            .AddConsoleExporter()
            .ConfigureResource(setupResource)
            ;
        })*/
        ;
});

builder.ConfigureLogging((context, logging) =>
{
    logging.AddOpenTelemetry(options =>
    {
        var rb = ResourceBuilder.CreateDefault();
        setupResource(rb);
        options.IncludeScopes = true;
        options.ParseStateValues = true;
        options.AddOtlpExporter(otlpOptions);
        options.SetResourceBuilder(rb);
    });
});

var host = builder
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
