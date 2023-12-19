using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Infrastructure.Repositories
{
    public class AmenityRepo : Repository<Amenity>, IAmenity
    {
        public readonly ApplicationDbContext _context;
        public AmenityRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        

        public void Update(Amenity entity)
        {
            _context.Amenities.Update(entity);
        }
    }
}
