using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUsers
{
    [CreatedImplicitly]
    public class GetUsersQueryHandler : IGetUsersQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUsersQueryResult> Handle(GetUsersQuery query)
        {
            var users = await _userRepository.GetAllUsers();
            return new GetUsersQueryResult(users);
        }
    }

    public interface IGetUsersQueryHandler : IQueryHandler<GetUsersQuery, GetUsersQueryResult> { }
}