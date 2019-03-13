using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRenting.Data;
using CarRenting.Models;
using CarRenting.Models.Entities;
using CarRenting.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRenting.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DataAccessCarRenting _dataAccessCarRenting;

        public BookingController(ApplicationDbContext context, DataAccessCarRenting dataAccessCarRenting)
        {
            _context = context;
            _dataAccessCarRenting = dataAccessCarRenting;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var bookACarVm = new BookingACarVm();

            string[] arr = Enum.GetNames(typeof(CarModel));
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in arr)
            {
                var enm = new SelectListItem() { Text = item, Value = item };
                list.Add(enm);
            }
            bookACarVm.CarModels = list;
            return View(bookACarVm);
        }

        public async Task <IActionResult> BookCar(BookingACarVm bookACarVm)
        {
            if (ModelState.IsValid)
            {
                //if (bookACarVm.Car.CarModel!=_dataAccessCarRenting.GetCar(bookACarVm.Car.RegNumber))
                //{
                //    ModelState.AddModelError("CarTypeError", "Biltypen på denna bilen är inte samma som kunden önskade");
                //    return View();

                //}
                await _dataAccessCarRenting.SaveBooking(bookACarVm);
            return View(bookACarVm);
            }
            return View();
        }

        public async Task<IActionResult> AllBookings ()
        {
            var allBookingsVm = new ViewAllBookingsVm();
            allBookingsVm.Bookings=await _dataAccessCarRenting.GetAllBookings();
           
            return View(allBookingsVm);
        }
        public async Task<IActionResult> AllBookingsSort(int sortChoice)
        {

            var allBookingsVm = new ViewAllBookingsVm();
            allBookingsVm.Bookings = await _dataAccessCarRenting.GetAllBookingsSort(sortChoice);

            return View("AllBookings", allBookingsVm);
        }

        public async Task<IActionResult> BookingsDependingOnStatus(int status)
        {
            var allBookingsVm = new ViewAllBookingsVm();
            allBookingsVm.BookingStatus = status;
            allBookingsVm.Bookings = await _dataAccessCarRenting.GetAllBookingsDependingOnStatus(status);

            return View("AllBookings", allBookingsVm);
        }

        public async Task<IActionResult> AllBookingsSortForUnavailible(int sortChoice)
        {
            
            int status = 1;
            var allBookingsVm = new ViewAllBookingsVm();
            allBookingsVm.BookingStatus = 1;
            allBookingsVm.Bookings = await _dataAccessCarRenting.GetAllBookingsSortDependingOnCarStatus(sortChoice, status);

            return View("AllBookings", allBookingsVm);
        }

        public async Task<IActionResult> AllBookingsSortForAvailible(int sortChoice)
        {
            int status = 2;
            var allBookingsVm = new ViewAllBookingsVm();
            allBookingsVm.BookingStatus = 2;
            allBookingsVm.Bookings = await _dataAccessCarRenting.GetAllBookingsSortDependingOnCarStatus(sortChoice, status);

            return View("AllBookings", allBookingsVm);
        }

        public async Task<IActionResult> EndBooking(Guid? id)
        {
            var booking = _dataAccessCarRenting.GetBooking(id);
       
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndBookingStep2(Guid id, [Bind("BookingId,CustomerId, Customer, CarId, Car, BookingTime, Price,ReturnTime,NumberOfRentDays,Distance")] Booking booking)
        {
            if (booking.BookingTime > booking.ReturnTime)
            {
                ModelState.AddModelError("DateError", "Returdatumet tidigare än bokningsdatumet går inte"); //Funkar inte som jag tänkt
                return View("EndBooking", booking);
            }
            await _dataAccessCarRenting.EndBooking(booking);

            return View(booking);
        }
    }
}
