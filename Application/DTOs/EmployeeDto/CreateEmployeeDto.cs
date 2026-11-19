using Domain.Models;

namespace Application.DTOs;

public class CreateEmployeeDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public Position? position { get; set; }
}
