using Sample.API.Controllers;
using Synergy.Web.Api.Testing;

namespace Synergy.Samples.Web.API.Tests.Weather
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

        public HttpOperation Create(TodoItem todo)
            => _testServer.Post(Path, body: todo)
                          .Details($"Create a new TODO item named '{todo.Name}'");
    }
}