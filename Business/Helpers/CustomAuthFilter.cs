using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public class CustomAuthFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if session is null or "MoiUser" is not set
            var moiUser = context.HttpContext.Session.GetString("MoiUser");

            if (string.IsNullOrEmpty(moiUser))
            {
                // Get the current request URL for redirecting back after login
                var returnUrl = context.HttpContext.Request.Path;

                // Redirect to the Login action of the Account controller
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            }
            else
            {
                // Proceed with the action if authenticated
                await next();
            }
        }
    }
}