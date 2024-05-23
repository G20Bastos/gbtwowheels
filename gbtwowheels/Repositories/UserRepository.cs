using System.Collections.Generic;
using System.Linq;
using gbtwowheels.Data;
using gbtwowheels.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using BCrypt.Net;

namespace gbtwowheels.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public async Task<ServiceResponse<User>> Login(User request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == request.UserEmail);

            if (user == null)
            {
                return new ServiceResponse<User> { Success = false, Message = "Wrong User or Password" };
            }

            
            if (!BCrypt.Net.BCrypt.Verify(request.UserPassword, user.UserPassword))
            {
                return new ServiceResponse<User> { Success = false, Message = "Wrong User or Password" };
            }

            return new ServiceResponse<User> { Success = true, Data = user };
        }

        public async Task<ServiceResponse<User>> AddUser(User user)
        {
            
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
            if (existingUser != null)
            {
                return new ServiceResponse<User> { Success = false, Message = "Email already exists." };
            }

           
            user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<User> { Success = true, Data = user };
        }
    }
}
