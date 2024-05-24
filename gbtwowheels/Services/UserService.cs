using System.Collections.Generic;
using gbtwowheels.Helpers;
using gbtwowheels.Models;
using gbtwowheels.Repositories;
using NuGet.Protocol.Plugins;

namespace gbtwowheels.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public async Task<ServiceResponse<User>> AddUser(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }

        public async Task<ServiceResponse<string>> Login(User request)
        {
            var user = await _userRepository.Login(request);

            if (user == null)
            {
                return new ServiceResponse<string> { Success = false, Message = "Wrong user or passwrd" };
            }

            
            var token = JwtService.GenerateToken(user.Data.UserId);

            return new ServiceResponse<string> { Success = true, Data = token };
        }

       
    }
}
