using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Controllers._Base;
using Restaurant.Core.OrderContext.HttpRequests;
using Restaurant.Core.OrderContext.Queries;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Views.Order;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public OrdersController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets the pending orders of the current user with paging.
        /// Accepts query parameters in URL query string:
        /// o startPage (the page that should be fetched)
        /// o limit (the number of orders that should be returned in the range 2..20) 
        /// </summary>
        /// <param name="request">
        /// <see cref="GetPendingOrdersRequest"/>
        /// </param>
        /// <returns>
        /// Orders made by the current user (with the above filtering and paging applied),
        /// which are currently pending.
        /// </returns>
        [HttpGet]
        [Route("user-pending-orders")]
        [ProducesResponseType(typeof(IList<PendingOrderView>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PendingOrders([FromQuery] GetPendingOrdersRequest request)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);

            var query = new GetPendingOrdersByUser(identityUser.Id, request);

            return (await _mediator.Send(query))
                .Match(Ok, Error);
        }
    }
}