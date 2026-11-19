using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Department
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public List<Employee> employees { get; set; } = new();
}
