using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Sentinel.ConnectionChecks.Tests
{
    public class PipelineStepsTests
    {
        [Fact]
        public void methodAsksforAClassToSearchAssamblyfortheBaseClasses()
        {
            PipelineServiceConfiguration configuration = new PipelineServiceConfiguration();
            configuration.RegisterServicesFromAssemblyContaining(typeof(Sentinel.ConnectionChecks.ConnectionChecksAssemblyMarker));

            Assert.NotNull(configuration.ConnectionCheckTypes);
        }

        [Fact]
        public void methodAsksforAClassToSearchAssamblyfortheBaseClassesFromTestProject()
        {
            PipelineServiceConfiguration configuration = new PipelineServiceConfiguration();
            configuration.RegisterServicesFromAssemblyContaining(typeof(UnitTest1));

            configuration.ConnectionCheckTypes.Count.ShouldBe(1);
        }

        [Fact]
        public void methodRunoverMiddleWare()
        {
            var services = new ServiceCollection();
            services.AddMiddlewarePipeline(
                config =>
                {
                    config.RegisterServicesFromAssemblyContaining(typeof(UnitTest1));
                    config.RegisterServicesFromAssemblyContaining(typeof(Sentinel.ConnectionChecks.ConnectionChecksAssemblyMarker));
                });

            var sp = services.BuildServiceProvider();
            var pipe = sp.GetService<PipelineClass>();//.ShouldNotBeNull();
            pipe.Configuration.ConnectionCheckTypes.Count.ShouldBe(11);
        }

        [Fact]
        public void methodRunoverMiddleWare2()
        {
            var services = new ServiceCollection();
            services.AddMiddlewarePipeline(
                config =>
                {
                    config.RegisterServicesFromAssemblyContaining<UnitTest1>();
                    config.RegisterServicesFromAssemblyContaining<ConnectionChecksAssemblyMarker>();
                });

            var sp = services.BuildServiceProvider();
            var pipe = sp.GetService<PipelineClass>();//.ShouldNotBeNull();
            pipe.Configuration.ConnectionCheckTypes.Count.ShouldBe(11);
        }
    }
}
