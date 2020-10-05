using HotelManagementSystem.Models;
using HotelManagementSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface IImageService
    {
        Task<AddImagesViewModel> AddImagesAsync(List<IFormFile> files, int Id);

        Task<IEnumerable<Image>> GetRoomImagesAsync(Room room);

        public void UpdateRoomImagesList(Room room);
    }
}
