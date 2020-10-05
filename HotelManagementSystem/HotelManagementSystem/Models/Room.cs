using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Room №")]
        [Required(ErrorMessage = "Room Number is required.")]
        public int Number { get; set; }

        [Display(Name = "Room Price")]
        [Required(ErrorMessage = "Room Price is required.")]
        [Range(300, 5000, ErrorMessage = "Price need to be betweeen 300 and 5000.")]
        public decimal Price { get; set; }

        [Display(Name = "Room Type")]
        [Required(ErrorMessage = "Select your Room Type.")]
        public int RoomTypeId { get; set; }

        public virtual RoomType RoomType { get; set; }

        [Display(Name = "Booking Status")]
        public int BookingStatusId { get; set; }

        public virtual BookingStatus BookingStatus { get; set; }

        [Display(Name = "Room Description")]
        [Required(ErrorMessage = "Describe room.")]
        public string Description { get; set; }

        [Display(Name = "Room Capacity")]
        [Required(ErrorMessage = "Room Capacity is required.")]
        [Range(1, 7, ErrorMessage = "Room Capacity need to be between 1 and 7.")]
        public int Capacity { get; set; }

        public virtual List<Image> RoomImages { get; set; }
        public virtual List<RoomBooking> Bookings { get; set; }
    }
}