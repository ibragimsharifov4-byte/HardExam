using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext appDbContext) : IDepartmentRepository
{
    private AppDbContext _context = appDbContext;

    public async Task<bool> AddDepartment(Department department)
    {
        _context.Departments.Add(department);
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<bool> DeleteDepartment(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
        if (department == null)
            return false;

        _context.Departments.Remove(department);
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<Department?> GetDepartmentById(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
        return department;
    }

    public async Task<List<Department>> GetDepartments()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<bool> UpdateDepartment(Department department)
    {
        var existing = await _context.Departments.FirstOrDefaultAsync(x => x.Id == department.Id);
        if (existing == null)
            return false;

        existing.Name = department.Name;
        existing.Description = department.Description;
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }
}
