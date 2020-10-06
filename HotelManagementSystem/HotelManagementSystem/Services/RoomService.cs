using ByteSizeLib;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelContext _context;
        protected DbSet<Room> DbSet;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RoomService(HotelContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            DbSet = context.Set<Room>();
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task CreateItemAsync(Room entity)
        {
            DbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(Room entity)
        {
            DbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task EditItemAsync(Room entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetAllItemsAsync()
        {
            return await DbSet.ToArrayAsync();
        }

        public async Task<Room> GetItemByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> SearchFor(Expression<Func<Room, bool>> expression)
        {
            return await DbSet.Where(expression).ToArrayAsync();
        }

        public RoomsViewModel GetAllRoomsAndRoomTypes()
        {

            var rooms = _context.Rooms.ToList();
            var roomtypes = _context.RoomTypes.ToList();
            var bookingstatuses = _context.BookingStatuses.ToList();

            var roomsViewModel = new RoomsViewModel
            {
                Rooms = rooms,
                RoomTypes = roomtypes,
                BookingStatuses = bookingstatuses
            };
            return roomsViewModel;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _context.RoomTypes.ToArrayAsync();
        }

        public async Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync()
        {
            return await _context.BookingStatuses.ToArrayAsync();
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus);
        }

        public void UpdateStatusCreate()
        {
            var reservedBookings = _context.RoomBookings.Where(x => x.RoomId == x.Room.Id).Where(x => x.Room.BookingStatusId == 2);
            foreach (var booking in reservedBookings)
            {
                if (booking.BookingFrom <= DateTime.Now)
                {
                    var room = _context.Rooms.Where(x => x.Id == booking.RoomId);
                    room.FirstOrDefault().BookingStatusId = 3;
                }
            }

        }

        public Room GetAllRoomsWithImage(int id)
        {
            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Include(x => x.RoomImages).Include(x => x.Bookings).Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
