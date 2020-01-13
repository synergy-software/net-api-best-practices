using System.Threading.Tasks;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Commands;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Commands.CreateUser
{
    [CreatedImplicitly]
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateUserCommandResult> Handle(CreateUserCommand command)
        {
            // TODO: Add validation (bad-request) mechanism - maybe use data annotations?
            Fail.IfWhitespace(command.Login, nameof(command.Login));
            var user = await _userRepository.CreateUser(command.Login);
            return new CreateUserCommandResult(user);
        }
    }

    public interface ICreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserCommandResult> { }
}