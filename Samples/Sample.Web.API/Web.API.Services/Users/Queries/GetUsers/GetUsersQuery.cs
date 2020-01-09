using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUsers
{
    public class GetUsersQuery { }

    [CreatedImplicitly]
    public class GetUsersQueryHandler : IGetUsersQueryHandler
    {
        public GetUsersQueryHandler() { }

        public async Task<GetUsersQueryResult> Handle(GetUsersQuery query)
        {
            return new GetUsersQueryResult();
        }
    }

    public interface IGetUsersQueryHandler:IQueryHandler<GetUsersQuery, GetUsersQueryResult> { }

    public class GetUsersQueryResult
    {
        public GetUsersQueryResult() { }
    }
}
