using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Domain.Models;
using Infrastructure.Interfaces;
using System.Net;

namespace Application.Services;

public class EmployeeService(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IPositionRepository positionRepository) : IEmployeeService
{
    private IEmployeeRepository _employeeRepository = employeeRepository;
    private IDepartmentRepository _departmentRepository = departmentRepository;
    private IPositionRepository _positionRepository = positionRepository;

    public async Task<ApiResponse<bool>> AddEmployee(CreateEmployeeDto employeeDto)
    {
        var error = await ValidateEmployee(employeeDto.Email, employeeDto.HireDate, employeeDto.Salary,
            employeeDto.DepartmentId, employeeDto.PositionId, null);
        if (error != null)
            return error;

        var employee = new Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            Phone = employeeDto.Phone!,
            HireDate = employeeDto.HireDate,
            Salary = employeeDto.Salary,
            DepartmentId = employeeDto.DepartmentId,
            PositionId = employeeDto.PositionId
        };

        var res = await _employeeRepository.AddEmployee(employee);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Employee added successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to add employee", false);
    }

    public async Task<ApiResponse<bool>> DeleteEmployee(int id)
    {
        var res = await _employeeRepository.DeleteEmployee(id);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Employee deleted successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to delete employee", false);
    }

    public async Task<ApiResponse<GetEmployeeByIdDto>> GetEmployeeById(int id)
    {
        var res = await _employeeRepository.GetEmployeeById(id);
        if (res == null)
            return new ApiResponse<GetEmployeeByIdDto>(HttpStatusCode.NotFound, "Employee not found", null!);

        var department = await _departmentRepository.GetDepartmentById(res.DepartmentId);
        var position = await _positionRepository.GetPositionById(res.PositionId);

        var employeeDto = new GetEmployeeByIdDto
        {
            Id = res.Id,
            FirstName = res.FirstName,
            LastName = res.LastName,
            Email = res.Email,
            Phone = res.Phone,
            HireDate = res.HireDate,
            Salary = res.Salary,
            IsActive = res.IsActive,
            DepartmentId = res.DepartmentId,
            DepartmentName = department?.Name ?? "",
            PositionId = res.PositionId,
            PositionTitle = position?.Title ?? ""
        };
        return new ApiResponse<GetEmployeeByIdDto>(HttpStatusCode.OK, "Employee retrieved successfully", employeeDto);
    }

    public async Task<ApiResponse<List<GetEmployeeDto>>> GetEmployees()
    {
        var res = await _employeeRepository.GetEmployees();
        if (res == null)
            return new ApiResponse<List<GetEmployeeDto>>(HttpStatusCode.NotFound, "Employees not found", null!);

        var employees = await ToGetEmployeeDtos(res);
        return new ApiResponse<List<GetEmployeeDto>>(HttpStatusCode.OK, "Employees retrieved successfully", employees);
    }

    public async Task<ApiResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employeeDto)
    {
        var error = await ValidateEmployee(employeeDto.Email, employeeDto.HireDate, employeeDto.Salary,
            employeeDto.DepartmentId, employeeDto.PositionId, id);
        if (error != null)
            return error;

        var employee = new Employee
        {
            Id = id,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            Phone = employeeDto.Phone!,
            HireDate = employeeDto.HireDate,
            Salary = employeeDto.Salary,
            DepartmentId = employeeDto.DepartmentId,
            PositionId = employeeDto.PositionId
        };
        var res = await _employeeRepository.UpdateEmployee(employee);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Employee updated successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to update employee", false);
    }

    public async Task<ApiResponse<List<GetEmployeeDto>>> GetEmployeesByFilter()
    {
        var res = await _employeeRepository.GetEmployeesByFilter();
        if (res == null)
        return new ApiResponse<List<GetEmployeeDto>>(HttpStatusCode.NotFound, "Employees not found", null!);
        var employees = await ToGetEmployeeDtos(res);
        return new ApiResponse<List<GetEmployeeDto>>(HttpStatusCode.OK, "Employees retrieved successfully", employees);
    }

    public async Task<ApiResponse<bool>> PatchEmployee(UpdateEmployeeDto employeeDto)
    {
        var employee = new Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            Phone = employeeDto.Phone!,
            HireDate = employeeDto.HireDate,
            Salary = employeeDto.Salary,
            DepartmentId = employeeDto.DepartmentId,
            PositionId = employeeDto.PositionId
        };
        var res = await _employeeRepository.UpdateEmployee(employee);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Employee updated successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to update employee", false);
    }

    // Проверки по порядку: 404 -> 400 -> 409. Возвращает null, если всё в порядке.
    private async Task<ApiResponse<bool>?> ValidateEmployee(string email, DateTime hireDate, decimal salary,
        int departmentId, int positionId, int? excludeId)
    {
        var department = await _departmentRepository.GetDepartmentById(departmentId);
        if (department == null)
            return new ApiResponse<bool>(HttpStatusCode.NotFound, "Department not found", false);

        var position = await _positionRepository.GetPositionById(positionId);
        if (position == null)
            return new ApiResponse<bool>(HttpStatusCode.NotFound, "Position not found", false);

        if (hireDate.Date > DateTime.UtcNow.Date)
            return new ApiResponse<bool>(HttpStatusCode.BadRequest, "HireDate cannot be in the future", false);

        if (salary < position.MinSalery || salary > position.MaxSalery)
            return new ApiResponse<bool>(HttpStatusCode.BadRequest,
                $"Salary must be between {position.MinSalery} and {position.MaxSalery}", false);

        var allEmployees = await _employeeRepository.GetEmployees();
        if (allEmployees.Any(e => e.Id != excludeId && string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase)))
            return new ApiResponse<bool>(HttpStatusCode.Conflict, "Employee with this email already exists", false);

        return null;
    }

    // Названия отдела и должности берём через уже существующие методы репозиториев
    private async Task<List<GetEmployeeDto>> ToGetEmployeeDtos(List<Employee> employees)
    {
        var departments = (await _departmentRepository.GetDepartments()).ToDictionary(d => d.Id, d => d.Name);
        var positions = (await _positionRepository.GetPositions()).ToDictionary(p => p.Id, p => p.Title);

        return employees.Select(e => new GetEmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            HireDate = e.HireDate,
            Salary = e.Salary,
            IsActive = e.IsActive,
            DepartmentId = e.DepartmentId,
            DepartmentName = departments.GetValueOrDefault(e.DepartmentId, ""),
            PositionId = e.PositionId,
            PositionTitle = positions.GetValueOrDefault(e.PositionId, "")
        }).ToList();
    }
}
