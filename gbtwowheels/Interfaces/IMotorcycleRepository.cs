using System;
using gbtwowheels.Filters;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IMotorcycleRepository
    {
        IEnumerable<Motorcycle> GetAll();
        Motorcycle GetById(int id);
        Task<ServiceResponse<Motorcycle>> Add(Motorcycle motorcycle);
        Task<ServiceResponse<Motorcycle>> Update(Motorcycle motorcycle);
        void Delete(int id);
        Task<bool> IsExistingMotorcycle(Motorcycle motorcycle);
        IEnumerable<Motorcycle> GetByFilter(MotorcycleFilters filter);
    }
}

