using gbtwowheels.Data;
using gbtwowheels.Models;
using gbtwowheels.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gbtwowheels.Repositories
{
    public class RentRepository : IRentRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<RentRepository> _logger;

        public RentRepository(ApplicationDbContext context, ILogger<RentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResponse<Rent>> Add(Rent rent)
        {
            var response = new ServiceResponse<Rent>();

            try
            {
                _context.Rents.Add(rent);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = rent;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while add rent in database");

            }

            return response;
        }

        public IEnumerable<Rent> GetAll()
        {
            return _context.Rents.ToList();
        }

        public async Task<bool> IsRentActive(Rent rent)
        {
            try
            {
                var isRentActive = await _context.Rents.FirstOrDefaultAsync(u => u.UserId == rent.UserId);
                if (isRentActive != null)
                {
                    return true;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while analizying if the motorcycle already exists in database");

            }

            return false;
        }
    }
}

