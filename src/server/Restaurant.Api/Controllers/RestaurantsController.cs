using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Core.RestaurantContext.Commands;
using System.Threading.Tasks;
using Restaurant.Api.Controllers._Base;
using Restaurant.Core.RestaurantContext.HttpRequests;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Views.Auth;

namespace Restaurant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public RestaurantsController(
            IMediator mediator,
            UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a new restaurant.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> RegisterRestaurant([FromBody] RegisterRestaurantRequest request)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);

            var command = new RegisterRestaurant(request.Name, request.TownId, identityUser.Id);

            return (await _mediator.Send(command))
                .Match(Ok, Error);
        }
    }
}