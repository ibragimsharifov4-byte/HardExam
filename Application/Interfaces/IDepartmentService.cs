using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IDepartmentService
{
    Task<ApiResponse<List<GetDepartmentDto>>> GetDepartments();
    Task<ApiResponse<GetDepartmentByIdDto>> GetDepartmentById(int id);
    Task<ApiResponse<bool>> AddDepartment(CreateDepartmentDto departmentDto);
    Task<ApiResponse<bool>> UpdateDepartment(int id, UpdateDepartmentDto departmentDto);
    Task<ApiResponse<bool>> DeleteDepartment(int id);
}
