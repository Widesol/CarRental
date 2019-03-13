using CarRenting.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Models
{
    public class BookingValidator: AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(x => x.ReturnTime).GreaterThanOrEqualTo(x => x.BookingTime).WithMessage("Du kan inte ha slutdatum tidigare än startdatum");
        }
    }
}
