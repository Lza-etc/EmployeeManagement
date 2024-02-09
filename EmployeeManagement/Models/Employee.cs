namespace EmployeeManagement.Models
{
    public enum Role
    {
        Reportee,
        Manager,
        Admin
    }

    public class Employee
    {
        private static int _lastEmployeeId = 100;

        public int Id { get; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }
        public int? ManagerId { get; set; } = null;

        public Employee()
        {
            Id = Interlocked.Increment(ref _lastEmployeeId);
        }
    }
}

