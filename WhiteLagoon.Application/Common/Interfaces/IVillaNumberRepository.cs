﻿using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    public void Update(VillaNumber villaNumber);
    public void Save();
}