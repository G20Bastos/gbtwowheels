using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IRentRepository
    {
        IEnumerable<Rent> GetAll();
        Task<ServiceResponse<Rent>> Add(Rent rent);
        Task<bool> IsRentActive(Rent rent);
    }
}

