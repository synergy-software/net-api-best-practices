using Newtonsoft.Json;

namespace Synergy.Samples.Web.API.Services.Users.Commands.CreateUser
{
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