using Green_Lagoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Application.Common.Interface
{
    public interface IVillaNumber:IRepository<VillaNumber>
    {
       
        void Update(VillaNumber entity);
       
        void Save();

    }
}
