using System.ComponentModel.DataAnnotations;

namespace Synergy.Samples.Web.API.Services.Users.Commands.CreateUser
{
    public class CreateUserCommand
    {
        [Required]
        public string Login { get; set; }
    }
}