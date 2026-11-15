using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService departmentService , ILogger<DepartmentController> logger) : ControllerBase
{
    private IDepartmentService _departmentService = departmentService;
    private ILogger _logger = logger;
    [HttpGet]
    public async Task<ApiResponse<List<GetDepartmentDto>>> GetDepartments()
    {
        _logger.LogInformation("GetDepartments called");
        return await _departmentService.GetDepartments();
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetDepartmentByIdDto>> GetDepartmentById(int id)
    {
        _logger.LogInformation($"GetDepartmentById called with id: {id}");
        return await _departmentService.GetDepartmentById(id);
    }

    [HttpPost]
    public async Task<ApiResponse<bool>> AddDepartment(CreateDepartmentDto departmentDto)
    {
        _logger.LogInformation("AddDepartment called");
        var res = await _departmentService.AddDepartment(departmentDto);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<bool>> UpdateDepartment(int id, UpdateDepartmentDto departmentDto)
    {
        _logger.LogInformation($"UpdateDepartment called with id: {id}");
        var res = await _departmentService.UpdateDepartment(id, departmentDto);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> DeleteDepartment(int id)
    {
        _logger.LogInformation($"DeleteDepartment called with id: {id}");
        var res = await _departmentService.DeleteDepartment(id);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }
}
