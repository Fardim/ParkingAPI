using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Dtos
{
    public class PhotoForDetailDto
    {
        public int Id { get; set; }
        public String Url { get; set; }
        public String Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}
