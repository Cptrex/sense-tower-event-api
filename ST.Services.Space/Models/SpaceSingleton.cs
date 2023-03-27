﻿using ST.Services.Space.Interfaces;

namespace ST.Services.Space.Models;

public class SpaceSingleton : ISpaceSingleton
{
    public List<Guid> Spaces { get; set; } = new()
    {
        new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
    };
}