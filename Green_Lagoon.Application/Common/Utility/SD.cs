using Green_Lagoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int VillaRoomAvailable_Count(int villaId,List<VillaNumber> villaNumberList,DateOnly checkInDate, int nights,List<Booking> bookings)
        {
            List<int> bookingInDate = new();
            int finalAvailableroomForAllNights = int.MaxValue;
            var roomInVilla = villaNumberList.Where(x=>x.VillaId == villaId).Count();


            for(int i=0;i<nights;i++)
            {
                var villaBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i) && u.CheckInOut > checkInDate.AddDays(i) && u.VillaId == villaId);

                foreach (var booking in villaBooked)
                {
                    if(!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }
                var totalAvailableRooms = roomInVilla - bookingInDate.Count;

                if(totalAvailableRooms==0)
                {
                    return 0;
                }
                else
                {
                    if (finalAvailableroomForAllNights > totalAvailableRooms)
                    {
                        finalAvailableroomForAllNights=totalAvailableRooms;
                    }
                }
              
            }
            return finalAvailableroomForAllNights;
        }


    }
}
