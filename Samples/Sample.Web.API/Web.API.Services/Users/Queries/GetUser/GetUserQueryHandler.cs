using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Exceptions;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUser
{
    [CreatedImplicitly]
    public class GetUserQueryHandler : IGetUserQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserQueryResult> Handle(GetUserQuery query)
        {
            var user = await _userRepository.FindUserBy(query.UserId);
            if (user == null)
            {
                throw new ResourceNotFoundException($"User with id {query.UserId} does not exist");
            }

            return new GetUserQueryResult(user);
        }
    }

    public interface IGetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserQueryResult>{}
}