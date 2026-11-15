using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(IEmployeeService employeeService , ILogger<EmployeeController> logger) : ControllerBase
{
    private IEmployeeService _employeeService = employeeService;
    private ILogger _logger = logger;

    [HttpGet("/api/employeesdepartmentId=&positionId=&search=&page=&pageSize=")]
    public async Task<ApiResponse<List<GetEmployeeDto>>> GetEmployeesByFilter()
    {
        _logger.LogInformation("GetEmployees by filtering called");
        return await _employeeService.GetEmployeesByFilter();
    }

    [HttpGet]
    public async Task<ApiResponse<List<GetEmployeeDto>>> GetEmployees()
    {
        _logger.LogInformation("GetEmployees called");
        return await _employeeService.GetEmployees();
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetEmployeeByIdDto>> GetEmployeeById(int id)
    {
        _logger.LogInformation($"GetEmployeeById called with id: {id}");
        return await _employeeService.GetEmployeeById(id);
    }

    [HttpPost]
    public async Task<ApiResponse<bool>> AddEmployee(CreateEmployeeDto employeeDto)
    {
        _logger.LogInformation("AddEmployee called");
        var res = await _employeeService.AddEmployee(employeeDto);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employeeDto)
    {
        _logger.LogInformation($"UpdateEmployee called with id: {id}");
        var res = await _employeeService.UpdateEmployee(id, employeeDto);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> DeleteEmployee(int id)
    {
        _logger.LogInformation($"DeleteEmployee called with id: {id}");
        return await _employeeService.DeleteEmployee(id);
    }

    [HttpPatch]
    public async Task<ApiResponse<bool>> PatchEmployee(UpdateEmployeeDto employeeDto)
    {
        _logger.LogInformation("PatchEmloyee called");
        return await _employeeService.PatchEmployee(employeeDto);
    }
}
