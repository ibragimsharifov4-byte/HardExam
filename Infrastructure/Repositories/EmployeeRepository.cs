using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext appDbContext) : IEmployeeRepository
{
    private AppDbContext _context = appDbContext;

    public async Task<bool> AddEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (employee == null)
            return false;

        _context.Employees.Remove(employee);
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<Employee?> GetEmployeeById(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        return employee;
    }

    public async Task<List<Employee>> GetEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<bool> UpdateEmployee(Employee employee)
    {
        var existing = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
        if (existing == null)
            return false;

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Email = employee.Email;
        existing.Phone = employee.Phone;
        existing.HireDate = employee.HireDate;
        existing.Salary = employee.Salary;
        existing.DepartmentId = employee.DepartmentId;
        existing.PositionId = employee.PositionId;
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<List<Employee>> GetEmployeesByFilter()
    {
        var res = await (from employee in _context.Employees
                         join position in _context.Positions on employee.PositionId equals position.Id
                         join department in _context.Departments on employee.DepartmentId equals department.Id
                         orderby employee.FirstName
                         select employee).ToListAsync();
        return res;
    }

    public async Task<bool> PatchEmployee(Employee employee)
    {
        var existing = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
        if (existing == null)
            return false;

        existing.IsActive = false;
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }
}
