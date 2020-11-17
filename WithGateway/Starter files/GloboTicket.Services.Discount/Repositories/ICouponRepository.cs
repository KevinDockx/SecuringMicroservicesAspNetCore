using System;
using GloboTicket.Services.Discount.Entities;
using System.Threading.Tasks;

namespace GloboTicket.Services.Discount.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon> GetCouponByUserId(Guid userId);         
        Task<Coupon> GetCouponById(Guid couponId);
    }
}
