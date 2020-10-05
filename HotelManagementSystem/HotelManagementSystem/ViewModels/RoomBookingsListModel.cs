using HotelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.ViewModels
{
    public class RoomBookingsListingModel
    {
        public List<RoomBookingViewModel> RoomBookingsList { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
