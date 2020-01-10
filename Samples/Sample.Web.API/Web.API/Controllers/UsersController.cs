using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synergy.Samples.Web.API.Services.Infrastructure.Commands;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;
using Synergy.Samples.Web.API.Services.Users;
using Synergy.Samples.Web.API.Services.Users.Commands.CreateUser;
using Synergy.Samples.Web.API.Services.Users.Queries.GetUsers;

namespace Sample.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public UsersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Returns all users stored in the system.
        /// </summary>
        [HttpGet]
        public async Task<GetUsersQueryResult> GetUsers()
        {
            return await _queryDispatcher.Dispatch<GetUsersQuery, IGetUsersQueryHandler, GetUsersQueryResult>(new GetUsersQuery());
        }

        [HttpGet("{userId}", Name=nameof(GetUser))]
        public async Task<UserReadModel> GetUser(string userId)
        {
            return await Task.FromResult(new UserReadModel(userId, "login"));
        }

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/users
        ///     {
        ///        "login": "marcin@synergy.com",
        ///     }
        /// 
        /// </remarks>
        /// <param name="version">API version</param>
        /// <param name="user">Details of user to create</param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the request contains invalid data</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<CreateUserCommandResult>> Create([FromRoute] string version, [FromBody] CreateUserCommand user)
        {
            var created = await _commandDispatcher.Dispatch<CreateUserCommand, ICreateUserCommandHandler, CreateUserCommandResult>(user);
            return CreatedAtRoute(nameof(GetUser), new {version, userId = created.User.Id}, created);
        }
    }
}