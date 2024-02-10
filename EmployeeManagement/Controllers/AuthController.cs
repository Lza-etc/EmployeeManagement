using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<Employee> _employees;

        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _employees = DataStore.Instance.Employees;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployeeDTO employeeDto)
        {
            var employee = _employees.FirstOrDefault(e => e.Username == employeeDto.Username && e.Password == employeeDto.Password);
            if (employee == null)
            {
                return NotFound("Invalid credentials");
            }

            await _authService.GenerateToken(employee, _httpContextAccessor.HttpContext);
            return Ok("Logged in successfully");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out successfully");
        }
    }
}
