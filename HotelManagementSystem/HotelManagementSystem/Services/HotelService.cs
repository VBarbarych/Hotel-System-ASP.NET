//using ByteSizeLib;
//using HotelManagementSystem.Data;
//using HotelManagementSystem.Models;
//using HotelManagementSystem.ViewModels;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace HotelManagementSystem.Services
//{
//    public class HotelService<TEntity> : IHotelService<TEntity> where TEntity : class
//    {
//        private readonly HotelContext _context;
//        protected DbSet<TEntity> DbSet;
//        private readonly IHostingEnvironment _hostingEnvironment;


//        public HotelService(HotelContext context, IHostingEnvironment hostingEnvironment)
//        {
//            _context = context;
//            DbSet = context.Set<TEntity>();
//            _hostingEnvironment = hostingEnvironment;
//        }

//        public async Task CreateItemAsync(TEntity entity)
//        {
//            DbSet.Add(entity);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteItemAsync(TEntity entity)
//        {
//            DbSet.Remove(entity);
//            await _context.SaveChangesAsync();
//        }

//        public async Task EditItemAsync(TEntity entity)
//        {
//            _context.Update(entity);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<IEnumerable<TEntity>> GetAllItemsAsync()
//        {
//            return await DbSet.ToArrayAsync();
//        }

//        public async Task<TEntity> GetItemByIdAsync(int? id)
//        {
//            if (id == null)
//            {
//                return null;
//            }

//            return await DbSet.FindAsync(id);
//        }

//        public async Task<IEnumerable<TEntity>> SearchFor(Expression<Func<TEntity, bool>> expression)
//        {
//            return await DbSet.Where(expression).ToArrayAsync();
//        }

//        //specific method for controllers
//        public RoomsViewModel GetAllRoomsAndRoomTypes()
//        {

//            var rooms = _context.Rooms.ToList();
//            var roomtypes = _context.RoomTypes.ToList();
//            var bookingstatuses = _context.BookingStatuses.ToList();

//            var roomsViewModel = new RoomsViewModel
//            {
//                Rooms = rooms,
//                RoomTypes = roomtypes,
//                BookingStatuses = bookingstatuses
//            };
//            return roomsViewModel;
//        }

//        //public RoomBookingsListingModel GetAllRoomBookingsAndRooms()
//        //{
//        //    var roomBooking = _context.RoomBookings.ToList();
//        //    var room = _context.Rooms.ToList();

//        //    int firstId = _context.RoomBookings.FirstOrDefault().Id;
//        //    int lastId = roomBooking.LastOrDefault().Id;
//        //    List<RoomBookingViewModel> rbvm = new List<RoomBookingViewModel>();

//        //    for (int i = firstId; i <= lastId; i++)
//        //    {
//        //        var roomBookings = _context.RoomBookings.Include(x => x.Room).Where(x => x.Id == i).FirstOrDefault();
//        //        var rooms = _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.Id == roomBookings.RoomId).FirstOrDefault();
//        //        int totalDays = roomBookings.BookingTo.Subtract(roomBookings.BookingFrom).Days;


//        //        var roomBookingViewModel = new RoomBookingViewModel
//        //        {
//        //            Id = roomBookings.Id,
//        //            RoomNumber = 100 + roomBookings.RoomId,
//        //            BookingFrom = roomBookings.BookingFrom,
//        //            BookingTo = roomBookings.BookingTo,
//        //            NoOfMembers = _context.RoomBookings.FirstOrDefault().NoOfMembers,
//        //            CustomerName = roomBookings.CustomerName,
//        //            CustomerEmail = roomBookings.CustomerEmail,
//        //            CustomerPhone = roomBookings.CustomerPhone,
//        //            TotalDays = totalDays,
//        //            TotalPay = totalDays * rooms.Price
//        //        };

//        //        rbvm.Add(roomBookingViewModel);
//        //    }
//        //    var roomBookingsListingModel = new RoomBookingsListingModel
//        //    {
//        //        RoomBookingsList = rbvm,
//        //        Rooms = room
//        //    };

//        //    return roomBookingsListingModel;




