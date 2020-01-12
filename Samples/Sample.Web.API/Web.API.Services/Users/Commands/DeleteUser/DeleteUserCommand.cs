namespace Synergy.Samples.Web.API.Services.Users.Commands.DeleteUser {
    public class DeleteUserCommand
    {
        public string UserId { get; }

        public DeleteUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}