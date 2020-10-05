using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface IRoomTypeService
    {
        Task CreateItemAsync(RoomType entity);

        Task DeleteItemAsync(RoomType entity);

        Task EditItemAsync(RoomType entity);

        Task<IEnumerable<RoomType>> GetAllItemsAsync();

        Task<RoomType> GetItemByIdAsync(int? id);

        Task<IEnumerable<RoomType>> SearchFor(Expression<Func<RoomType, bool>> expression);

        RoomType GetRoomTypeImages(List<IFormFile> files, RoomType roomType);

        RoomType GetRoomTypeById(int id);
    }
}
