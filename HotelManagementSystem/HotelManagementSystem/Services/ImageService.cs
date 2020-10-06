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
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public class ImageService : IImageService
    {
        private readonly HotelContext _context;
        protected DbSet<Image> DbSet;
        private readonly IHostingEnvironment _hostingEnvironment;


        public ImageService(HotelContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            DbSet = context.Set<Image>();
            _hostingEnvironment = hostingEnvironment;
        }

        //Upload images from directory
        public async Task<AddImagesViewModel> AddImagesAsync(List<IFormFile> files, int Id)
        {
            var UploadErrors = new List<string>();
            var AddedImages = new List<Image>();
            var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

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
                        await formFile.CopyToAsync(stream);
                    }
                    var image = new Image
                    {
                        Name = NewFileName,
                        Size = ByteSize.FromBytes(formFile.Length).ToString(),
                        ImageUrl = "~/images/" + NewFileName,
                        FilePath = filePath,
                        RoomId = Id
                    };
                    AddedImages.Add(image);

                }
                else
                {
                    UploadErrors.Add(formFile.FileName + " Size is not Valid. -->(" + ByteSize.FromBytes(formFile.Length).ToString() + ")... Upload a file less than 1MB");
                }
            }
            _context.Images.AddRange(AddedImages);
            _context.SaveChanges();


            var result = new AddImagesViewModel
            {
                AddedImages = AddedImages,
                UploadErrors = UploadErrors
            };
            return result;
        }

        public async Task<IEnumerable<Image>> GetRoomImagesAsync(Room room)
        {
            var RoomImagesRelationship = _context.Images.Where(x => x.RoomId == room.Id);
            var images = new List<Image>();
            foreach (var RoomImage in RoomImagesRelationship)
            {
                var Image = await _context.Images.FindAsync(RoomImage.Id);
                images.Add(Image);
            }

            return images;
        }

        public void UpdateRoomImagesList(Room room)
        {
            var PreviouslySelectedImages = _context.Images.Where(x => x.RoomId == room.Id);
            _context.Images.RemoveRange(PreviouslySelectedImages);
            _context.SaveChanges();
        }
    }
}
