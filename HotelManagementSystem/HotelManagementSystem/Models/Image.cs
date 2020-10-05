using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public string FilePath { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
    }
}
