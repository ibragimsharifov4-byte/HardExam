using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Domain.Models;
using Infrastructure.Interfaces;
using System.Net;

namespace Application.Services;

public class PositionService(IPositionRepository positionRepository) : IPositionService
{
    private IPositionRepository _positionRepository = positionRepository;

    public async Task<ApiResponse<bool>> AddPosition(CreatePositionDto positionDto)
    {
        if (positionDto.MaxSalary < positionDto.MinSalary)
            return new ApiResponse<bool>(HttpStatusCode.BadRequest, "MaxSalary cannot be less than MinSalary", false);

        if ((await _positionRepository.GetPositions()).Any(p => string.Equals(p.Title, positionDto.Title, StringComparison.OrdinalIgnoreCase)))
            return new ApiResponse<bool>(HttpStatusCode.Conflict, "Position with this title already exists", false);

        var position = new Position
        {
            Title = positionDto.Title,
            MinSalery = positionDto.MinSalary,
            MaxSalery = positionDto.MaxSalary
        };

        var res = await _positionRepository.AddPosition(position);
        return res
        ? new ApiResponse<bool>(HttpStatusCode.OK, "Position added successfully", true)
        : new ApiResponse<bool>(HttpStatusCode.InternalServerError, "Failed to add position", false);
    }

    public async Task<ApiResponse<GetPositionByIdDto>> GetPositionById(int id)
    {
        var res = await _positionRepository.GetPositionById(id);
        if (res == null)
            return new ApiResponse<GetPositionByIdDto>(HttpStatusCode.NotFound, "Position not found", null!);

        var positionDto = new GetPositionByIdDto
        {
            Id = res.Id,
            Title = res.Title,
            MinSalary = res.MinSalery,
            MaxSalary = res.MaxSalery
        };
        return new ApiResponse<GetPositionByIdDto>(HttpStatusCode.OK, "Position retrieved successfully", positionDto);
    }

    public async Task<ApiResponse<List<GetPositionDto>>> GetPositions()
    {
        var res = await _positionRepository.GetPositions();
        if (res == null)
            return new ApiResponse<List<GetPositionDto>>(HttpStatusCode.NotFound, "Positions not found", null!);

        var positions = new List<GetPositionDto>();
        foreach (var position in res)
        {
            positions.Add(new GetPositionDto
            {
                Id = position.Id,
                Title = position.Title,
                MinSalary = position.MinSalery,
                MaxSalary = position.MaxSalery
            });
        }
        return new ApiResponse<List<GetPositionDto>>(HttpStatusCode.OK, "Positions retrieved successfully", positions);
    }
}
