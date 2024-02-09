using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class AuthController : ControllerBase
    {
        List<Employee> employees = DataStore.Instance.Employees;
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployeeDTO employeeDto)
        {
            Employee employee = employees.Where(e => e.Username == employeeDto.Username && e.Password == employeeDto.Password).FirstOrDefault();
            if ( employee==null)
            {
                return Unauthorized();
            }
            AuthServices.GenerateToken(employee);
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}

