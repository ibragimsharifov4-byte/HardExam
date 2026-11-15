using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Dapper;

namespace Infrastructure.Repositories;

public class PositionRepsoitory(Database database) : IPositionRepository
{
    private Database _data = database;

    public async Task<bool> AddPosition(Position position)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "insert into positions(title , minsalery , maxsalery) values (@Title , @MinSalery , @MaxSalery)";
        var res = await con.ExecuteAsync(query, position);
        return res > 0;
    }

    public async Task<Position?> GetPositionById(int id)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "select * from positions where id = @id";
        var res = await con.QueryFirstOrDefaultAsync<Position>(query , new{id});
        return res;
    }

    public async Task<List<Position>> GetPositions()
    {
        using var con = _data.Connection();
        con.Open();
        var query = "select * from positions";
        var res = await con.QueryAsync<Position>(query);
        return res.ToList();
    }
}
