using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
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
    public class RoomTypeService : IRoomTypeService
    {
        private readonly HotelContext _context;
        protected DbSet<RoomType> DbSet;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RoomTypeService(HotelContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            DbSet = context.Set<RoomType>();
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task CreateItemAsync(RoomType entity)
        {
            DbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(RoomType entity)
        {
            DbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task EditItemAsync(RoomType entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomType>> GetAllItemsAsync()
        {
            return await DbSet.ToArrayAsync();
        }

        public async Task<RoomType> GetItemByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<RoomType>> SearchFor(Expression<Func<RoomType, bool>> expression)
        {
            return await DbSet.Where(expression).ToArrayAsync();
        }

        //Upload images from directory
        public RoomType GetRoomTypeImages(List<IFormFile> files, RoomType roomType)
        {
            var UploadErrors = new List<string>();
            var AddedImages = new List<string>();
            RoomType roomTypeWithImage = new RoomType();
            var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "RoomTypeImages");

            foreach (var formFile in files)
            {

                var _ext = Path.GetExtension(formFile.FileName).ToLower(); //file Extension

                if (formFile.Length > 0 && formFile.Length < 1000000)
                {
                    if (!(_ext == ".jpg" || _ext == ".png" || _ext == ".gif" || _ext == ".jpeg"))
                    {
                        UploadErrors.Add("The File \"" + formFile.FileName + "\" could Not be Uploaded because it has a bad extension --> \"" + _ext + "\"");
                        continue;
                    }

                    string NewFileName;
                    var ExistingFilePath = Path.Combine(imagesFolder, formFile.FileName);
                    var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);

                    NewFileName = FileNameWithoutExtension + _ext;
                    var filePath = Path.Combine(imagesFolder, NewFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyToAsync(stream);
                    }

                    var imageUrl = "~/RoomTypeImages/" + NewFileName;
                    roomTypeWithImage = new RoomType
                    {
                        Id = roomType.Id,
                        Type = roomType.Type,
                        Description = roomType.Description,
                        ImageUrl = "/RoomTypeImages/" + NewFileName,
                        BasePrice = roomType.BasePrice
                    };

                    AddedImages.Add(imageUrl);

                }
            }

            return roomTypeWithImage;
        }

        public RoomType GetRoomTypeById(int id)
        {
            return _context.RoomTypes.Include(x => x.Rooms).Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
