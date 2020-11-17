using GloboTicket.Services.Discount.DbContexts;
using GloboTicket.Services.Discount.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Services.Discount.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly DiscountDbContext _discountDbContext;

        public CouponRepository(DiscountDbContext discountDbContext)
        {
            _discountDbContext = discountDbContext;
        }          

        public async Task<Coupon> GetCouponById(Guid couponId)
        {
            return await _discountDbContext.Coupons.Where(x => x.CouponId == couponId).FirstOrDefaultAsync();
        }

        public async Task<Coupon> GetCouponByUserId(Guid userId)
        {
            // TODO!
            return await _discountDbContext.Coupons.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
