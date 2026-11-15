using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Interfaces;
using System.Net;

namespace Application.Services;

public class LeaveRequestService(ILeaveReguestRepository leaveRequestRepository, IEmployeeRepository employeeRepository) : ILeaveRequestService
{
    private ILeaveReguestRepository _leaveRequestRepository = leaveRequestRepository;
    private IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<ApiResponse<bool>> AddLeaveRequest(CreateLeaveRequestDto leaveRequestDto)
    {
        var leaveRequest = new LeaveRequest
        {
            EmployeeId = leaveRequestDto.EmployeeId,
            LeaveType = leaveRequestDto.LeaveType,
            StartDate = leaveRequestDto.StartDate,
            EndDate = leaveRequestDto.EndDate,
            Reason = leaveRequestDto.Reason,
            Status = LeaveStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        };

        var res = await _leaveRequestRepository.AddLeaveRequest(leaveRequest);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Leave request added successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to add leave request", false);
    }

    public async Task<ApiResponse<List<GetLeaveRequestDto>>> GetLeaveRequests()
    {
        var res = await _leaveRequestRepository.GetLeaveRequests();
        if (res == null)
            return new ApiResponse<List<GetLeaveRequestDto>>(HttpStatusCode.NotFound, "Leave requests not found", null!);

        var employeeNames = (await _employeeRepository.GetEmployees()).ToDictionary(e => e.Id, e => $"{e.FirstName} {e.LastName}");
        var leaveRequests = new List<GetLeaveRequestDto>();
        foreach (var leaveRequest in res)
        {
            leaveRequests.Add(new GetLeaveRequestDto
            {
                Id = leaveRequest.Id,
                EmployeeId = leaveRequest.EmployeeId,
                EmployeeName = employeeNames.GetValueOrDefault(leaveRequest.EmployeeId, ""),
                LeaveType = leaveRequest.LeaveType,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Status = leaveRequest.Status,
                Reason = leaveRequest.Reason,
                CreatedAt = leaveRequest.CreatedAt
            });
        }
        return new ApiResponse<List<GetLeaveRequestDto>>(HttpStatusCode.OK, "Leave requests retrieved successfully", leaveRequests);
    }

    public async Task<ApiResponse<bool>> ApproveLeaveRequest(int id)
    {
        var res = await _leaveRequestRepository.ChangeStatus(id, new LeaveRequest { Status = LeaveStatus.Approved });
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Leave request approved", true)
        : new ApiResponse<bool>(HttpStatusCode.NotFound, "Leave request not found", false);
    }

    public async Task<ApiResponse<bool>> RejectLeaveRequest(int id)
    {
        var res = await _leaveRequestRepository.ChangeStatus(id, new LeaveRequest { Status = LeaveStatus.Rejected });
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Leave request rejected", true)
        : new ApiResponse<bool>(HttpStatusCode.NotFound, "Leave request not found", false);
    }
}
