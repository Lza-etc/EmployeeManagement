using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly List<Employee> _employees = DataStore.Instance.Employees;
        private readonly List<LeaveApplication> _allLeaveApplications = DataStore.Instance.LeaveApplications;

        [HttpGet("{employeeId}")]
        [Authorize]
        public IActionResult Get([FromRoute] int employeeId)
        {
            int? id = int.Parse(User.Identity.Name);

            Employee employee = _employees.FirstOrDefault(x => x.Id == employeeId)!;

            if (employee == null || (id != employeeId && employee.ManagerId != id))
                return Unauthorized();

            EmployeeResponseDTO response = new()
            {
                Id = employee.Id,
                Username = employee.Username,
                ManagerId = employee.ManagerId,
                Role = employee.Role.ToString(),
                LeaveApplications = GetLeaveApplications(employee.Id)
            };

            return Ok(response);
        }

        private List<LeaveApplicationResponseDTO> GetLeaveApplications(int employeeId)
        {
            List<LeaveApplication> leaveApplications = _allLeaveApplications.Where(x => x.EmployeeId == employeeId).ToList();
            return leaveApplications.Select(leaveApplication => new LeaveApplicationResponseDTO
            {
                Id = leaveApplication.Id,
                Status = leaveApplication.Status.ToString(),
                Reason = leaveApplication.Reason,
                StartDate = leaveApplication.StartDate,
                EndDate = leaveApplication.EndDate
            }).ToList();
        }


        [HttpGet("{employeeId}/reportees")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetReportees([FromRoute] int employeeId)
        {
            int? id = int.Parse(User.Identity.Name);

            if(id!=employeeId)
                return Unauthorized() ;

            List<Employee> reportees = _employees.Where(x => x.ManagerId == id).ToList();
            List<EmployeeResponseDTO> response = reportees.Select(employee => new EmployeeResponseDTO
            {
                Id = employee.Id,
                Username = employee.Username,
                ManagerId = employee.ManagerId,
                Role = employee.Role.ToString()
            }).ToList();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult AddEmployee(EmployeeDTO employee)
        {
            int? managerId = int.Parse(User.Identity.Name);
            string roleString = User.FindFirst(ClaimTypes.Role)?.Value!;
            if (!Enum.TryParse(roleString, out Role role))
                return BadRequest("Invalid role");

            role = role == Role.Admin ? Role.Manager : Role.Reportee;

            if (_employees.Any(e => e.Username == employee.Username))
            {
                return Conflict("Username already exists");
            }

            Employee newEmployee = new()
            {
                Username = employee.Username,
                Password = employee.Password,
                Role = role,
                ManagerId = managerId
            };
            DataStore.Instance.Employees.Add(newEmployee);

            EmployeeResponseDTO response = new()
            {
                Id = newEmployee.Id,
                Username = newEmployee.Username,
                ManagerId = newEmployee.ManagerId,
                Role = newEmployee.Role.ToString()
            };
            return CreatedAtAction("Get", new { EmployeeId = newEmployee.Id }, response);
        }
    }
}
