using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetDepartments();
    Task<Department?> GetDepartmentById(int id);
    Task<bool> AddDepartment(Department department);
    Task<bool> UpdateDepartment(Department department);
    Task<bool> DeleteDepartment(int id);
}
