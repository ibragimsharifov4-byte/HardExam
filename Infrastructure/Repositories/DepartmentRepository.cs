using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Dapper;
namespace Infrastructure.Repositories;

public class DepartmentRepository(Database database) : IDepartmentRepository
{
    private Database _data = database;
    public async Task<bool> AddDepartment(Department department)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "insert into departments (name, description, createdat) values (@name, @description, @createdat)";
        var res = await con.ExecuteAsync(query, department);
        return res > 0;
    }

    public async Task<bool> DeleteDepartment(int id)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "delete from departments where id = @id";
        var res = await con.ExecuteAsync(query, new { id });
        return res > 0;
    }

    public async Task<Department?> GetDepartmentById(int id)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "select * from departments where id = @id";
        return await con.QueryFirstOrDefaultAsync<Department>(query, new { id });
    }

    public async Task<List<Department>> GetDepartments()
    {
        using var con = _data.Connection();
        con.Open();
        var query = "select * from departments";
        return (await con.QueryAsync<Department>(query)).ToList();
    }

    public async Task<bool> UpdateDepartment(Department department)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "update departments set name = @name, description = @description where id = @id";
        var res = await con.ExecuteAsync(query, department);
        return res > 0;
    }

}
