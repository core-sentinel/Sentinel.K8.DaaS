using Sentinel.ConnectionChecks.Models;
using System.Reflection;

namespace Sentinel.ConnectionChecks
{
    public class ConnectionCheckDiscovery
    {
        private Dictionary<string, Tuple<string, Type, ConnectionCheckAttribute>> ConnectionCheckTypes { get; set; } = new();

        public Dictionary<string, string> Categories { get; private set; } = new();

        public ConnectionCheckDiscovery(params Type[] scanMarkers)
        {
            ScanForConnectionCheckTypes(scanMarkers);
        }

        public void ScanForConnectionCheckTypes(params Type[] scanMarkers)
        {
            ConnectionCheckTypes = new Dictionary<string, Tuple<string, Type, ConnectionCheckAttribute>>();

            List<Type> connectionCheckTypes = new List<Type>();
            foreach (var marker in scanMarkers)
            {
                var types = marker.Assembly.ExportedTypes
                  .Where(
                    t => t.GetInterfaces().Contains(typeof(IBasicCheckAccessRequest)) &&
                    !t.IsInterface && !t.IsAbstract &&
                    Attribute.IsDefined(t, typeof(ConnectionCheckAttribute))
                  ).ToList();

                foreach (var type in types)
                {
                    var attribute = type.GetCustomAttribute<ConnectionCheckAttribute>();
                    if (attribute != null)
                    {
                        var name = string.IsNullOrWhiteSpace(attribute.Name) ? type.Name : attribute.Name;
                        ConnectionCheckTypes.Add(type.Name, Tuple.Create(name, type, attribute));
                    }
                }
            }

            Categories = ConnectionCheckTypes.Where(p => p.Value.Item3.Enabled).OrderBy(p => p.Value.Item3.Order).ToDictionary(t => t.Key, t => t.Value.Item1);

            foreach (var item in ConnectionCheckTypes)
            {
                Console.WriteLine(item.Key + " " + item.Value.Item1);
            }
        }

        public IBasicCheckAccessRequest CreateCheckAccessRequest(string typeName)
        {
            if (ConnectionCheckTypes.ContainsKey(typeName))
            {
                return Activator.CreateInstance(ConnectionCheckTypes[typeName].Item2) as IBasicCheckAccessRequest;
            }
            throw new InvalidOperationException($"Resource type {typeName} is not supported.");
        }
    }
}
