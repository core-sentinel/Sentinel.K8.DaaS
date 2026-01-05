using DaaS.UI.Blazor.Background;
using DaaS.UI.Blazor.Components;
using DaaS.UI.Blazor.Services;
using Sentinel.ConnectionChecks;
using Sentinel.Core.TokenGenerator;
using TabBlazor;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ConnectionChecksAssemblyMarker>();
    cfg.RegisterServicesFromAssemblyContaining<ITokenGeneratorAssemblyMarker>();
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTabler()
              .AddSingleton<AppService>();

builder.Services.AddHttpClient();
// builder.Services.AddSingleton<ConnectivityCheckService>();

//ConnectionCheckDiscovery discovery = new ConnectionCheckDiscovery();
//discovery.ScanForConnectionCheckTypes(typeof(ConnectionChecksAssemblyMarker));
builder.Services.AddSingleton<ConnectionCheckDiscovery>((s) =>
{
    return new ConnectionCheckDiscovery(typeof(ConnectionChecksAssemblyMarker));
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddOptions<HealthCheckCronRequest>()
    .Configure<IConfiguration>((settings, configuration) =>
    {
        configuration.GetSection("HealthChecks").Bind(settings);
    });

builder.Services.AddTickerQ(options =>
{
    //options.UseInMemoryStorage();
    //options.ScanForTickerFunctions(typeof(DaaS.UI.Blazor.TickerQFunctions.TickerQFunctionsAssemblyMarker));

    options.AddDashboard(dashboardOptions =>
    {
        dashboardOptions.SetBasePath("/admin/tickerq");
        dashboardOptions.WithBasicAuth("admin", "secure-password");
    });
});

builder.Services.AddHostedService<CronTrigger>(); // Registration line



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseTickerQ(); // Activate job processor



app.Run();
