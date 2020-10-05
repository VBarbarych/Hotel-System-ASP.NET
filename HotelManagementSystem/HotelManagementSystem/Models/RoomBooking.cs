using HotelManagementSystem.Models.AdditionalValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class RoomBooking
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        [Required(ErrorMessage = "Start Date of your booking is required")]
        [DisplayFormat(DataFormatString = "{0: dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [DateFrom(ErrorMessage = "Invalid date. Date must be greater or equal today date")]
        public DateTime BookingFrom { get; set; }

        [Required(ErrorMessage = "Final Date of your booking is required")]
        [DisplayFormat(DataFormatString = "{0: dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [DateTo(ErrorMessage = "Invalid date. Date must be greater than today date")]
        public DateTime BookingTo { get; set; }

        [Required(ErrorMessage = "Number of members is required")]
        //[Range(1, 6, ErrorMessage = "Number of members must be in range of 1-6")]
        //[Capacity(RoomBooking)]
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
    }
}
