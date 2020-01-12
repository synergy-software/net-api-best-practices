using System.Threading.Tasks;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Commands;

namespace Synergy.Samples.Web.API.Services.Users.Commands.DeleteUser
{
    [CreatedImplicitly]
    public class DeleteUserCommandHandler : IDeleteUserCommandHandler
    {
        public DeleteUserCommandHandler()
        {
            // return await _commandDispatcher.Dispatch<DeleteUserCommand, IDeleteUserCommandHandler, DeleteUserCommandResult>(new DeleteUserCommand());
        }

        public async Task<DeleteUserCommandResult> Handle(DeleteUserCommand command)
        {
            return new DeleteUserCommandResult();
        }
    }

    public interface IDeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserCommandResult>{}
}