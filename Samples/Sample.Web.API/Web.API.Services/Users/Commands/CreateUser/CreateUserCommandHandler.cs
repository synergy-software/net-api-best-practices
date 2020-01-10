using System;
using System.Threading.Tasks;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Commands;

namespace Synergy.Samples.Web.API.Services.Users.Commands.CreateUser
{
    [CreatedImplicitly]
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        public Task<CreateUserCommandResult> Handle(CreateUserCommand command)
        {
            Fail.IfWhitespace(command.Login, nameof(command.Login));
            return Task.FromResult(new CreateUserCommandResult(Guid.NewGuid().ToString().Replace("-", ""), command.Login));
        }
    }

    public interface ICreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserCommandResult> { }
}