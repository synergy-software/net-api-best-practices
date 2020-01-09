using Serilog.Enrichers;

namespace Synergy.Samples.Web.API.Extensions
{
    public static class EnvironmentLogProperties
    {
        public const string MachineName = MachineNameEnricher.MachineNamePropertyName;
        public const string EnvironmentUserName = EnvironmentUserNameEnricher.EnvironmentUserNamePropertyName;
        public const string EnvironmentName = "EnvironmentName";
        public const string ApplicationName = "ApplicationName";
        public const string ApplicationVersion = "ApplicationVersion";
    }

    public static class RequestLogProperties
    {
        public const string RequestMethod = "RequestMethod";
        public const string RequestPath = "RequestPath";
        public const string ResponseStatus = "StatusCode";

        public const string RequestHost = "RequestHost";
        public const string RequestScheme = "RequestScheme";
    }
}