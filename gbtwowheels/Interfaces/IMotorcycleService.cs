using System;
using gbtwowheels.Filters;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IMotorcycleService
    {
        IEnumerable<Motorcycle> GetAllMotorcycles();
        Motorcycle GetMotorcycleById(int id);
        Task<ServiceResponse<Motorcycle>> Add(Motorcycle motorcycle);
        Task<ServiceResponse<Motorcycle>> UpdateMotorcycle(Motorcycle motorcycle);
        void DeleteMotorcycle(int id);
        IEnumerable<Motorcycle> GetByFilterAsync(MotorcycleFilters filter);

    }
}

