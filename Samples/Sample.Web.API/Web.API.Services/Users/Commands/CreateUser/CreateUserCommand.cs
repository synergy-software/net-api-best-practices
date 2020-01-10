using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Services.Infrastructure.Annotations;
using Synergy.Samples.Web.API.Services.Infrastructure.Commands;

namespace Synergy.Samples.Web.API.Services.Users.Commands.CreateUser
{
    public class CreateUserCommand
    {
        public string Login { get; set; }
    }

    [CreatedImplicitly]
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        public Task<CreateUserCommandResult> Handle(CreateUserCommand command)
        {
            Fail.IfWhitespace(command.Login, nameof(command.Login));
            return Task.FromResult(new CreateUserCommandResult(Guid.NewGuid().ToString(), command.Login));
        }
    }

    public interface ICreateUserCommandHandler:ICommandHandler<CreateUserCommand, CreateUserCommandResult>{}

    public class CreateUserCommandResult
    {
        [JsonProperty("user")]
        public UserReadModel User { get; }

        public CreateUserCommandResult(string id, string login)
        {
            User = new UserReadModel(id, login);
        }
    }
}