using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        Task<ServiceResponse<User>> Login(User request);
        Task<ServiceResponse<User>> AddUser(User user);
        Task<bool> IsExistingUser(User user);
        Task<bool> IsExistingCNPJ(User user);
    }
}

