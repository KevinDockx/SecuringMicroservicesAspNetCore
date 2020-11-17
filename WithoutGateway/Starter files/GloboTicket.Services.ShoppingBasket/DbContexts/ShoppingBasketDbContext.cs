using GloboTicket.Services.ShoppingBasket.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GloboTicket.Services.ShoppingBasket.DbContexts
{
    public class ShoppingBasketDbContext : DbContext
    {
        public ShoppingBasketDbContext(DbContextOptions<ShoppingBasketDbContext> options)
        : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketLine> BasketLines { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BasketChangeEvent> BasketChangeEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // seed the database with dummy data
            modelBuilder.Entity<Event>().HasData(
                 new Event
                 {
                     EventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                     Name = "John Egbert Live",
                     Date = DateTime.Now.AddMonths(6)
                 },
                 new Event
                 {
                     EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                     Name = "The State of Affairs: Michael Live!",
                     Date = DateTime.Now.AddMonths(9),

                 }, 
                 new Event
                 {
                     EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                     Name = "Clash of the DJs",
                     Date = DateTime.Now.AddMonths(4),
                 }, 
                 new Event
                 {
                     EventId = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
                     Name = "Spanish guitar hits with Manuel",
                     Date = DateTime.Now.AddMonths(4)
                 }, 
                 new Event
                 {
                     EventId = Guid.Parse("{1BABD057-E980-4CB3-9CD2-7FDD9E525668}"),
                     Name = "Techorama 2021",
                     Date = DateTime.Now.AddMonths(10)
                 }, 
                 new Event
                 {
                     EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                     Name = "To the Moon and Back",
                     Date = DateTime.Now.AddMonths(8)
                 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
