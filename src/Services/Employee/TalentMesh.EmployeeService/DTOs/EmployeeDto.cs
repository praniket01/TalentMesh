public class EmployeeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string Designation { get; set; } = "";

    public decimal ExperienceYears { get; set; }

    public string Location { get; set; } = "";

    public int AllocationPercentage { get; set; }

    public List<string> Skills { get; set; } = [];
}