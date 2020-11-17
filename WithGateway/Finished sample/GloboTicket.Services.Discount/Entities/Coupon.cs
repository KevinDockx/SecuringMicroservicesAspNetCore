using System;

namespace GloboTicket.Services.Discount.Entities
{
    public class Coupon
    {
        public Guid CouponId { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
    }
}
