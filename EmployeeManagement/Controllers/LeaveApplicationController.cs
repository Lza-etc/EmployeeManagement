using System;
using EmployeeManagement.Attributes;
using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("employees/{employeeId}/leave")]
    [ApiController]
    public class LeaveApplicationController : ControllerBase
    {
        private readonly ILeaveApplicationService _leaveApplicationService;

        public LeaveApplicationController(ILeaveApplicationService leaveApplicationService)
        {
            _leaveApplicationService = leaveApplicationService;
        }

        [HttpPost]
        [Authorize]
        [EmployeeAuthorization("employeeId")]
        public IActionResult Apply([FromRoute] int employeeId, [FromBody] LeaveApplicationDTO leaveApplicationDto)
        {
            try
            {
                var leaveApplication = new LeaveApplication
                {
                    EmployeeId = employeeId,
                    Reason = leaveApplicationDto.Reason,
                    StartDate = leaveApplicationDto.StartDate,
                    EndDate = leaveApplicationDto.EndDate,
                    Status = LeaveStatus.Pending
                };

                _leaveApplicationService.Apply(employeeId, leaveApplication);

                return Ok("Leave application submitted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{leaveId}")]
        [Authorize]
        [EmployeeAuthorization("employeeId")]
        public IActionResult GetDetails([FromRoute] int employeeId, [FromRoute] int leaveId)
        {
            try
            {
                var leaveApplication = _leaveApplicationService.GetDetails(employeeId, leaveId);
                return Ok(leaveApplication);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{leaveId}")]
        [Authorize(Roles = "Manager")]
        [ReporteeManagerAuth("employeeId")]
        public IActionResult Approve([FromRoute] int employeeId, [FromRoute] int leaveId)
        {
            try
            {
                _leaveApplicationService.Approve(employeeId, leaveId);
                return Ok("Leave application approved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
