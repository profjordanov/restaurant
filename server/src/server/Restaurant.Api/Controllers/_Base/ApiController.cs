using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Core._Base;
using Restaurant.Domain;
using Restaurant.Domain.Enumerations;
using System.Net;

namespace Restaurant.Api.Controllers._Base
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        /// <summary>
        /// Enables using method groups when matching on Unit.
        /// </summary>
        protected IActionResult Ok(Unit unit) => Ok();

        protected IActionResult NotFound(Error error) => NotFound((object)error);

        protected IActionResult Error(Error error)
        {
            switch (error.Type)
            {
                case ErrorType.Validation:
                    return BadRequest(error);
                case ErrorType.NotFound:
                    return NotFound(error);
                case ErrorType.Unauthorized:
                    return Unauthorized();
                case ErrorType.Conflict:
                    return Conflict(error);
                case ErrorType.Critical:
                    // This shouldn't really happen as critical errors are there to be used by the generic exception filter
                    return new ObjectResult(error)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                default:
                    return BadRequest(error);
            }
        }

        protected IActionResult File(HttpFile httpFile) =>
            File(httpFile.Data, httpFile.ContentType, httpFile.FileName);
    }
}
