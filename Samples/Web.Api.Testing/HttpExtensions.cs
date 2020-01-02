using System.Net.Http;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT
{
    public static class HttpExtensions
    {
        [MustUseReturnValue]
        public static JToken? ReadJson(this HttpContent? content)
        {
            var str = content?.ReadAsStringAsync().Result;
            if (string.IsNullOrWhiteSpace(str))
                return null;

            return JToken.Parse(str);
        }

        [MustUseReturnValue]
        public static HttpContent? Read<T>([NotNull] this HttpContent? content, string jsonPath, out T value)
        {
            Fail.IfNull(content, nameof(content));
            var node = content.ReadJson()!.SelectToken(jsonPath);
            value = node.Value<T>();
            return content;
        }

        [Pure]
        public static string GetRequestFullMethod(this HttpRequestMessage request)
            => $"{request.Method} {request.RequestUri.ToString().Replace("http://localhost", "")}";
    }
}