using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Controllers._Base;
using Restaurant.Core.RatingContext.Commands;
using Restaurant.Core.RatingContext.HttpRequests;
using Restaurant.Core.RestaurantContext.Commands;
using Restaurant.Core.RestaurantContext.HttpRequests;
using Restaurant.Core.RestaurantContext.Queries;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Views.Restaurant;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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
        /// Returns all restaurants by town id, ordered by rating.
        /// </summary>
        /// <param name="query">Town ID GUID.</param>
        /// <returns>ID, name, average rating and town.</returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RestaurantWithAvrgRatingView>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRestaurantsByTown([FromQuery] GetAllRestaurantsByTown query) =>
            (await _mediator.Send(query)).Match(Ok, Error);

        /// POST: api/restaurants/register
        /// <summary>
        /// Creates a new restaurant and publishes an `RegisterRestaurant` event. 
        /// </summary>
        /// <response code="201">If the request passes the validations.</response>
        /// <response code="400">If town with current ID does not exist.</response>
        /// <response code="409">If restaurant with current name and town already exist.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> RegisterRestaurant([FromBody] RegisterRestaurantRequest request)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);

            var command = new RegisterRestaurant(request.Name, request.TownId, identityUser.Id);

            return (await _mediator.Send(command))
                .Match(r => CreatedAtAction(nameof(RegisterRestaurant), r), Error);
        }

        /// POST: api/restaurants/{restaurantId}/rate
        /// <summary>
        /// Rates an existing restaurant.
        /// An user may only give one rating per restaurant.
        /// If he rates twice, the old rating is overridden with the new value.
        /// Also publishes an `RestaurantRated` event.
        /// </summary>
        /// <param name="id">Restaurant ID.</param>
        /// <param name="request">Stars rate.</param>
        /// <response code="200">If the request passes the validations.</response>
        /// <response code="404"> If restaurant with current ID does not exist </response>
        /// <response code="400"> If user is trying to rate he/her's own restaurant. </response>
        [HttpPost]
        [Route("{id}/rate")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RateRestaurant(
            [FromRoute] string id,
            [FromBody] RateRestaurantRequest request)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);

            var command = new RateRestaurant(request.Stars, id, identityUser.Id);

            return (await _mediator.Send(command))
                .Match(Ok, Error);
        }
    }
}