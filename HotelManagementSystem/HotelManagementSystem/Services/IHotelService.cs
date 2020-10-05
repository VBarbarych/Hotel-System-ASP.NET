//using HotelManagementSystem.Models;
//using HotelManagementSystem.ViewModels;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace HotelManagementSystem.Services
//{
//    public interface IHotelService<TEntity>
//    {
//        Task<IEnumerable<TEntity>> GetAllItemsAsync();

//        Task<TEntity> GetItemByIdAsync(int? id);

//        Task<IEnumerable<TEntity>> SearchFor(Expression<Func<TEntity, bool>> expression);

//        Task CreateItemAsync(TEntity entity);

//        Task EditItemAsync(TEntity entity);

//        Task DeleteItemAsync(TEntity entity);

//        RoomsViewModel GetAllRoomsAndRoomTypes();

//        //RoomBookingsListingModel GetAllRoomBookingsAndRooms();

//        //RoomBookingViewModel BookingItem();

//        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();

//        IEnumerable<Room> GetAllFreeRoom();

//        Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync();

//        IEnumerable<Room> GetAllRooms();
//        IEnumerable<RoomBooking> GetAllRoomBookings();

//        void UpdateStatusCreate(int roomId, int statusId);
//        void UpdateStatusDelete(int roomId);
//        void UpdateStatusCreate();

//        Task<AddImagesViewModel> AddImagesAsync(List<IFormFile> files, int Id);

//        RoomType GetRoomTypeImages(List<IFormFile> file, RoomType roomType);

//        Task<IEnumerable<Image>> GetRoomImagesAsync(Room room);

//        Room GetAllRoomsWithImage(int id);

//        RoomType GetRoomTypeById(int id);

//        RoomBooking GetRoomBookingById(int id);

//        void UpdateRoomImagesList(Room room);
//    }
//}
