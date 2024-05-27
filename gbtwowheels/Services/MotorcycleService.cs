using System;
using gbtwowheels.Controllers;
using gbtwowheels.Filters;
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
                    response.Message = "Moto já cadastrada na plataforma - Placa já existente.";

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

                _logger.LogError(ex, "Error to add moto in service");


                response.Success = false;
                response.Message = "Ocorreu um erro ao adicionar a moto na plataforma.";

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

        public IEnumerable<Motorcycle> GetByFilterAsync(MotorcycleFilters filter)
        {
           
            try
            {
                    return _motorcycleRepository.GetByFilter(filter);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error to find moto by filters in service");
                return Enumerable.Empty<Motorcycle>();

            }

        }

        public async Task<Motorcycle> GetMotorcycleAvailable()
        {
            return await _motorcycleRepository.GetMotorcycleAvailable();
        }

        public Motorcycle GetMotorcycleById(int id)
        {
            return _motorcycleRepository.GetById(id);
        }

        public async Task<ServiceResponse<Motorcycle>> UpdateMotorcycle(Motorcycle motorcycle)
        {

            var response = new ServiceResponse<Motorcycle>();

            try
            {


                if (await _motorcycleRepository.IsExistingMotorcycle(motorcycle))
                {
                    response.Success = false;
                    response.Message = "Moto já cadastrada na plataforma - Placa já existente.";

                }
                else
                {

                    var result = await _motorcycleRepository.Update(motorcycle);
                    response.Success = true;
                    response.Message = "Alterações realizadas com sucesso!";
                    response.Data = result.Data;


                }


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error to update moto in service");


                response.Success = false;
                response.Message = "Ocorreu um erro ao alterar a moto na plataforma.";

            }

            return response;

            
        }
    }
}

