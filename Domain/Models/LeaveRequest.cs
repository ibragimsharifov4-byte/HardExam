using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Domain.Enums;
namespace Domain.Models;

public class LeaveRequest
{
    [Key]
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    [ForeignKey(nameof(Employee))]
    public Employee? employee { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}