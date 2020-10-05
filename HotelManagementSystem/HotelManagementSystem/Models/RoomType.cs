using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [Display(Name = "Type of Room")]
        [Required(ErrorMessage = "Room Type is required.")]
        public string Type { get; set; }

        [Display(Name = "Base price")]
        [Required(ErrorMessage = "Base price is required.")]
        public decimal BasePrice { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}