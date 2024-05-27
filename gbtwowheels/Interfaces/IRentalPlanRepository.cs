using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IRentalPlanRepository
    {
        IEnumerable<RentalPlan> GetAll();
    }
}

