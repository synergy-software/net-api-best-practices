using Serilog.Enrichers;

namespace Synergy.Samples.Web.API.Extensions
{
    public static class EnvironmentLogProperties
    {
        public const string MachineName = MachineNameEnricher.MachineNamePropertyName;
        public const string EnvironmentUserName = EnvironmentUserNameEnricher.EnvironmentUserNamePropertyName;
    }
}