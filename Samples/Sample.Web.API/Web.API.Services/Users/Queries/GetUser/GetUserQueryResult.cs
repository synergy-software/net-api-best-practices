using Newtonsoft.Json;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUser
{
    public class GetUserQueryResult
    {
        [JsonProperty("user")]
        public UserReadModel User { get; }

        public GetUserQueryResult(string id, string login)
        {
            User = new UserReadModel(id, login);
        }
    }
}