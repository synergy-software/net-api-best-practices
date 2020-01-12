using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUser
{
    [CreatedImplicitly]
    public class GetUserQueryHandler : IGetUserQueryHandler
    {
        public GetUserQueryHandler()
        {
            // return await _queryDispatcher.Dispatch<GetUserQuery, IGetUserQueryHandler, GetUserQueryResult>(new GetUserQuery());
        }

        public async Task<GetUserQueryResult> Handle(GetUserQuery query)
        {
            return new GetUserQueryResult(query.UserId, "login");
        }
    }

    public interface IGetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserQueryResult>{}
}