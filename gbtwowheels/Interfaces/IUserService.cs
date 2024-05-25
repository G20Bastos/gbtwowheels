using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        Task<ServiceResponse<User>> AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        Task<ServiceResponse<User>> Login(User request);
    }
}

