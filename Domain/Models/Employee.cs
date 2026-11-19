using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    [MaxLength(12)]
    public string Phone { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; } = true;
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(Department))]
    public Department? department { get; set; }
    public int PositionId { get; set; }
    [ForeignKey(nameof(Position))]
    public Position? position { get; set; }
    public List<LeaveRequest> requests { get; set; } = new();
}