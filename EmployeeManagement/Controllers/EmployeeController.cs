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
        List<Employee> employees = DataStore.Instance.Employees;
        List<LeaveApplication> allLeaveApplications = DataStore.Instance.LeaveApplications;

        [HttpGet("{employeeId}")]
        [Authorize]
        public IActionResult Get([FromRoute] int employeeId)
        {
            int? id = int.Parse(User.Identity.Name);

            Employee employee = employees.Where(x => x.Id == employeeId).FirstOrDefault();
            //|| (employee != null && employee.ManagerId != id)

            if (id != employeeId)
                return Unauthorized();

            EmployeeResponseDTO response = new()
            {
                Id = employee.Id,
                Username = employee.Username,
                ManagerId = employee.ManagerId,
                Role = employee.Role.ToString(),
            };
            List<LeaveApplication> leaveApplications = allLeaveApplications.Where(x => x.EmployeeId == id).ToList();
            List<LeaveApplicationResponseDTO> leaveApplicationsResponse = new();
            foreach(LeaveApplication leaveApplication in leaveApplications)
            {
                leaveApplicationsResponse.Add(new LeaveApplicationResponseDTO
                {
                    Id= leaveApplication.Id,
                    Status = leaveApplication.Status.ToString(),
                    Reason = leaveApplication.Reason,
                    StartDate = leaveApplication.StartDate,
                    EndDate = leaveApplication.EndDate,

                });
            }
            if (leaveApplications != null)
                response.LeaveApplications = leaveApplicationsResponse;

            return Ok(response);
        }


        [HttpGet("{employeeId}/reportees")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetReportees([FromRoute] int employeeId)
        {
            int? id = int.Parse(User.Identity.Name);
            List<Employee> reportees = employees.Where(x => x.ManagerId == id).ToList();
            List<EmployeeResponseDTO> response= new();
            foreach (Employee employee in reportees)
            {
                response.Add(new EmployeeResponseDTO
                {
                    Id = employee.Id,
                    Username = employee.Username,
                    ManagerId = employee.ManagerId,
                    Role = employee.Role.ToString(),
                });
            }
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult AddEmployee([FromBody] EmployeeDTO employee)
        {
            int? managerId = int.Parse(User.Identity.Name);
            var roleClaim = User.FindFirst(ClaimTypes.Role);

            if (roleClaim != null)
            {
                string roleString = roleClaim.Value;
                if (Enum.TryParse<Role>(roleString, out Role role)) 
                {
                    role=role==Role.Admin ? Role.Manager : Role.Reportee;
                    Employee newEmployee = new() { Username = employee.Username, Password = employee.Password, Role = role, ManagerId = managerId };
                    DataStore.Instance.Employees.Add(newEmployee);

                    EmployeeResponseDTO response = new()
                    {
                        Id = newEmployee.Id,
                        Username = newEmployee.Username,
                        ManagerId = newEmployee.ManagerId,
                        Role = newEmployee.Role.ToString(),
                    };
                    return CreatedAtAction("Get", new { EmployeeId = newEmployee.Id }, response);
                }
                else
                    return BadRequest("Invalid role");
            }
            else
                return Unauthorized();
        }

    }
}
