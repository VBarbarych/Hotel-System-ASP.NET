using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly HotelContext _context;
        protected DbSet<RoomBooking> DbSet;

        public RoomBookingService(HotelContext context)
        {
            _context = context;
            DbSet = context.Set<RoomBooking>();
        }

        public async Task CreateItemAsync(RoomBooking entity)
        {
            DbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(RoomBooking entity)
        {
            DbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task EditItemAsync(RoomBooking entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomBooking>> GetAllItemsAsync()
        {
            return await DbSet.ToArrayAsync();
        }

        public async Task<RoomBooking> GetItemByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<RoomBooking>> SearchFor(Expression<Func<RoomBooking, bool>> expression)
        {
            return await DbSet.Where(expression).ToArrayAsync();
        }

        public IEnumerable<RoomBooking> GetAllRoomBookings()
        {
            return _context.RoomBookings.Include(x => x.Room);
        }

        public IEnumerable<Room> GetAllFreeRoom()
        {
            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.BookingStatus.Id == 1);
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus);
        }

        public async Task UpdateStatusDelete(int roomId)
        {
            var item = _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.Id == roomId).FirstOrDefault();

            item.BookingStatusId = 1;
            //_context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusCreate(int roomId, int statusId)
        {
            var item = _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.Id == roomId).FirstOrDefault();

            item.BookingStatusId = statusId;
            //_context.Update(item);
            await _context.SaveChangesAsync();
        }

        //public void UpdateStatusCreate()
        //{
        //    //List<Room> reservedRoom = (List<Room>)_context.Rooms.Where(x => x.BookingStatusId == 2);
        //    var reservedBookings = _context.RoomBookings.Where(x => x.RoomId == x.Room.Id).Where(x => x.Room.BookingStatusId == 2);
        //    foreach (var booking in reservedBookings)
        //    {
        //        if (booking.BookingFrom <= DateTime.Now)
        //        {
        //            var room = _context.Rooms.Where(x => x.Id == booking.RoomId);
        //            room.FirstOrDefault().BookingStatusId = 3;
        //        }
        //    }

        //}
    }
}
