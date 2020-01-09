using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.API.Controllers;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Services.Infrastructure.Queries;
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

        public UsersController(ILogger<UsersController> logger, IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Returns all users stored in the system.
        /// </summary>
        [HttpGet]
        public Task<GetUsersQueryResult> GetUsers()
        {
            return _queryDispatcher.Dispatch<GetUsersQuery, IGetUsersQueryHandler, GetUsersQueryResult>(new GetUsersQuery());
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/weather
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public ActionResult<TodoItem> Create([FromBody] TodoItem item)
        {
            Fail.IfWhitespace(item.Name, nameof(item.Name));

            return Created("forecast", item);
        }
    }
}