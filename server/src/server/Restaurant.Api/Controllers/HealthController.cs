using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Restaurant.Api is up and running!";
    }
}