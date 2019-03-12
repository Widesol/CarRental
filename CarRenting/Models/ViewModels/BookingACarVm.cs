using CarRenting.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Models
{
    public class BookingACarVm
    {
        public Car Car { get; set; }
        public Booking Booking { get; set; }
        public Customer Customer { get; set; }
        public List<SelectListItem> CarModels { get; set; }
      
    }
}