//        //    //var roomBookingList = new RoomBookingList()
//        //    //{
//        //    //    RoomBookings = roomBooking,
//        //    //    Rooms = room
//        //    //};

//        //    //return roomBookingList;
//        //}

//        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
//        {
//            return await _context.RoomTypes.ToArrayAsync();
//        }

//        public async Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync()
//        {
//            return await _context.BookingStatuses.ToArrayAsync();
//        }

//        public IEnumerable<Room> GetAllFreeRoom()
//        {
//            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.BookingStatus.Id == 1);
//        }

//        public IEnumerable<Room> GetAllRooms()
//        {
//            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus);
//        }

//        public IEnumerable<RoomBooking> GetAllRoomBookings()
//        {
//            return _context.RoomBookings.Include(x => x.Room);
//        }

//        public void UpdateStatusCreate(int roomId, int statusId)
//        {
//            var item = _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.Id == roomId).FirstOrDefault();

//            item.BookingStatusId = statusId;
//            //_context.Update(item);
//            _context.SaveChanges();
//        }

//        public void UpdateStatusDelete(int roomId)
//        {
//            var item = _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Where(x => x.Id == roomId).FirstOrDefault();

//            item.BookingStatusId = 1;
//            //_context.Update(item);
//            _context.SaveChanges();
//        }

//        public void UpdateStatusCreate()
//        {
//            //List<Room> reservedRoom = (List<Room>)_context.Rooms.Where(x => x.BookingStatusId == 2);
//            var reservedBookings = _context.RoomBookings.Where(x => x.RoomId == x.Room.Id).Where(x => x.Room.BookingStatusId == 2);
//            foreach (var booking in reservedBookings)
//            {
//                if (booking.BookingFrom <= DateTime.Now)
//                {
//                    var room = _context.Rooms.Where(x => x.Id == booking.RoomId);
//                    room.FirstOrDefault().BookingStatusId = 3;
//                }
//            }

//        }


//        public async Task<AddImagesViewModel> AddImagesAsync(List<IFormFile> files, int Id)
//        {
//            var UploadErrors = new List<string>();
//            var AddedImages = new List<Image>();
//            var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

//            foreach (var formFile in files)
//            {

//                var _ext = Path.GetExtension(formFile.FileName).ToLower(); //file Extension

//                if (formFile.Length > 0 && formFile.Length < 1000000)
//                {
//                    if (!(_ext == ".jpg" || _ext == ".png" || _ext == ".gif" || _ext == ".jpeg"))
//                    {
//                        UploadErrors.Add("The File \"" + formFile.FileName + "\" could Not be Uploaded because it has a bad extension --> \"" + _ext + "\"");
//                        continue;
//                    }

//                    string NewFileName;
//                    var ExistingFilePath = Path.Combine(imagesFolder, formFile.FileName);
//                    var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);

//                    //for (var count = 1; File.Exists(ExistingFilePath) == true; count++)
//                    //{
//                    //    FileNameWithoutExtension = FileNameWithoutExtension + " (" + count.ToString() + ")";

//                    //    var UpdatedFileName = FileNameWithoutExtension + _ext;
//                    //    var UpdatedFilePath = Path.Combine(imagesFolder, UpdatedFileName);
//                    //    ExistingFilePath = UpdatedFilePath;

//                    //}

//                    NewFileName = FileNameWithoutExtension + _ext;
//                    var filePath = Path.Combine(imagesFolder, NewFileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await formFile.CopyToAsync(stream);
//                    }
//                    var image = new Image
//                    {
//                        //Id = Guid.NewGuid(),
//                        Name = NewFileName,
//                        Size = ByteSize.FromBytes(formFile.Length).ToString(),
//                        ImageUrl = "~/images/" + NewFileName,
//                        FilePath = filePath,
//                        RoomId = Id
//                    };
//                    AddedImages.Add(image);

//                }
//                else
//                {
//                    UploadErrors.Add(formFile.FileName + " Size is not Valid. -->(" + ByteSize.FromBytes(formFile.Length).ToString() + ")... Upload a file less than 1MB");
//                }
//            }
//            _context.Images.AddRange(AddedImages);
//            _context.SaveChanges();


