using DaaS.UI.Blazor.Components;
using DaaS.UI.Blazor.Services;
using Sentinel.ConnectionChecks;
using Sentinel.Core.TokenGenerator;
using Sentinel.NetworkUtils;
using TabBlazor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<NetworkUtilsAssemblyMarker>();
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
