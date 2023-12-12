using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IVilla Villa { get; private set; }
        public IVillaNumber VillaNumber { get; private set; }
        public IAmenity Amenity { get; private set; }
        public IBooking Booking { get; private set; }
        public IApplicationUser User { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        { 
            _db = db;
            Villa = new VillaRepo(_db);
            VillaNumber = new VillaNumberRepo(_db);
            Amenity = new AmenityRepo(_db);
            Booking = new BookingRepo(_db);
            User = new ApplicationUserRepo(_db);
        }
       
    }
}
