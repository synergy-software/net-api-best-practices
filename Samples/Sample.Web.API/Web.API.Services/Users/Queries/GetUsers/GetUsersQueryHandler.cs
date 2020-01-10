using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUsers
{
    [CreatedImplicitly]
    public class GetUsersQueryHandler : IGetUsersQueryHandler
    {
        public Task<GetUsersQueryResult> Handle(GetUsersQuery query)
        {
            return Task.FromResult(new GetUsersQueryResult());
        }
    }

    public interface IGetUsersQueryHandler : IQueryHandler<GetUsersQuery, GetUsersQueryResult> { }
}