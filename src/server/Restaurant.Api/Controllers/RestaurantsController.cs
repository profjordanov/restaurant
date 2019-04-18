using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Core.RestaurantContext.Commands;
using System.Threading.Tasks;
using Restaurant.Api.Controllers._Base;
using Restaurant.Domain.Entities;

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
        public async Task<IActionResult> RegisterRestaurant([FromBody] RegisterRestaurant command)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User); //TODO In Base
            command.OwnerId = identityUser.Id;

            return (await _mediator.Send(command))
                .Match(Ok, Error);
        }
    }
}