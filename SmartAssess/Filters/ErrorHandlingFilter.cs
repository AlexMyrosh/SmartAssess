using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Filters
{
    public class ErrorHandlingFilter(ILogger<ErrorHandlingFilter> logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "An unexpected error occurred: {Message}", context.Exception.Message);
            context.Result = new RedirectToActionResult("Error5xx", "Error", new { area = "" });
            context.ExceptionHandled = true;
        }
    }
}
