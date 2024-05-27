using System;
using gbtwowheels.Controllers;
using gbtwowheels.Filters;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Repositories;
using gbtwowheels.Utils;

namespace gbtwowheels.Services
{
    public class RentService : IRentService
    {

        private readonly IRentRepository _rentRepository;
        private readonly ILogger<RentService> _logger;

        public RentService(IRentRepository rentRepository, ILogger<RentService> logger)
        {
            _rentRepository = rentRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<Rent>> Add(Rent rent)
        {
            var response = new ServiceResponse<Rent>();

            try
            {
                if (await _rentRepository.IsRentActive(rent))
                {
                    response.Success = false;
                    response.Message = "Prezado usuário, você já tem uma locação ativa. Locação não realizada";

                }
                else
                {

                var result = await _rentRepository.Add(rent);
                response.Success = true;
                response.Message = "Locação realizada com sucesso!";
                response.Data = result.Data;
            }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error to add rent in service");


                response.Success = false;
                response.Message = "Ocorreu um erro ao realizar a locação na plataforma.";

            }

            return response;
        }

        public IEnumerable<Rent> GetAllRents()
        {
            return _rentRepository.GetAll();
        }

    }
}

