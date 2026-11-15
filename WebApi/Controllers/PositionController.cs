using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionController(IPositionService positionService , ILogger<PositionController> logger) : ControllerBase
{
    private IPositionService _positionService = positionService;
    private ILogger _logger = logger;
    
    [HttpGet]
    public async Task<ApiResponse<List<GetPositionDto>>> GetPositions()
    {
        _logger.LogInformation("GetPositions called");
        return await _positionService.GetPositions();
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<GetPositionByIdDto>> GetPositionById(int id)
    {
        _logger.LogInformation($"GetPositionById called with id: {id}");
        return await _positionService.GetPositionById(id);
    }

    [HttpPost]
    public async Task<ApiResponse<bool>> AddPosition(CreatePositionDto positionDto)
    {
        _logger.LogInformation("AddPosition called");
        var res = await _positionService.AddPosition(positionDto);
        Response.StatusCode = (int)res.StatusCode;
        return res;
    }

}
