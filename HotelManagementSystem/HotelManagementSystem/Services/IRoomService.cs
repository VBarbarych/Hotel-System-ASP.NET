using HotelManagementSystem.Models;
using HotelManagementSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface IRoomService
    {
        Task CreateItemAsync(Room entity);

        Task DeleteItemAsync(Room entity);

        Task EditItemAsync(Room entity);

        Task<IEnumerable<Room>> GetAllItemsAsync();

        Task<Room> GetItemByIdAsync(int? id);

        Task<IEnumerable<Room>> SearchFor(Expression<Func<Room, bool>> expression);

        RoomsViewModel GetAllRoomsAndRoomTypes();

        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();

        Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync();

        IEnumerable<Room> GetAllRooms();

        void UpdateStatusCreate();

        Room GetAllRoomsWithImage(int id);

    }
}
