using gbtwowheels.Data;
using gbtwowheels.Models;
using gbtwowheels.Interfaces;

namespace gbtwowheels.Repositories
{
    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<RentalPlanRepository> _logger;

        public RentalPlanRepository(ApplicationDbContext context, ILogger<RentalPlanRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IEnumerable<RentalPlan> GetAll()
        {
            return _context.RentalPlans.ToList();
        }

        
       
    }
}
