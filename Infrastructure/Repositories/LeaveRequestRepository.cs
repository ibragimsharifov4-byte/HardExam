using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LeaveRequestRepository(AppDbContext appDbContext) : ILeaveReguestRepository
{
    private AppDbContext _context = appDbContext;

    public async Task<bool> AddLeaveRequest(LeaveRequest leaveRequest)
    {
        _context.LeaveRequests.Add(leaveRequest);
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequests()
    {
        var res = await (from leaveRequest in _context.LeaveRequests
                         join employee in _context.Employees on leaveRequest.EmployeeId equals employee.Id
                         orderby leaveRequest.Status
                         select leaveRequest).ToListAsync();
        return res;
    }

    public async Task<bool> ChangeStatus(int id, LeaveRequest leaveRequest)
    {
        var existing = await _context.LeaveRequests.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
            return false;

        existing.Status = leaveRequest.Status;
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }
}
