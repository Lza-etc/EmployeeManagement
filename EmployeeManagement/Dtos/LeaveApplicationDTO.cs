using EmployeeManagement.Models;

namespace EmployeeManagement.Dtos
{
    public class LeaveApplicationDTO
    {
        public string? Reason { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
