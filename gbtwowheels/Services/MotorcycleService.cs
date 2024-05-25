using System;
using gbtwowheels.Controllers;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Repositories;
using gbtwowheels.Utils;

namespace gbtwowheels.Services
{
    public class MotorcycleService : IMotorcycleService
    {

        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger<MotorcycleService> _logger;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository, ILogger<MotorcycleService> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Motorcycle>> Add(Motorcycle motorcycle)
        {
            var response = new ServiceResponse<Motorcycle>();

            try
            {

                if (await _motorcycleRepository.IsExistingMotorcycle(motorcycle))
                {
                    response.Success = false;
                    response.Message = "Moto já cadastrada na plataforma.";

                }
                else
                {
                    
                    var result = await _motorcycleRepository.Add(motorcycle);
                    response.Success = true;
                    response.Message = "Moto cadastrada com sucesso!";
                    response.Data = result.Data;

                    
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

        public void DeleteMotorcycle(int id)
        {
            _motorcycleRepository.Delete(id);
        }

        public IEnumerable<Motorcycle> GetAllMotorcycles()
        {
            return _motorcycleRepository.GetAll();
        }

        public Motorcycle GetMotorcycleById(int id)
        {
            return _motorcycleRepository.GetById(id);
        }

        public void UpdateMotorcycle(Motorcycle motorcycle)
        {
            _motorcycleRepository.Update(motorcycle);
        }
    }
}

