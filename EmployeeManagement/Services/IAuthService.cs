using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface IAuthService
    {
        public Task GenerateToken(Employee employee, HttpContext httpContext);
    }
}
