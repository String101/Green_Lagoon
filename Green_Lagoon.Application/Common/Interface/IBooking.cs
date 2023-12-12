using Green_Lagoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Lagoon.Application.Common.Interface
{
    public interface IBooking:IRepository<Booking>
    {
        void Update(Booking entity);
        void UpdateStatus(int bookingId, string orderStatuse);
        void UpdateStripePaymentID(int bookingId, string sessionId,string paymentIntentId);
        void Save();
    }
}
