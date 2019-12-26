using System.Collections.ObjectModel;
using System.Reflection;
using Castle.Core.Internal;

namespace Synergy.Samples.Web.API.Extensions
{
    public static class Application
    {
        public static ReadOnlyCollection<Assembly> GetApplicationAssemblies()
        {
            return ReflectionUtil.GetApplicationAssemblies(Assembly.GetExecutingAssembly()).AsReadOnly();
        }
    }
}
