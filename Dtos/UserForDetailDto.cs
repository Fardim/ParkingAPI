﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Dtos
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForDetailDto> Photos { get; set; }
    }
}
