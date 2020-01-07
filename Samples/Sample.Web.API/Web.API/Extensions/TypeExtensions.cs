using System;

namespace Synergy.Samples.Web.API.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsConstructable(this Type t)
        {
            return !t.IsAbstract;
        }
    }
}