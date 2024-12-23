using DaaS.UI.Blazor.Components;
using DaaS.UI.Blazor.Services;
using Sentinel.ConnectionChecks;
using Sentinel.Core.TokenGenerator;
using TabBlazor;

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

app.Run();
