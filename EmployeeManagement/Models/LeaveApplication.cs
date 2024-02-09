namespace EmployeeManagement.Models
{
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class LeaveApplication
    {
        private static int _lastLeaveApplicationId = 100;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeaveApplication()
        {
            Id = Interlocked.Increment(ref _lastLeaveApplicationId);
        }
    }
}
