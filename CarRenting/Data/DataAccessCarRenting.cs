using CarRenting.Models;
using CarRenting.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRenting.Data
{
    public class DataAccessCarRenting
    {
        public ApplicationDbContext _bookingContext;
        

        public DataAccessCarRenting(ApplicationDbContext bookingContext)
        {
            _bookingContext = bookingContext;
        }
        
        internal async Task SaveBooking(BookingACarVm bookACarVm)
        {
            var booking = new Booking();
            var tryCar = _bookingContext.Cars.Where(x => x.RegNumber == bookACarVm.Car.RegNumber).ToList();

            if (tryCar.Count ==0)
            {
                Car car = new Car();
                car.CarModel = bookACarVm.Car.CarModel;
                car.RegNumber = bookACarVm.Car.RegNumber;
                car.Available = false;
            booking.BookingTime = bookACarVm.Booking.BookingTime;
            booking.Car = car;
            }
            else
            {
              
                booking.BookingTime= bookACarVm.Booking.BookingTime;
                booking.Car = tryCar.First();
            }
            var tryCustomer = _bookingContext.Customers.Where(x => x.PersonNumber == bookACarVm.Customer.PersonNumber).ToList();
            if (tryCustomer.Count==0)
            {
                Customer customer = new Customer();
                customer.PersonNumber = bookACarVm.Customer.PersonNumber;
                booking.Customer = customer;
            }
            else
            {
                booking.Customer = tryCustomer.First();
            }

            _bookingContext.Add(booking);

            await _bookingContext.SaveChangesAsync();
            
        }

        internal async Task<List<Booking>> GetAllBookings()
        {
            return await _bookingContext.Bookings.Include(x=>x.Car).Include(x => x.Customer).ToListAsync();
        }

        internal async Task<List<Booking>> GetAllBookingsSort(int sortChoice)
        {
            switch (sortChoice)
            {
                case 1:
             return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).OrderBy(x=>x.BookingTime).ToListAsync();
                case 2:
                    return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).OrderBy(x => x.Car.CarModel).ToListAsync();
                default:
                    return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).OrderBy(x => x.Customer.PersonNumber).ToListAsync();
            }
        }

        internal async Task<List<Booking>> GetAllBookingsSortDependingOnCarStatus(int sortChoice, int status)
        {
            if (status==2)
            {
                switch (sortChoice)
                {
                    case 1:
                        return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x=>x.Car.Available==true).OrderBy(x => x.BookingTime).ToListAsync();
                    case 2:
                        return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == true).OrderBy(x => x.Car.CarModel).ToListAsync();
                    default:
                        return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == true).OrderBy(x => x.Customer.PersonNumber).ToListAsync();
                }
            }
         
            else
            {
                switch (sortChoice)
                {
                    case 1:
                        return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == false).OrderBy(x => x.BookingTime).ToListAsync();
                    case 2:
                        return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == false).OrderBy(x => x.Car.CarModel).ToListAsync();
                    default:
                        return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == false).OrderBy(x => x.Customer.PersonNumber).ToListAsync();
                }
            }
        }

        internal async Task EndBooking(Booking booking)
        {
            
            var car = await _bookingContext.Cars.FindAsync(booking.CarId);
            var customer= await _bookingContext.Customers.FindAsync(booking.CustomerId);
            car.DrivedDistance += booking.Distance;
            car.Available = true;
            _bookingContext.Update(booking);
            _bookingContext.Update(car);
            await _bookingContext.SaveChangesAsync();
        }

        internal async Task<List<Booking>> GetAllBookingsDependingOnStatus(int status)
        {
            if (status==2)
            {
                return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == true).ToListAsync();
            }
            else
            {
                return await _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.Car.Available == false).ToListAsync();
            }
        }

        internal Booking GetBooking(Guid? id)
        {
            var booking = _bookingContext.Bookings.Include(x => x.Car).Include(x => x.Customer).Where(x => x.BookingId == id).FirstOrDefault();
            return booking;
                //_bookingContext.Bookings.FindAsync(id);
        }
    }
}
