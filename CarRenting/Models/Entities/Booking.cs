using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Models.Entities
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid CarId { get; set; }
        public Car Car { get; set; }
        [Display (Name = "Starttid")]
        [Required]
        public DateTime BookingTime { get; set; }
        [Display (Name ="Sluttid")]
        public DateTime ReturnTime { get; set; }
        [Display(Name = "Antal dagar")]
        public decimal NumberOfRentDays
        {
            get
            {
                decimal numberOfRentDays = (ReturnTime - BookingTime).Days+1;
                return numberOfRentDays;
            }
        }
        [Range(0,int.MaxValue, ErrorMessage = "Du kan inte ligga på minus i kilometer, inte ens om du bara backat")]
        [Display (Name ="Körd distans")]

        public decimal Distance { get; set; }

        private decimal price=0;
        int baseDayRental = 12;
        int kmPrice = 4;
        [Display(Name = "Pris")]
        public decimal Price
        {
            get
            {
                if (Car==null)
                {
                    price = 0;
                    return price;
                }
                else
                {
                    string[] arr = Enum.GetNames(typeof(CarModel));


                    if (arr.Single(x => x == "SmallCar") == Car.CarModel.ToString())
                    {
                        price = baseDayRental * NumberOfRentDays;
                        return price;
                    }

                    else if (arr.Single(x => x == "Van") == Car.CarModel.ToString())
                    {
                        price = baseDayRental * NumberOfRentDays * 1.2m + Distance * kmPrice * 1.2m;
                        return price;

                    }
                    else if (arr.Single(x => x == "MiniBus") == Car.CarModel.ToString())
                    {
                        price = baseDayRental * NumberOfRentDays * 1.7m + Distance * kmPrice * 1.5m;
                        return price;
                    }
                    else if (Car == null)
                    {
                        price = 0;
                        return price;
                    }
                    return price;
                }

            }

        }

    }
}
