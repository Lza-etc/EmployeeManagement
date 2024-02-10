using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmployeeManagement.Models;

namespace EmployeeManagement.Attributes
{
    public class ReporteeManagerAuthAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _employeeIdParameterName;
        private readonly List<Employee> _employees;

        public ReporteeManagerAuthAttribute(string employeeIdParameterName)
        {
            _employeeIdParameterName = employeeIdParameterName;
            _employees = DataStore.Instance.Employees;
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

            var employee = _employees.FirstOrDefault(x => x.Id == employeeId);
            if (employee == null || userIdClaim != employee.ManagerId.ToString())
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
