using EmployeeManagement.Models;

namespace EmployeeManagement.Models
{
    public class DataStore
    {
        private static DataStore _instance;
        public static DataStore Instance => _instance ??= new DataStore();

        public List<Employee> Employees { get; } = new List<Employee>();
        public List<LeaveApplication> LeaveApplications { get; } = new List<LeaveApplication>();
        private DataStore()
        {
           Employees.Add(new Employee{ Username = "elsa", Password= "elsa@2002", Role = Role.Admin });
        }
    }
}
