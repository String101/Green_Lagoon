
using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Infrastructure.Repositories
{
    public class VillaNumberRepo : Repository<VillaNumber>, IVillaNumber
    {

        private readonly ApplicationDbContext _context;
        public VillaNumberRepo(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

        

        

       public void Update(VillaNumber entity)
        {
            _context.VillaNumbers.Update(entity);
        }
        
    }
}
