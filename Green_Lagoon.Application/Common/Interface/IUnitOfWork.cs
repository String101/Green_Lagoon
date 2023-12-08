﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Application.Common.Interface
{
    public interface IUnitOfWork
    {
        IVilla Villa { get; }
        IVillaNumber VillaNumber { get; }
        
    }
}
