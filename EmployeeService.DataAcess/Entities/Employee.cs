namespace EmployeeService.DataAcess.Entities
{
    public class Employee
    {
        public Employee()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime When { get; set; }
    }
}
