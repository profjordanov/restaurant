using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Controllers._Base;
using Restaurant.Core.MealContext.Commands;
using Restaurant.Core.MealContext.HttpRequests;
using Restaurant.Core.OrderContext.Commands;
using Restaurant.Core.OrderContext.HttpRequests;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using System.Net;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public MealsController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a new meal and publishes an event.
        /// </summary>
        /// <response code="201">If the request passes the validations.</response>
        /// <response code="400">If meal type does not exist.</response>
        /// <response code="401">If current user is not the restaurant owner.</response>
        /// <response code="404">If restaurant with current ID does not exist.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RegisterMeal([FromBody] RegisterMealRequest request)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);

            var command = new RegisterMeal(
                request.Name, request.Price, request.TypeId, request.RestaurantId, identityUser.Id);

            return (await _mediator.Send(command))
                .Match(r => CreatedAtAction(nameof(RegisterMeal), r), Error);
        }

        /// <summary>
        /// Creates a new order of a meal in a specified quantity.
        /// </summary>
        /// <param name="id">Meal ID</param>
        /// <param name="request">quantity</param>
        [HttpPost]
        [Route("{id}/order")]
        [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> MakeOrder([FromRoute] string id, [FromBody] MakeOrderRequest request)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);

            var command = new MakeNewOrder(id, request.Quantity, identityUser.Id);

            return (await _mediator.Send(command))
                .Match(Ok, Error);
        }
    }
}