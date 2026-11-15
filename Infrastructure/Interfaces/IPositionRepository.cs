using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IPositionRepository
{
    Task<List<Position>> GetPositions();
    Task<Position?> GetPositionById(int id);
    Task<bool> AddPosition(Position position);
}