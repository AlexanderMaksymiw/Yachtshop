using Hangfire.Dashboard;

namespace AlexAPI.Services
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            bool result = httpContext.User.IsInRole("Admin");
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return result;
        }
    }
}
