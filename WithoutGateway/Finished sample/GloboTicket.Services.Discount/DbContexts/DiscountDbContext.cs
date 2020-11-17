using GloboTicket.Services.Discount.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GloboTicket.Services.Discount.DbContexts
{
    public class DiscountDbContext: DbContext
    {
        public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = Guid.NewGuid(), 
                Amount = 10, 
                UserId = Guid.Parse("{E455A3DF-7FA5-47E0-8435-179B300D531F}")
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = Guid.NewGuid(), 
                Amount = 20, 
                UserId = Guid.Parse("{bbf594b0-3761-4a65-b04c-eec4836d9070}")
            }); 
        }
    }
}
