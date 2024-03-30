﻿using AutoMapper;
using Newtonsoft.Json.Linq;
using RouteWise.Service.DTOs.Trailer;

namespace RouteWise.Service.Helpers;

public class TrailerDateTimeResolver : IValueResolver<JToken, TrailerStateDto, DateTime>
{
    public DateTime Resolve(JToken source, TrailerStateDto destination, DateTime destMember, ResolutionContext context)
    {
        return !string.IsNullOrEmpty(source.Value<string>("eventDateTime"))
            ? ConvertToEdt(DateTime.ParseExact(source.Value<string>("eventDateTime"), "yyyy-MM-dd HH:mm:ss", null)) // Default DateTime in FleetLocate is UTC
            : DateTime.ParseExact($"{source.SelectToken("lastEvent.messageDate")}", "MM-dd-yyyy HH:mm:ss", null);   // Default DateTime in RoadReady is EDT
    }

    private DateTime ConvertToEdt(DateTime utc) 
        => TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
}