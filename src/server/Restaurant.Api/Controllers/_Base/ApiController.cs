using Microsoft.AspNetCore.Mvc;
using Restaurant.Core;

namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        protected IActionResult Error(Error error) =>
            new BadRequestObjectResult(error);
    }
}
