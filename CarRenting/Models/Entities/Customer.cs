using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Models.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        [Required]
        [RegularExpression("^(19|20)?[0-9]{6}[-]?[0-9]{4}$")]
        public string PersonNumber { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
