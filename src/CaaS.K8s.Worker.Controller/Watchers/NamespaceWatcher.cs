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
                        _logger.LogInformation("deployment found for namespace {ns} a new one will be Created...", ns);
                        // Create a new Deployment
                        addDeployment(resource.Metadata.Name, _client);
                    }
                    _logger.LogInformation(deployments.Count().ToString());
                }
                _logger.LogInformation(resource.Name() + "has the caas-enabled=true Annotation");

            }
            _logger.LogInformation("Namespace watcher event: {type} :  {name}", Event, resource.Metadata.Name);
        }


        public V1Deployment addDeployment(string @namespace, IKubernetesClient client)
        {
            var newdeployment = new V1Deployment();
            newdeployment.Kind = "Deployment";
            newdeployment.ApiVersion = "apps/v1";
            newdeployment.Metadata = new V1ObjectMeta();
            newdeployment.Metadata.Name = "caas-deployment";
            newdeployment.Metadata.NamespaceProperty = @namespace;
            newdeployment.Metadata.Labels = new Dictionary<string, string>();
            newdeployment.Metadata.Labels.Add("caas-deployment", "enabled");
            newdeployment.Spec = new V1DeploymentSpec();
            newdeployment.Spec.Replicas = 1;
            newdeployment.Spec.RevisionHistoryLimit = 1;
            newdeployment.Spec.Selector = new V1LabelSelector();
            newdeployment.Spec.Selector.MatchLabels = new Dictionary<string, string>();
            newdeployment.Spec.Selector.MatchLabels.Add("app", "test-deployment");
            newdeployment.Spec.Template = new V1PodTemplateSpec();
            newdeployment.Spec.Template.Metadata = new V1ObjectMeta();
            newdeployment.Spec.Template.Metadata.Labels = new Dictionary<string, string>();
            newdeployment.Spec.Template.Metadata.Labels.Add("app", "test-deployment");
            newdeployment.Spec.Template.Spec = new V1PodSpec();

            //  newdeployment.Spec.Template.Spec.ServiceAccountName = "app";
            newdeployment.Spec.Template.Spec.Containers = new List<V1Container>();
            newdeployment.Spec.Template.Spec.Containers.Add(new V1Container()
            {
                Name = "caas-deployment",
                Image = "mmercan/caas-ui-blazor-core:latest",  //"nginx:1.7.9",
                ImagePullPolicy = "IfNotPresent",

                Ports = new List<V1ContainerPort>()
                {
                    new V1ContainerPort()
                    {
                        ContainerPort = 80
                    }
                }
            });


            var deploy = client.Save<V1Deployment>(newdeployment);
            deploy.Wait();
            return deploy.Result;
        }
    }
}
