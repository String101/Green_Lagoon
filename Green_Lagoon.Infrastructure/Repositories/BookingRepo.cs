﻿using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Infrastructure.Repositories
{
    public class BookingRepo : Repository<Booking>, IBooking
    {
        public readonly ApplicationDbContext _context;
        public BookingRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Save()
        {
           _context.SaveChanges();
        }

        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
        }
    }
}