using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using JetBrains.Annotations;

namespace Synergy.Samples.Web.API.Services.Infrastructure.Queries
{
    [UsedImplicitly]
    public class ComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IQueryHandlerFactory>().AsFactory());
        }
    }
}