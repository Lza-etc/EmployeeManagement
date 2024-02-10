using EmployeeManagement.Models;

namespace EmployeeManagement.Dtos
{
    public class EmployeeResponseDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public int? ManagerId { get; set; } = null;
        public List<LeaveApplicationResponseDTO> LeaveApplications { get; set; }= new List<LeaveApplicationResponseDTO>();
    }
}
