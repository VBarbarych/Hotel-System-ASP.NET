using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface IRoomBookingService
    {
        Task CreateItemAsync(RoomBooking entity);



        Task DeleteItemAsync(RoomBooking entity);


        Task EditItemAsync(RoomBooking entity);

        Task<IEnumerable<RoomBooking>> GetAllItemsAsync();

        Task<RoomBooking> GetItemByIdAsync(int? id);


        Task<IEnumerable<RoomBooking>> SearchFor(Expression<Func<RoomBooking, bool>> expression);


        IEnumerable<RoomBooking> GetAllRoomBookings();


        IEnumerable<Room> GetAllFreeRoom();


        IEnumerable<Room> GetAllRooms();

        Task UpdateStatusDelete(int roomId);

        Task UpdateStatusCreate(int roomId, int statusId);
    }
}
