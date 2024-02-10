using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public class LeaveApplicationService : ILeaveApplicationService
    {
        private readonly List<LeaveApplication> _leaveApplications = DataStore.Instance.LeaveApplications;

        public void Apply(int employeeId, LeaveApplication leaveApplication)
        {
            DataStore.Instance.LeaveApplications.Add(leaveApplication);
        }

        public void Approve(int employeeId, int leaveId)
        {
            var leaveApplication = _leaveApplications.FirstOrDefault(x => x.EmployeeId == employeeId && x.Id == leaveId);

            if (leaveApplication != null)
            {
                _leaveApplications.Remove(leaveApplication);
                leaveApplication.Status = LeaveStatus.Approved;
                DataStore.Instance.LeaveApplications.Add(leaveApplication);
            }
            else
            {
                throw new Exception("Leave application not found");
            }
        }

        public LeaveApplication GetDetails(int employeeId, int leaveId)
        {
            return _leaveApplications.FirstOrDefault(x => x.EmployeeId == employeeId && x.Id == leaveId)!;
        }
    }
}
