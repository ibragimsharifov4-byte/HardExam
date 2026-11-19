using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Position
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public Decimal MinSalery { get; set; }
    public Decimal MaxSalery { get; set; }
    public List<Employee> employees { get; set; } = new();
}
