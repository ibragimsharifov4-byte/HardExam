using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ILeaveReguestRepository
{
    Task<List<LeaveRequest>> GetLeaveRequests();
    Task<bool> AddLeaveRequest(LeaveRequest leaveRequest);
    Task<bool> ChangeStatus(int id , LeaveRequest leaveRequest);
}
