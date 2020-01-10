using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUsers
{
    public class GetUsersQuery { }

    [CreatedImplicitly]
    public class GetUsersQueryHandler : IGetUsersQueryHandler
    {
        public async Task<GetUsersQueryResult> Handle(GetUsersQuery query)
        {
            return new GetUsersQueryResult();
        }
    }

    public interface IGetUsersQueryHandler : IQueryHandler<GetUsersQuery, GetUsersQueryResult> { }

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