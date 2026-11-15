using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestController(ILeaveRequestService leaveRequestService , ILogger<LeaveRequestController> logger) : ControllerBase
{
    private ILeaveRequestService _leaveRequestService = leaveRequestService;
    private ILogger _logger = logger;
    [HttpGet]
    public async Task<ApiResponse<List<GetLeaveRequestDto>>> GetLeaveRequests()
    {
        _logger.LogInformation("GetLeaveRequests called");
        return await _leaveRequestService.GetLeaveRequests();
    }

    [HttpPost]
    public async Task<ApiResponse<bool>> AddLeaveRequest(CreateLeaveRequestDto leaveRequestDto)
    {
        _logger.LogInformation("AddLeaveRequest called");
        return await _leaveRequestService.AddLeaveRequest(leaveRequestDto);
    }

    [HttpPost("{id}/approve")]
    public async Task<ApiResponse<bool>> ApproveLeaveRequest(int id)
    {
        _logger.LogInformation($"ApproveLeaveRequest called with id: {id}");
        var res = await _leaveRequestService.ApproveLeaveRequest(id);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }

    [HttpPost("{id}/reject")]
    public async Task<ApiResponse<bool>> RejectLeaveRequest(int id)
    {
        _logger.LogInformation($"RejectLeaveRequest called with id: {id}");
        var res = await _leaveRequestService.RejectLeaveRequest(id);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }
}
