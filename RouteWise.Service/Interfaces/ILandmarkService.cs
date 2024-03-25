﻿using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Interfaces;

public interface ILandmarkService
{
    Task<int> GetLandmarkIdOrDefaultAsync(TrailerStateDto dto);
}