//            var result = new AddImagesViewModel
//            {
//                AddedImages = AddedImages,
//                UploadErrors = UploadErrors
//            };
//            return result;
//        }


//        public RoomType GetRoomTypeImages(List<IFormFile> files, RoomType roomType)
//        {
//            var UploadErrors = new List<string>();
//            var AddedImages = new List<string>();
//            RoomType roomTypeWithImage = new RoomType();
//            var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "RoomTypeImages");

//            foreach (var formFile in files)
//            {

//                var _ext = Path.GetExtension(formFile.FileName).ToLower(); //file Extension

//                if (formFile.Length > 0 && formFile.Length < 1000000)
//                {
//                    if (!(_ext == ".jpg" || _ext == ".png" || _ext == ".gif" || _ext == ".jpeg"))
//                    {
//                        UploadErrors.Add("The File \"" + formFile.FileName + "\" could Not be Uploaded because it has a bad extension --> \"" + _ext + "\"");
//                        continue;
//                    }

//                    string NewFileName;
//                    var ExistingFilePath = Path.Combine(imagesFolder, formFile.FileName);
//                    var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);

//                    //for (var count = 1; File.Exists(ExistingFilePath) == true; count++)
//                    //{
//                    //    FileNameWithoutExtension = FileNameWithoutExtension + " (" + count.ToString() + ")";

//                    //    var UpdatedFileName = FileNameWithoutExtension + _ext;
//                    //    var UpdatedFilePath = Path.Combine(imagesFolder, UpdatedFileName);
//                    //    ExistingFilePath = UpdatedFilePath;

//                    //}

//                    NewFileName = FileNameWithoutExtension + _ext;
//                    var filePath = Path.Combine(imagesFolder, NewFileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        formFile.CopyToAsync(stream);
//                    }

//                    var imageUrl = "~/RoomTypeImages/" + NewFileName;
//                    roomTypeWithImage = new RoomType
//                    {
//                        Id = roomType.Id,
//                        Type = roomType.Type,
//                        Description = roomType.Description,
//                        ImageUrl = "/RoomTypeImages/" + NewFileName,
//                        BasePrice = roomType.BasePrice
//                    };

//                    AddedImages.Add(imageUrl);

//                }
//                //else
//                //{
//                //    UploadErrors.Add(formFile.FileName + " Size is not Valid. -->(" + ByteSize.FromBytes(formFile.Length).ToString() + ")... Upload a file less than 1MB");
//                //}
//                //_context.Update(roomType);
//                //_context.SaveChanges();
//            }

//            return roomTypeWithImage;
//        }

//        public async Task<IEnumerable<Image>> GetRoomImagesAsync(Room room)
//        {
//            var RoomImagesRelationship = _context.Images.Where(x => x.RoomId == room.Id);
//            var images = new List<Image>();
//            foreach (var RoomImage in RoomImagesRelationship)
//            {
//                var Image = await _context.Images.FindAsync(RoomImage.Id);
//                images.Add(Image);
//            }


            
//            return images;
//        }

//        public Room GetAllRoomsWithImage(int id)
//        {
//            return _context.Rooms.Include(x => x.RoomType).Include(x => x.BookingStatus).Include(x => x.RoomImages).Include(x => x.Bookings).Where(x => x.Id == id).FirstOrDefault();
//        }

//        public RoomType GetRoomTypeById(int id)
//        {
//            return _context.RoomTypes.Include(x => x.Rooms).Where(x => x.Id == id).FirstOrDefault();
//        }

//        public RoomBooking GetRoomBookingById(int id)
//        {
//            return _context.RoomBookings.Include(x => x.Room).Where(x => x.Id == id).FirstOrDefault();
//        }

//        public void UpdateRoomImagesList(Room room)
//        {
//            var PreviouslySelectedImages = _context.Images.Where(x => x.RoomId == room.Id);
//            _context.Images.RemoveRange(PreviouslySelectedImages);
//            _context.SaveChanges();

            
//        }

//    }
//}
