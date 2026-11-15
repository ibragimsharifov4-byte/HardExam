namespace Domain.Models;

public class Position
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public Decimal MinSalery { get; set; }
    public Decimal MaxSalery { get; set; }
}
