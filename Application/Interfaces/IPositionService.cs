using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IPositionService
{
    Task<ApiResponse<List<GetPositionDto>>> GetPositions();
    Task<ApiResponse<GetPositionByIdDto>> GetPositionById(int id);
    Task<ApiResponse<bool>> AddPosition(CreatePositionDto positionDto);
}
