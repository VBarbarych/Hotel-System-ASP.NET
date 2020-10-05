using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.ViewModels
{
    public class RoomBookingViewModel
    {
        //public List<RoomBooking> RoomBookings { get; set; }
        public int Id { get; set; }

        public int RoomId { get; set; }

        //public virtual Room Room { get; set; }
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Start Date of your booking is required")]
        [DisplayFormat(DataFormatString = "{0: dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingFrom { get; set; }

        [Required(ErrorMessage = "Final Date of your booking is required")]
        [DisplayFormat(DataFormatString = "{0: dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingTo { get; set; }

        [Required(ErrorMessage = "Number of members is required")]
        public int NoOfMembers { get; set; }

        [Required(ErrorMessage = "Enter customer name")]
        public string CustomerName { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Please provide a valid phone number")]
        [Phone(ErrorMessage = "Not a valid phone number")]
        [Display(Name = "Customer Phone number")]
        public string CustomerPhone { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required(ErrorMessage = "Enter customer email")]
        public string CustomerEmail { get; set; }

        public int TotalDays { get; set; }

        public decimal TotalPay { get; set; }

        //public IEnumerable<RoomBookingViewModel> ListOfRoomBokings { get; set; }

        //public List<Room> Rooms { get; set; }
    }
}
