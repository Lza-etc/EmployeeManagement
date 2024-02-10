using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface ILeaveApplicationService
    {
        void Apply(int employeeId, LeaveApplication leaveApplicationDto);
        LeaveApplication GetDetails(int employeeId, int leaveId);
        void Approve(int employeeId, int leaveId );
    }
}
