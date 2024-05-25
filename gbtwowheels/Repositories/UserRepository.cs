using System.Collections.Generic;
using System.Linq;
using gbtwowheels.Data;
using gbtwowheels.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using BCrypt.Net;
using gbtwowheels.Interfaces;
using Azure;
using gbtwowheels.Controllers;

namespace gbtwowheels.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
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
            var response = new ServiceResponse<User>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == request.UserEmail);

                if (user != null)
                {
                    response.Data = user;
                    response.Success = true;
                }
                else
                {
                    response.Success = false;


                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while login");
            }

            return response;
            
        }


        public async Task<ServiceResponse<User>> AddUser(User user)
        {

            var response = new ServiceResponse<User>();

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = user;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while add user in database");

            }

            return response;

        }

        public async Task<bool> IsExistingUser(User user)
        {
            
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
                if (existingUser != null)
                {
                    return true;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while analizying if the user already exists in database");

            }

            return false;
        }
    }
}
