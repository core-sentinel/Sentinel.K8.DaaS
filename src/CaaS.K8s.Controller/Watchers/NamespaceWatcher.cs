using k8s;
using k8s.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Sentinel.Core.K8s;
using Sentinel.Core.K8s.BackgroundServices;
using System.Xml.Linq;

namespace CaaS.K8s.Worker.Controller.Watchers
{
    [K8sWatcher(Name = "NamespaceWatcher",
        WatchAllNamespaces = true,
        Description = "Watches for Namespace changes",
        TimeoutTotalMinutes = 60,
        Enabled = true)]
    public class NamespaceWatcher : WatcherBackgroundService<V1Namespace>
    {


        // protected override Task watcher(WatchEventType type, k8s.Models.V1Namespace item)
        // {
        //     _logger.LogInformation("Namespace watcher event: {type} :  {name}", type, item.Metadata.Name);
        //     return Task.CompletedTask;
        // }

        IKubernetesClient _client;
        public NamespaceWatcher(IConfiguration configuration, IKubernetesClient client,
        ILogger<WatcherBackgroundService<V1Namespace>> logger, IOptions<HealthCheckServiceOptions> hcoptions)
        : base(configuration, client, logger, hcoptions)
        {
            _client = client;
        }

        public override void Watch(WatchEventType Event, V1Namespace resource)
        {
            if (resource.Metadata.Labels != null && resource.Metadata.Labels.ContainsKey("caas-injection") && resource.Metadata.Labels["caas-injection"] == "enabled")
            {
                if (Event == WatchEventType.Added || Event == WatchEventType.Modified)
                {
                    var ns = resource.Metadata.Name;

                    var deploymentTask = _client.List<V1Deployment>(resource.Metadata.Name, "caas-deployment=enabled");
                    deploymentTask.Wait();
                    var deployments = deploymentTask.Result;
                    if (deployments == null)
                    {
                        // Create a new Deployment
                    }
                    _logger.LogInformation(deployments.Count().ToString());
                }
                _logger.LogInformation(resource.Name() + "has the caas-enabled=true Annotation");

            }
            _logger.LogInformation("Namespace watcher event: {type} :  {name}", Event, resource.Metadata.Name);
        }
    }
}
