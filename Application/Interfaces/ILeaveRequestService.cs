using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface ILeaveRequestService
{
    Task<ApiResponse<List<GetLeaveRequestDto>>> GetLeaveRequests();
    Task<ApiResponse<bool>> AddLeaveRequest(CreateLeaveRequestDto leaveRequestDto);
    Task<ApiResponse<bool>> ApproveLeaveRequest(int id);
    Task<ApiResponse<bool>> RejectLeaveRequest(int id);
}
