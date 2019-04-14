using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Core.RestaurantContext.Commands;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public RestaurantsController(
            IMediator mediator,
            UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterRestaurant([FromBody] RegisterRestaurant command)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            command.OwnerId = identityUser.Id;

            return Accepted(); //TODO
        }
    }
}