using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Attributes
{
    public class EmployeeAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _employeeIdParameterName;

        public EmployeeAuthorizationAttribute(string employeeIdParameterName)
        {
            _employeeIdParameterName = employeeIdParameterName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!int.TryParse(context.HttpContext.Request.RouteValues[_employeeIdParameterName]?.ToString(), out int employeeId))
            {
                context.Result = new BadRequestObjectResult("Invalid employee id");
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.Name)?.Value;
            if (userIdClaim != employeeId.ToString())
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
