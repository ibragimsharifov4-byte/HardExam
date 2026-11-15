using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Data;
using Dapper;

namespace Infrastructure.Repositories;

public class EmployeeRepository(Database database) : IEmployeeRepository
{
    private Database _data = database;
    public async Task<bool> AddEmployee(Employee employee)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "insert into employees(firstname , lastname , email , phone , hiredate , salary , departmentid , positionid) values (@firstname , @lastname , @email , @phone, @hiredate , @salary , @departmentid , @positionid)";
        var res = await con.ExecuteAsync(query, employee);
        return res > 0;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "delete from employees where id = @id";
        var res = await con.ExecuteAsync(query, new { id });
        return res > 0;
    }

    public async Task<Employee?> GetEmployeeById(int id)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "select * from employees where id = @id";
        var res = await con.QueryFirstOrDefaultAsync<Employee>(query, new { id });
        return res;
    }

    public async Task<List<Employee>> GetEmployees()
    {
        using var con = _data.Connection();
        con.Open();
        var query = "select * from employees";
        var res = await con.QueryAsync<Employee>(query);
        return res.ToList();
    }

    public async Task<bool> UpdateEmployee(Employee employee)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "update employees set firstname = @firstname, lastname = @lastname, email = @email, phone = @phone, hiredate = @hiredate, salary = @salary, departmentid = @departmentid, positionid = @positionid where id = @id";
        var res = await con.ExecuteAsync(query, employee);
        return res > 0;
    }

    public async Task<List<Employee>> GetEmployeesByFilter()
    {
        using var con = _data.Connection();
        con.Open();
        var query = @"select e.firstname , e.lastname , p.title , d.name 
        from employees e
        join positions p on e.positionid = p.id
        join departments d on e.departmentid = d.id
        order by e.firstname";
        var res = await con.QueryAsync<Employee>(query);
        return res.ToList();
    }

    public async Task<bool> PatchEmployee(Employee employee)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "update employee set IsActive = false where id = @id";
        var res = await con.ExecuteAsync(query, employee);
        return res > 0;
    }
}
