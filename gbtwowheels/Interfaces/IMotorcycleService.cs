using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IMotorcycleService
    {
        IEnumerable<Motorcycle> GetAllMotorcycles();
        Motorcycle GetMotorcycleById(int id);
        Task<ServiceResponse<Motorcycle>> Add(Motorcycle motorcycle);
        void UpdateMotorcycle(Motorcycle motorcycle);
        void DeleteMotorcycle(int id);

    }
}

