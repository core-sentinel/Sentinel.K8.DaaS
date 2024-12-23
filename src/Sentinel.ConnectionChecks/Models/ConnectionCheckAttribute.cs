namespace Sentinel.ConnectionChecks.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class ConnectionCheckAttribute : Attribute
    {
        public string Name { get; set; } = default!;
        public int Order { get; set; }
        public string Description { get; set; } = default!;
        public bool Enabled { get; set; } = true;
    }
}
