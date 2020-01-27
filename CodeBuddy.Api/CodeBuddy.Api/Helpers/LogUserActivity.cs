using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CodeBuddy.Api.Context.Repository;
using CodeBuddy.Api.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace CodeBuddy.Api.Helpers
{
    /// <summary>
    /// LogUserActivity is used to log the last active timestamp of user
    /// </summary>
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var repo = resultContext.HttpContext.RequestServices.GetService<IGenericRepository>();

            var user = await repo.Get<User>(userId);

            user.LastActive = DateTime.Now;

            await repo.SaveAll();
        }
    }
}
