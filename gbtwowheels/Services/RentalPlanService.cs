using System;
using gbtwowheels.Controllers;
using gbtwowheels.Filters;
using gbtwowheels.Interfaces;
using gbtwowheels.Models;
using gbtwowheels.Repositories;
using gbtwowheels.Utils;

namespace gbtwowheels.Services
{
    public class RentalPlanService : IRentalPlanService
    {

        private readonly IRentalPlanRepository _rentalPlanRepository;
        private readonly ILogger<RentalPlanService> _logger;

        public RentalPlanService(IRentalPlanRepository rentalPlanRepository, ILogger<RentalPlanService> logger)
        {
            _rentalPlanRepository = rentalPlanRepository;
            _logger = logger;
        }



        public IEnumerable<RentalPlan> GetAllRentalPlans()
        {
            return _rentalPlanRepository.GetAll();
        }

    }
}

