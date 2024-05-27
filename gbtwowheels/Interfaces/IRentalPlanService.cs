using System;
using gbtwowheels.Models;

namespace gbtwowheels.Interfaces
{
    public interface IRentalPlanService
    {
        IEnumerable<RentalPlan> GetAllRentalPlans();

    }
}

