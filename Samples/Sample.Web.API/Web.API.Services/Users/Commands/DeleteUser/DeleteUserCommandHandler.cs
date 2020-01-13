using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Commands;
using Synergy.Samples.Web.API.Services.Infrastructure.Exceptions;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Commands.DeleteUser
{
    [CreatedImplicitly]
    public class DeleteUserCommandHandler : IDeleteUserCommandHandler
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<DeleteUserCommandResult> Handle(DeleteUserCommand command)
        {
            var user = await _userRepository.FindUserBy(command.UserId);
            if (user == null)
            {
                throw new ResourceNotFoundException($"User with id {command.UserId} does not exist");
            }

            await _userRepository.DeleteUser(command.UserId);

            return new DeleteUserCommandResult();
        }
    }

    public interface IDeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserCommandResult>{}
}