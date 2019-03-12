using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Models.Entities
{
    public class Car
    {
        public Guid CarId { get; set; }
        [RegularExpression("^[A-Za-z]{3}-[0-9]{3}$")]
        [Display(Name = "Registreringsnummer")]
        public string RegNumber { get; set; }
        public bool Available { get; set; }
        public decimal DrivedDistance { get; set; }
        public List<Booking> Bookings { get; set; }
        [Required]
        [Display(Name = "Bilstorlek")]
        public CarModel CarModel { get; set; }
    }

    public enum CarModel
    {
        SmallCar,
        Van,
        Minibus
    }
}
