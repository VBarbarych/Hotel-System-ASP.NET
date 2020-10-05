using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using HotelManagementSystem.ViewModels;

namespace HotelManagementSystem.Controllers
{
    public class RoomBookingsController : Controller
    {
        private readonly IRoomBookingService _roomBookingService;
        private readonly IRoomService _roomService;

        public RoomBookingsController(IRoomBookingService roomBookingService, IRoomService roomService)
        {
            _roomBookingService = roomBookingService;
            _roomService = roomService;
        }

        // GET: RoomBookings
        public async Task<IActionResult> Index()
        {
            var room = _roomService.GetAllRooms();
            var roomBooking = _roomBookingService.GetAllRoomBookings();

            var roomBookingViewModel = roomBooking
                .Select(rb => new RoomBookingViewModel
                {
                    Id = rb.Id,
                    RoomNumber = room.Where(x => x.Id == rb.RoomId).FirstOrDefault().Number,
                    BookingFrom = rb.BookingFrom,
                    BookingTo = rb.BookingTo,
                    NoOfMembers = rb.NoOfMembers,
                    CustomerName = rb.CustomerName,
                    CustomerEmail = rb.CustomerEmail,
                    CustomerPhone = rb.CustomerPhone,
                    TotalDays = roomBooking.Where(x => x.Id == rb.Id).FirstOrDefault().BookingTo.Subtract(roomBooking.Where(x => x.Id == rb.Id).FirstOrDefault().BookingFrom).Days,
                    TotalPay = roomBooking.Where(x => x.Id == rb.Id).FirstOrDefault().BookingTo.Subtract(roomBooking.Where(x => x.Id == rb.Id).FirstOrDefault().BookingFrom).Days * room.Where(x => x.Id == rb.RoomId).FirstOrDefault().Price

                }).ToList();

            var roomBookingsListingModel = new RoomBookingsListingModel
            {
                RoomBookingsList = roomBookingViewModel,
                Rooms = room.ToList()
            };

            return View(roomBookingsListingModel);
        }

        // GET: RoomBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomBooking = await _roomBookingService.GetItemByIdAsync(id);

            if (roomBooking == null)
            {
                return NotFound();
            }
            return View(roomBooking);
        }

        // GET: RoomBookings/Create
        public IActionResult Create()
        {
            var rooms = _roomBookingService.GetAllFreeRoom();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Number");
            return View();
        }

        // POST: RoomBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,BookingFrom,BookingTo,NoOfMembers,CustomerName,CustomerPhone,CustomerEmail")] RoomBooking roomBooking)
        {
            var room = _roomService.GetAllRooms();
            var roomCapacity = room.Where(x => x.Id == roomBooking.RoomId).FirstOrDefault().Capacity;

            if (roomBooking.NoOfMembers > roomCapacity)
            {
                ModelState.AddModelError("NoOfMembers", $"Number of members must be in range of 1-{roomCapacity}");
            }

            if (ModelState.IsValid)
            {
                
                await _roomBookingService.CreateItemAsync(roomBooking);
                if (roomBooking.BookingFrom > DateTime.Now.AddMinutes(1))
                {
                    await _roomBookingService.UpdateStatusCreate(roomBooking.RoomId, 2);
                }
                else
                {
                    await _roomBookingService.UpdateStatusCreate(roomBooking.RoomId, 3);
                }
                return RedirectToAction(nameof(Index));
            }

            var rooms = _roomBookingService.GetAllFreeRoom();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Number");
            return View(roomBooking);
        }

        // GET: RoomBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomBooking = await _roomBookingService.GetItemByIdAsync(id);
            if (roomBooking == null)
            {
                return NotFound();
            }

            var rooms = _roomBookingService.GetAllRooms();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Number");
            return View(roomBooking);
        }

        // POST: RoomBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,BookingFrom,BookingTo,NoOfMembers,CustomerName,CustomerPhone,CustomerEmail")] RoomBooking roomBooking)
        {
            if (id != roomBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roomBookingService.EditItemAsync(roomBooking);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_roomBookingService.GetItemByIdAsync(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var rooms = _roomBookingService.GetAllRooms();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Number");
            return View(roomBooking);
        }

        // GET: RoomBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomBooking = await _roomBookingService.GetItemByIdAsync(id);
            if (roomBooking == null)
            {
                return NotFound();
            }

            return View(roomBooking);
        }

        // POST: RoomBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomBooking = await _roomBookingService.GetItemByIdAsync(id);
            await _roomBookingService.UpdateStatusDelete(roomBooking.RoomId);
            await _roomBookingService.DeleteItemAsync(roomBooking);
            return RedirectToAction(nameof(Index));
        }
    }
}