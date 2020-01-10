using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUsers
{
    public class GetUsersQueryResult
    {
        [JsonProperty("users")]
        public ReadOnlyCollection<UserReadModel> Users { get; }

        public GetUsersQueryResult()
        {
            Users = new ReadOnlyCollection<UserReadModel>(new List<UserReadModel>());
        }
    }
}