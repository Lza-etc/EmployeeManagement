using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("employees/{employeeId}/leave")]
    [ApiController]
    public class LeaveApplicationController : ControllerBase
    {

        List<LeaveApplication> leaveApplications = DataStore.Instance.LeaveApplications;
        List<Employee> employees = DataStore.Instance.Employees;

        [HttpPost]
        [Authorize]
        public IActionResult Apply([FromRoute] int employeeId, [FromBody] LeaveApplicationDTO leaveApplicationDto)
        {
            int id = int.Parse(User.Identity.Name);
            if (id != employeeId)
                return Unauthorized();

            LeaveApplication leaveApplication = new()
             {
                EmployeeId = id,
                Reason= leaveApplicationDto.Reason,
                StartDate= leaveApplicationDto.StartDate,
                EndDate= leaveApplicationDto.EndDate,
                Status=LeaveStatus.Pending
             };
            DataStore.Instance.LeaveApplications.Add(leaveApplication);

            return Ok(leaveApplication);
        }

        [HttpGet("{leaveId}")]
        [Authorize]
        public IActionResult Apply([FromRoute] int employeeId, [FromRoute] int leaveId)
        {
            int id = int.Parse(User.Identity.Name);
            if (id != employeeId)
                return Unauthorized();
            LeaveApplication leaveApplication = leaveApplications.Where(x => x.EmployeeId == employeeId && x.Id == leaveId).FirstOrDefault();

            return Ok(leaveApplication);
        }

        [HttpPut("{leaveId}")]
        [Authorize(Roles ="Manager")]
        public IActionResult Approve([FromRoute] int employeeId, [FromRoute] int leaveId)
        {
            int id = int.Parse(User.Identity.Name);

             Employee employee=employees.Where(x=>x.Id==employeeId).FirstOrDefault();
            if (id != employee.ManagerId)
                return Unauthorized();

            LeaveApplication leaveApplication=leaveApplications.Where(x=>x.EmployeeId==employeeId && x.Id==leaveId).FirstOrDefault();

            if (leaveApplication != null)
            {
                leaveApplications.Remove(leaveApplication);
                leaveApplication.Status = LeaveStatus.Approved;
                DataStore.Instance.LeaveApplications.Add(leaveApplication);
                return Ok("Leave application approved successfully");
            }
            else
            {
                return NotFound("Leave application not found");
            }
        }
    }
}
