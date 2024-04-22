using k8s;
using k8s.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Sentinel.Core.K8s;
using Sentinel.Core.K8s.BackgroundServices;
using System.Xml.Linq;

namespace DaaS.K8s.Worker.Controller.Watchers
{
    [K8sWatcher(Name = "NamespaceWatcher",
        WatchAllNamespaces = true,
        Description = "Watches for Namespace changes",
        TimeoutTotalMinutes = 60,
        Enabled = true)]
    public class NamespaceWatcher : WatcherBackgroundService<V1Namespace>
    {

        public static string ControllerName = "";
        public static string DaasName = "daas-deployment";
        public static string DaasLabelName = "daas-deployment";
        public static string DaasLabelValue = "enabled";
        public static string DaasLabelCombined = "daas-deployment=enabled";
        public static string DaasNamespaceLabelName = "daas-injection";
        public static string DaasNamespaceLabelValue = "enabled";

        public static string DaasImage = "mmercan/daas-ui-blazor-core";
        public static string DaasTag = "latest";
        public static int DaasPortNumber = 8080;


        IKubernetesClient _client;
        IConfiguration _configuration;
        public NamespaceWatcher(IConfiguration configuration, IKubernetesClient client,
        ILogger<WatcherBackgroundService<V1Namespace>> logger, IOptions<HealthCheckServiceOptions> hcoptions)
        : base(configuration, client, logger, hcoptions)
        {
            _client = client;
            _configuration = configuration;
        }

        public override void Watch(WatchEventType Event, V1Namespace resource)
        {
            var deploymentTask = _client.List<V1Deployment>(resource.Metadata.Name, DaasLabelCombined);
            deploymentTask.Wait();
            var deployments = deploymentTask.Result;
            if (Event == WatchEventType.Added || Event == WatchEventType.Modified)
            {
                if (resource.Metadata.Labels != null && resource.Metadata.Labels.ContainsKey(DaasNamespaceLabelName) && resource.Metadata.Labels[DaasNamespaceLabelName] == DaasNamespaceLabelValue)
                {
                    _logger.LogInformation("{ns} has the daas-enabled=true Annotation", resource.Metadata.Name);
                    if (deployments == null || deployments.Count == 0)
                    {
                        try
                        {
                            _logger.LogInformation("deployment found for namespace {ns} a new one will be Created...", resource.Metadata.Name);
                            AddDeployment(resource, _client);
                            _logger.LogInformation($"New Deployment is Created in {resource.Metadata.Name}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("new deployment can not be created for {ns} : {ex}", resource.Metadata.Name, ex.Message);
                        }
                    }
                }
                else
                {
                    if (deployments != null && deployments.Count > 0)
                    {
                        try
                        {
                            RemoveDeployment(resource.Metadata.Name, deployments, _client);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("deployment can not be deleted for {ns} : {ex}", resource.Metadata.Name, ex.Message);
                        }
                    }
                }
            }
            _logger.LogInformation("Namespace watcher event: {type} :  {name}", Event, resource.Metadata.Name);
        }

        public V1Deployment AddDeployment(V1Namespace resource, IKubernetesClient client)
        {
            var image = DaasImage;
            var tag = DaasTag;
            int portnumber = DaasPortNumber;

            string daasPodIdentity = null;
            string? daasServiceAccount = null;
            if (resource.Metadata.Labels.ContainsKey("daas-podidentity"))
            {
                daasPodIdentity = resource.Metadata.Labels["daas-podidentity"];
            }

            if (resource.Metadata.Labels.ContainsKey("daas-service-account"))
            {
                daasServiceAccount = resource.Metadata.Labels["daas-service-account"];
            }

            if (_configuration["blazorimage"] != null)
            {
                image = _configuration["blazorimage"];
            }
            if (_configuration["blazorimagetag"] != null)
            {
                tag = _configuration["blazorimagetag"];
            }

            if (_configuration["blazorport"] != null)
            {

                if (Int32.TryParse(_configuration["blazorport"], out portnumber))
                {
                    _logger.LogInformation("port captured from config and assinged");
                }
                else
                {
                    _logger.LogInformation("port captured from config portnumber can not converted to int");
                }
            }

            var newdeployment = new V1Deployment();
            newdeployment.Kind = "Deployment";
            newdeployment.ApiVersion = "apps/v1";
            newdeployment.Metadata = new V1ObjectMeta();
            newdeployment.Metadata.Name = DaasName; ;
            newdeployment.Metadata.NamespaceProperty = resource.Metadata.Name;
            newdeployment.Metadata.Labels = new Dictionary<string, string>();
            newdeployment.Metadata.Labels.Add(DaasLabelName, DaasLabelValue);

            newdeployment.Spec = new V1DeploymentSpec();
            newdeployment.Spec.Replicas = 1;
            newdeployment.Spec.RevisionHistoryLimit = 1;
            newdeployment.Spec.Selector = new V1LabelSelector();
            newdeployment.Spec.Selector.MatchLabels = new Dictionary<string, string>();
            newdeployment.Spec.Selector.MatchLabels.Add("app", DaasName);
            newdeployment.Spec.Template = new V1PodTemplateSpec();
            newdeployment.Spec.Template.Metadata = new V1ObjectMeta();
            newdeployment.Spec.Template.Metadata.Labels = new Dictionary<string, string>();
            newdeployment.Spec.Template.Metadata.Labels.Add("app", DaasName);
            newdeployment.Spec.Template.Spec = new V1PodSpec();

            if (daasServiceAccount != null)
            {
                newdeployment.Spec.Template.Metadata.Labels.Add("azure.workload.identity/use", "true");
                newdeployment.Spec.Template.Spec.ServiceAccountName = daasServiceAccount;
            }
            newdeployment.Spec.Template.Metadata.Annotations = new Dictionary<string, string>();
            if (daasPodIdentity != null)
            {
                newdeployment.Spec.Template.Metadata.Labels.Add("aadpodidbinding", daasPodIdentity);
            }
            newdeployment.Spec.Template.Spec.Containers = new List<V1Container>();
            newdeployment.Spec.Template.Spec.Containers.Add(new V1Container()
            {
                Name = DaasName,
                Image = image + ":" + tag,
                ImagePullPolicy = "IfNotPresent",

                Ports = new List<V1ContainerPort>()
                {
                    new V1ContainerPort()
                    {
                        Name = "http",
                        ContainerPort = portnumber,
                        Protocol= "TCP"

                    }
                }
            });

            newdeployment.Spec.Template.Spec.NodeSelector = new Dictionary<string, string>();
            newdeployment.Spec.Template.Spec.NodeSelector.Add("beta.kubernetes.io/os", "linux");


            var deploy = client.Save<V1Deployment>(newdeployment);
            deploy.Wait();
            return deploy.Result;
        }

        public void RemoveDeployment(string @namespace, IList<V1Deployment> deployments, IKubernetesClient client)
        {
            _logger.LogInformation("deployment found for namespace {ns} a new one will be Deleted...", @namespace);
            if (deployments[0].Metadata.Name == DaasName)
            {
                var deploymentTask = _client.Delete<V1Deployment>(deployments[0].Metadata.Name, @namespace);
                deploymentTask.Wait();
                _logger.LogInformation($"Deployment is Deleted in {@namespace}");

            }
            else
            {
                _logger.LogInformation($"Deployment is not Deleted in {@namespace} because it is not created by daas");
            }

        }
    }
}
