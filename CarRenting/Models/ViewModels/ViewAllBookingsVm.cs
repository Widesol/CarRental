using CarRenting.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Models.ViewModels
{
    public class ViewAllBookingsVm
    {
        public List<Booking> Bookings { get; set; }
        public Booking Booking { get; set; }
    }
}
