using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;

namespace Synergy.Web.Api.Testing
{
    public static class HttpExtensions
    {
        [MustUseReturnValue]
        public static JToken? ReadJson(this HttpContent? content)
        {
            var str = content?.ReadAsStringAsync().Result;
            if (string.IsNullOrWhiteSpace(str))
                return null;
            
            Fail.IfNotEqual(content.Headers.ContentType.MediaType, MediaTypeNames.Application.Json, "Content-Type");

            return JToken.Parse(str);
        }

        [MustUseReturnValue]
        public static HttpContent? Read<T>([NotNull] this HttpContent? content, string jsonPath, out T value)
        {
            value = content.Read<T>(jsonPath);
            return content;
        }

        [MustUseReturnValue]
        public static T Read<T>([NotNull] this HttpContent? content, string jsonPath)
        {
            Fail.IfNull(content, nameof(content));
            var node = content.ReadJson()!.SelectToken(jsonPath);
            return node.Value<T>();
        }

        [Pure]
        public static string GetRequestFullMethod(this HttpRequestMessage request)
            => $"{request.Method} {request.GetRequestRelativeUrl()}";

        
        [Pure]
        public static string GetRequestRelativeUrl(this HttpRequestMessage request)
            => request.RequestUri.ToString().Replace("http://localhost", "");

        public static List<KeyValuePair<string, IEnumerable<string>>> GetAllHeaders(this HttpRequestMessage request)
        {
            var headers = request.Headers.ToList();
            if (request.Content != null)
                headers.AddRange(request.Content.Headers);

            return headers;
        }

        public static List<KeyValuePair<string, IEnumerable<string>>> GetAllHeaders(this HttpResponseMessage response)
        {
            var headers = response.Headers.ToList();
            if (response.Content != null)
                headers.AddRange(response.Content.Headers);

            return headers;
        }
    }
}