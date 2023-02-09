using System.Reflection;
using System.Text.Json.Serialization;
using BlazorApplicationInsights;
using BlazorState;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SolarniBaron.Domain;
using SolarniBaron.Web;
using SolarniBaron.Web.Core;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var clientConfig = builder.Configuration.GetSection("ClientConfig");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<JsonSerializerContext>(jso =>
{
    jso.Options.AddContext<CommonSerializationContext>();
});

builder.Services.AddSingleton<LocalStorage>();

builder.Services.AddBlazorApplicationInsights();

builder.Services.AddSingleton(new ClientConfig(clientConfig["LocalGetStatsUrl"] ?? "api/batterybox/getstats"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazorState(blazorStateOptions =>
    {
#if DEBUG
        blazorStateOptions.UseReduxDevTools(reduxDevToolsOptions =>
        {
            reduxDevToolsOptions.Trace = true;
            reduxDevToolsOptions.TraceLimit = 25;
        });
#endif
        blazorStateOptions.Assemblies =
            new Assembly[]
            {
                typeof(Program).GetTypeInfo().Assembly,
            };
    }
);

var webAssemblyHost = builder.Build();

await webAssemblyHost.RunAsync();

namespace SolarniBaron.Web
{
    public record ClientConfig(string LocalGetStatsUrl);
}
