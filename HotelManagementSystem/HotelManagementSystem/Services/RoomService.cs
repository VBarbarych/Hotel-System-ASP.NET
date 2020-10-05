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
            //List<Room> reservedRoom = (List<Room>)_context.Rooms.Where(x => x.BookingStatusId == 2);
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

                    //for (var count = 1; File.Exists(ExistingFilePath) == true; count++)
                    //{
                    //    FileNameWithoutExtension = FileNameWithoutExtension + " (" + count.ToString() + ")";

                    //    var UpdatedFileName = FileNameWithoutExtension + _ext;
                    //    var UpdatedFilePath = Path.Combine(imagesFolder, UpdatedFileName);
                    //    ExistingFilePath = UpdatedFilePath;

                    //}

                    NewFileName = FileNameWithoutExtension + _ext;
                    var filePath = Path.Combine(imagesFolder, NewFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    var image = new Image
                    {
                        //Id = Guid.NewGuid(),
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

        public Room GetAllRoomsWithImage(int id)
        {
            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Include(x => x.RoomImages).Include(x => x.Bookings).Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task UpdateRoomImagesList(Room room)
        {
            var PreviouslySelectedImages = _context.Images.Where(x => x.RoomId == room.Id);
            _context.Images.RemoveRange(PreviouslySelectedImages);
            await _context.SaveChangesAsync();


        }
    }
}
