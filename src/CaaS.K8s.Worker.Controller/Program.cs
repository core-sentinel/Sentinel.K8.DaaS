using Sentinel.Core.K8s.Middlewares;

namespace CaaS.K8s.Worker.Controller;
public class Program
{
    public static void Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == null) { environment = "Development"; }
        var appname = System.AppDomain.CurrentDomain.FriendlyName;



        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCoreK8s(builder.Configuration);
        builder.Services.AddK8sWatcherDefinitions(builder.Configuration, typeof(CaaS.K8s.Worker.Controller.Program));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseK8sWatcherUI((o) => o.DocumentTitle = "K8s Sync Watcher UI");
        app.UseAuthorization();

        app.MapControllers();
        app.MapGet("/isalive", () => "App is Alive!");
        app.Run();
    }
}