namespace Synergy.Samples.Web.API.Services.Users.Queries.GetUser
{
    public class GetUserQuery
    {
        public string UserId { get; }

        public GetUserQuery(string userId)
        {
            UserId = userId;
        }
    }
}