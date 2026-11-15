using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Domain.Models;
using Infrastructure.Interfaces;
using System.Net;

namespace Application.Services;

public class DepartmentService(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository) : IDepartmentService
{
    private IDepartmentRepository _departmentRepository = departmentRepository;
    private IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<ApiResponse<bool>> AddDepartment(CreateDepartmentDto departmentDto)
    {
        if ((await _departmentRepository.GetDepartments()).Any(d => string.Equals(d.Name, departmentDto.Name, StringComparison.OrdinalIgnoreCase)))
            return new ApiResponse<bool>(HttpStatusCode.Conflict, "Department with this name already exists", false);

        var department = new Department
        {
            Name = departmentDto.Name,
            Description = departmentDto.Description!,
            CreatedAt = DateTime.UtcNow,
        };

        var res = await _departmentRepository.AddDepartment(department);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Department added successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to add department", false);
    }

    public async Task<ApiResponse<bool>> DeleteDepartment(int id)
    {
        if ((await _employeeRepository.GetEmployees()).Any(e => e.DepartmentId == id && e.IsActive))
            return new ApiResponse<bool>(HttpStatusCode.Conflict, "Department has active employees and cannot be deleted", false);

        var res = await _departmentRepository.DeleteDepartment(id);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Department deleted successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to delete department", false);
    }

    public async Task<ApiResponse<GetDepartmentByIdDto>> GetDepartmentById(int id)
    {
        var res = await _departmentRepository.GetDepartmentById(id);
        if (res == null)
            return new ApiResponse<GetDepartmentByIdDto>(HttpStatusCode.NotFound, "Department not found", null!);

        var departmentDto = new GetDepartmentByIdDto
        {
            Id = res.Id,
            Name = res.Name,
            Description = res.Description,
            CreatedAt = res.CreatedAt
        };
        return new ApiResponse<GetDepartmentByIdDto>(HttpStatusCode.OK, "Department retrieved successfully", departmentDto);
    }

    public async Task<ApiResponse<List<GetDepartmentDto>>> GetDepartments()
    {
        var res = await _departmentRepository.GetDepartments();
        if (res == null)
            return new ApiResponse<List<GetDepartmentDto>>(HttpStatusCode.NotFound, "Departments not found", null!);

        var departments = new List<GetDepartmentDto>();
        foreach (var department in res)
        {
            departments.Add(new GetDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt
            });
        }
        return new ApiResponse<List<GetDepartmentDto>>(HttpStatusCode.OK, "Departments retrieved successfully", departments);
    }

    public async Task<ApiResponse<bool>> UpdateDepartment(int id, UpdateDepartmentDto departmentDto)
    {
        if ((await _departmentRepository.GetDepartments()).Any(d => d.Id != id && string.Equals(d.Name, departmentDto.Name, StringComparison.OrdinalIgnoreCase)))
            return new ApiResponse<bool>(HttpStatusCode.Conflict, "Department with this name already exists", false);

        var department = new Department
        {
            Id = id,
            Name = departmentDto.Name,
            Description = departmentDto.Description!
        };
        var res = await _departmentRepository.UpdateDepartment(department);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Department updated successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to update department", false);
    }
}
