using Newtonsoft.Json;
using Synergy.Samples.Web.API.Services.Users.Domain;

namespace Synergy.Samples.Web.API.Services.Users.Commands.CreateUser
{
    public class CreateUserCommandResult
    {
        [JsonProperty("user")]
        public UserReadModel User { get; }

        public CreateUserCommandResult(User user)
        {
            User = new UserReadModel(user);
        }
    }
}