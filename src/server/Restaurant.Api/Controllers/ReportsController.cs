using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Controllers._Base;
using Restaurant.Core.ReportContext.Commands;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ApiController
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("user-logins-report")]
        public async Task<IActionResult> GenerateUserLoginReport() =>
            (await _mediator.Send(new UserLoginsReportRequest()))
            .Match(File, Error);
    }
}