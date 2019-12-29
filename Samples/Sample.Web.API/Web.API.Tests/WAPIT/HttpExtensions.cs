using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public static class HttpExtensions
    {
        public static JToken? ReadJson(this HttpContent? content)
        {
            var str = content?.ReadAsStringAsync().Result;
            if (String.IsNullOrWhiteSpace(str))
                return null;

            return JToken.Parse(str);
        }
    }
}