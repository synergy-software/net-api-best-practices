using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Synergy.Samples.Web.API.Extensions;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUsers
{
    public class GetUsersQueryResult
    {
        [JsonProperty("users")]
        public ReadOnlyCollection<UserReadModel> Users { get; }

        public GetUsersQueryResult(ReadOnlyCollection<User> users)
        {
            Users = users.ConvertAll(user => new UserReadModel(user));
        }
    }
}