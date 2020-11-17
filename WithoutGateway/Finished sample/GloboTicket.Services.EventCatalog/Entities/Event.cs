using System;
using System.ComponentModel.DataAnnotations;

namespace GloboTicket.Services.EventCatalog.Entities
{
    public class Event
    {
        [Required]
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Artist { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
