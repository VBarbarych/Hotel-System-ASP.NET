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
using Microsoft.AspNetCore.Http;

namespace HotelManagementSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IImageService _imageService;

        public RoomsController(IRoomService roomService, IImageService imageService)
        {
            _roomService = roomService;
            _imageService = imageService;
        }

        // GET: Rooms
        public IActionResult Index()
        {
            _roomService.UpdateStatusCreate();
            return View(_roomService.GetAllRoomsAndRoomTypes());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room =  _roomService.GetAllRooms().SingleOrDefault(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            var images = _imageService.GetRoomImagesAsync(room);
            ViewData["Images"] = images.Result;
            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var roomTypes = _roomService.GetAllRoomTypesAsync().Result;
            var bookingStatuses = _roomService.GetAllBookingStatusesAsync().Result;
            ViewData["BookingStatusId"] = new SelectList(bookingStatuses, "Id", "Status");
            ViewData["RoomTypeId"] = new SelectList(roomTypes, "Id", "Type");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Price,RoomTypeId,BookingStatusId,Description,Capacity")] Room room, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                await _roomService.CreateItemAsync(room);
                var result = await _imageService.AddImagesAsync(files, room.Id);
                var AddedImages = new List<string>();
                foreach (var image in result.AddedImages)
                {
                    AddedImages.Add(image.Name + " Added Successfully");
                }
                ViewData["AddedImages"] = AddedImages;
                ViewData["UploadErrors"] = result.UploadErrors;
                return RedirectToAction(nameof(Index));
            }

            var roomTypes = _roomService.GetAllRoomTypesAsync().Result;
            var bookingStatuses = _roomService.GetAllBookingStatusesAsync().Result;
            ViewData["BookingStatusId"] = new SelectList(bookingStatuses, "Id", "Status");
            ViewData["RoomTypeId"] = new SelectList(roomTypes, "Id", "Type");
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetItemByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var roomTypes = _roomService.GetAllRoomTypesAsync().Result;
            var bookingStatuses = _roomService.GetAllBookingStatusesAsync().Result;
            ViewData["BookingStatusId"] = new SelectList(bookingStatuses, "Id", "Status");
            ViewData["RoomTypeId"] = new SelectList(roomTypes, "Id", "Type");
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Price,RoomTypeId,BookingStatusId,Description,Capacity")] Room room, List<IFormFile> files)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _imageService.UpdateRoomImagesList(room);
                    var result = await _imageService.AddImagesAsync(files, id);
                    var AddedImages = new List<string>();
                    foreach (var image in result.AddedImages)
                    {
                        AddedImages.Add(image.Name + " Added Successfully");
                    }
                    ViewData["AddedImages"] = AddedImages;
                    ViewData["UploadErrors"] = result.UploadErrors;
                    await _roomService.EditItemAsync(room);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_roomService.GetItemByIdAsync(id) == null)
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

            var roomTypes = _roomService.GetAllItemsAsync().Result;
            var bookingStatuses = _roomService.GetAllBookingStatusesAsync().Result;
            ViewData["BookingStatusId"] = new SelectList(bookingStatuses, "Id", "Id", room.BookingStatusId);
            ViewData["RoomTypeId"] = new SelectList(roomTypes, "Id", "Id", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetItemByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = _roomService.GetAllRoomsWithImage(id);
            await _roomService.DeleteItemAsync(room);
            return RedirectToAction(nameof(Index));
        }
    }
}
