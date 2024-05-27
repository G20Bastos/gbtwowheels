using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IRentService
    {
        IEnumerable<Rent> GetAllRents();
        Task<ServiceResponse<Rent>> Add(Rent rent);

    }
}

