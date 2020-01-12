using System;
using Synergy.Samples.Web.API.Services.Users.Commands.CreateUser;
using Synergy.Samples.Web.API.Tests.Infrastructure;
using Synergy.Web.Api.Testing;

namespace Synergy.Samples.Web.API.Tests.Users
{
    public class UsersClient
    {
        private const string Path = "api/v1/users";
        private readonly TestServer _testServer;

        public UsersClient(TestServer testServer)
        {
            _testServer = testServer;
        }

        public HttpOperation GetAll()
            => _testServer.Get(Path)
                          .Details("Get list of users");
        
        public HttpOperation GetUserBy(Uri location)
            => _testServer.Get(location.ToString())
                          .Details($"Get user located at {location}");

        public HttpOperation GetUser(string userId)
            => _testServer.Get($"{Path}/{userId}")
                          .Details($"Get user with id {userId.QuoteOrNull()}");

        public CreateUserOperation Create(string login)
            => _testServer.Post<CreateUserOperation>(Path, body: new CreateUserCommand{Login = login})
                          .Details($"Create a new user with login {login.QuoteOrNull()}");

        public HttpOperation DeleteUser(string userId)
            => _testServer.Delete($"{Path}/{userId}")
                          .Details($"Delete user with id {userId.QuoteOrNull()}");

        public class CreateUserOperation : HttpOperation
        {
            public CreateUserOperation ReadUserId(out string userId)
            {
                Response.Content.Read("user.id", out userId);
                return this;
            }

            public CreateUserOperation ReadCreatedUserLocationUrl(out Uri location)
            {
                location = Response.Headers.Location;
                return this;
            }
        }
    }
}