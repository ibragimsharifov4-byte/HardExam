using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Dapper;

namespace Infrastructure.Repositories;

public class LeaveRequestRepository(Database database) : ILeaveReguestRepository
{
    private Database _data = database;
    public async Task<bool> AddLeaveRequest(LeaveRequest leaveRequest)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "insert into leaverequests(employeeid , leavetype, startdate , enddate , reason , status, createdat) values (@employeeid , @leavetype, @startdate , @enddate , @reason , @status, @createdat)";
        var res = await con.ExecuteAsync(query, leaveRequest);
        return res > 0;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequests()
    {
        using var con = _data.Connection();
        con.Open();
        var query = @"select lv.id, lv.employeeid, lv.leavetype, lv.startdate, lv.enddate,
        lv.status, lv.reason, lv.createdat,
        e.id, e.firstname, e.lastname, e.email
        from leaverequests lv
        join employees e on e.id = lv.employeeid
        order by lv.status";
        var res = await con.QueryAsync<LeaveRequest>(query);
        return res.ToList();
    }

    public async Task<bool> ChangeStatus(int id , LeaveRequest leaveRequest)
    {
        using var con = _data.Connection();
        con.Open();
        var query = "update leaverequests set status = @status where id = @id";
        var res = await con.ExecuteAsync(query , new{id , leaveRequest.Status});
        return res > 0;
    }

}
