using System;

namespace Synergy.Samples.Web.API.Services.Infrastructure.Exceptions
{
    // TODO: Convert this exception to 404
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message) { }
    }
}