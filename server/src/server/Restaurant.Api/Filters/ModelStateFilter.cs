using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Restaurant.Domain;
using System.Linq;

namespace Restaurant.Api.Filters
{
    public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context
                    .ModelState
                    .Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

                context.Result = new BadRequestObjectResult(Error.Validation(errors));
            }
        }
    }
}
