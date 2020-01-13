namespace Synergy.Samples.Web.API.Services.Users.Domain
{
    public class User
    {
        public string Id { get; }
        public string Login { get; }

        public User(string id, string login)
        {
            Id = id;
            Login = login;
        }
    }
}
