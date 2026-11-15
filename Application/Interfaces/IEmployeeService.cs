using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IEmployeeService
{
    Task<ApiResponse<List<GetEmployeeDto>>> GetEmployees();
    Task<ApiResponse<GetEmployeeByIdDto>> GetEmployeeById(int id);
    Task<ApiResponse<bool>> AddEmployee(CreateEmployeeDto employeeDto);
    Task<ApiResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employeeDto);
    Task<ApiResponse<bool>> DeleteEmployee(int id);
    Task<ApiResponse<List<GetEmployeeDto>>> GetEmployeesByFilter();
    Task<ApiResponse<bool>> PatchEmployee(UpdateEmployeeDto employeeDto);
}
