using System.Collections.Generic;
using Azure;
using gbtwowheels.Controllers;
using gbtwowheels.Helpers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Utils;
using gbtwowheels.Repositories;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace gbtwowheels.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
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
            var response = new ServiceResponse<User>();

            try
            {

                if (await _userRepository.IsExistingUser(user))
                {
                    response.Success = false;
                    response.Message = "Usuário já cadastrado na plataforma.";

                }
                else
                {
                    //Encrypting password
                    user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
                    var result = await _userRepository.AddUser(user);
                    response.Success = true;
                    response.Message = "Usuário cadastrado com sucesso!";
                    response.Data = result.Data;

                    if (user.ImageFile != null)
                    {
                       new Util().SaveImageInLocalStorage(response.Data!.ImageFile!, response.Data!.UserId.ToString(), "png", "wwwroot/uploads");
                    }
                }

                
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error to add user in service");

                
                response.Success = false;
                response.Message = "Ocorreu um erro ao adicionar o usuário na plataforma.";

            }

            return response;

        }

        

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }

        public async Task<ServiceResponse<User>> Login(User request)
        {
            var response = new ServiceResponse<User>();

            try
            {

                var result = await _userRepository.Login(request);

                if (result.Success)
                {
                    //Verifying password
                    if (BCrypt.Net.BCrypt.Verify(request.UserPassword, result.Data!.UserPassword))
                    {
                        var token = JwtService.GenerateToken(result.Data!.UserId);
                        result.Data!.TokenAccess = token;


                        response.Success = true;
                        response.Message = "Login realizado com sucesso!";
                        response.Data = result.Data;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Usuário e/ou senha incorretos";
                    }

                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuário e/ou senha incorretos";
                }


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error while login in service");


                response.Success = false;
                response.Message = "Ocorreu um erro realizar login na plataforma.";

            }

            return response;
           
        }

       
    }
}
