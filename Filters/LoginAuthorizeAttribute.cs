using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace website_ban_hang.Filters
{
    public class LoginAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = context.HttpContext.Session.GetInt32("CustomerId");

            if (id == null)
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    null);
            }

            base.OnActionExecuting(context);
        }
    }
}