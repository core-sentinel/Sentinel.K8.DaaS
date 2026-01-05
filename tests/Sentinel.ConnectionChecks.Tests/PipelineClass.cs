using Microsoft.Extensions.DependencyInjection;
using Sentinel.ConnectionChecks.Models;
using System.Reflection;

namespace Sentinel.ConnectionChecks.Tests;
public static class MiddlewarePipelineClass
{


    public static void AddMiddlewarePipeline(this IServiceCollection services)
    {

    }


    public static IServiceCollection AddMiddlewarePipeline(this IServiceCollection services,
        Action<PipelineServiceConfiguration> configuration)
    {
        var serviceConfig = new PipelineServiceConfiguration();
        configuration.Invoke(serviceConfig);
        return services.AddMiddlewarePipeline(serviceConfig);
    }

    public static IServiceCollection AddMiddlewarePipeline(this IServiceCollection services,
    PipelineServiceConfiguration configuration)
    {
        services.AddSingleton<PipelineClass>(sp =>
        {
            var pipelineClass = new PipelineClass(configuration);
            return pipelineClass;
        });
        return services;
    }
}

public class PipelineServiceConfiguration
{

    public List<Assembly> AssembliesToRegister { get; } = new List<Assembly>();
    public List<Type> ConnectionCheckTypes { get; } = new List<Type>();


    public PipelineServiceConfiguration RegisterServicesFromAssemblyContaining<T>()
        => RegisterServicesFromAssemblyContaining(typeof(T));

    public PipelineServiceConfiguration RegisterServicesFromAssemblyContaining(Type marker)
    {
        AssembliesToRegister.Add(marker.Assembly);
        var connectionCheckers = marker.Assembly.ExportedTypes
               .Where(t => t.GetInterfaces().Contains(typeof(IBasicCheckAccessRequest))
               && !t.IsInterface && !t.IsAbstract
               && Attribute.IsDefined(t, typeof(ConnectionCheckAttribute))
          ).ToList();

        ConnectionCheckTypes.AddRange(connectionCheckers);
        return this;
    }
}

public class PipelineClass
{
    public PipelineServiceConfiguration Configuration { get; set; }
    public PipelineClass(PipelineServiceConfiguration configuration)
    {
        Configuration = configuration;
    }
}



[ConnectionCheck(Description = "test")]
public class TestIBasicCheckAccessRequestClass : IBasicCheckAccessRequest
{
    public string Url { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string SelectedAuthenticationType { get; set; } = string.Empty;
    public bool UseMSI { get; set; } = false;
    public ServicePrincipal? ServicePrincipal { get; set; }
    public Type? AdditionalRequestRazorContentType => null;

    public string JsonDeserializationType => this.GetType().FullName;
}