using EmployeeManagement.Models;

namespace EmployeeManagement.Dtos
{
    public class LeaveApplicationResponseDTO
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Reason { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
