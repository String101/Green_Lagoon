using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Application.Common.Utility;
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

       

        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
        }

        public void UpdateStatus(int bookingId, string bookingStatus,int villaNumber=0)
        {
           var bookingFromDb= _context.Bookings.FirstOrDefault(u=>u.Id==bookingId);
            if(bookingFromDb!=null)
            {
                bookingFromDb.Status = bookingStatus;
                if(bookingStatus==SD.StatusCheckedIn)
                {
                    bookingFromDb.VillaNumber = villaNumber;
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                    
                }
                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckOutDate = DateTime.Now;

                }
            }
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(u => u.Id == bookingId);
            if(bookingFromDb!=null)
            {
                if(!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDb.StripeSessionId = sessionId;

                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromDb.PaymentDate= DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful = true;

                }

            }
        }
    }
}
