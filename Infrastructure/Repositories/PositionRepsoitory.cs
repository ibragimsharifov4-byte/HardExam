using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PositionRepsoitory(AppDbContext appDbContext) : IPositionRepository
{
    private AppDbContext _context = appDbContext;

    public async Task<bool> AddPosition(Position position)
    {
        _context.Positions.Add(position);
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<Position?> GetPositionById(int id)
    {
        var position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
        return position;
    }

    public async Task<List<Position>> GetPositions()
    {
        return await _context.Positions.ToListAsync();
    }
}
