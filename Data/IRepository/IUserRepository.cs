using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAPI.Models;

namespace ParkingAPI.Data.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
    }
}
