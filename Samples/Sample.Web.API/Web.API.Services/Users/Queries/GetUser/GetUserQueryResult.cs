using Newtonsoft.Json;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUser
{
    public class GetUserQueryResult
    {
        [JsonProperty("user")]
        public UserReadModel User { get; }

        public GetUserQueryResult(User user)
        {
            User = new UserReadModel(user);
        }
    }
}