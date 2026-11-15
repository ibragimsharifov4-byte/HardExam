using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployees();
    Task<Employee?> GetEmployeeById(int id);
    Task<bool> AddEmployee(Employee employee);
    Task<bool> UpdateEmployee(Employee employee);
    Task<bool> DeleteEmployee(int id);
    Task<List<Employee>> GetEmployeesByFilter();
    Task<bool> PatchEmployee(Employee employee);
}