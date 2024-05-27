using System.Collections.Generic;
using System.Linq;
using gbtwowheels.Data;
using gbtwowheels.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using BCrypt.Net;
using gbtwowheels.Interfaces;
using Azure;
using gbtwowheels.Controllers;
using gbtwowheels.Filters;

namespace gbtwowheels.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<MotorcycleRepository> _logger;

        public MotorcycleRepository(ApplicationDbContext context, ILogger<MotorcycleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResponse<Motorcycle>> Add(Motorcycle motorcycle)
        {
            var response = new ServiceResponse<Motorcycle>();

            try
            {
                _context.Motorcycles.Add(motorcycle);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = motorcycle;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while add motorcycle in database");

            }

            return response;
        }

        public void Delete(int id)
        {
            var motorcycle = GetById(id);
            if (motorcycle != null)
            {
                _context.Motorcycles.Remove(motorcycle);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Motorcycle> GetAll()
        {
            return _context.Motorcycles.ToList();
        }

        public IEnumerable<Motorcycle> GetByFilter(MotorcycleFilters filters)
        {
            IQueryable<Motorcycle> query = _context.Motorcycles;

            if (filters.Year.HasValue && filters.Year != 0)
            {
                query = query.Where(m => m.Year == filters.Year);
            }

            if (!string.IsNullOrEmpty(filters.Model))
            {
                query = query.Where(m => m.Model!.Contains(filters.Model));
            }

            if (!string.IsNullOrEmpty(filters.LicensePlate))
            {
                query = query.Where(m => m.LicensePlate!.Contains(filters.LicensePlate));
            }

            if (!string.IsNullOrEmpty(filters.Color))
            {
                query = query.Where(m => m.Color!.Contains(filters.Color));
            }

            if (filters.EngineCapacity.HasValue && filters.EngineCapacity != 0)
            {
                query = query.Where(m => m.EngineCapacity == filters.EngineCapacity);
            }

            
           query = query.Where(m => m.IsAvailable == filters.isAvailable);
            

            return query.ToList();
        }
    

        public Motorcycle GetById(int id)
        {
            return _context.Motorcycles.Find(id);
        }

        public async Task<Motorcycle> GetMotorcycleAvailable()
        {
            var existingMotorcycle = await _context.Motorcycles.FirstOrDefaultAsync(u => u.IsAvailable == true);
            if (existingMotorcycle != null)
            {
                return existingMotorcycle;
            }

            return null!;
            

        }
        

        public async Task<bool> IsExistingMotorcycle(Motorcycle motorcycle)
        {
            try
            {
                var existingMotorcycle = await _context.Motorcycles.FirstOrDefaultAsync(u => u.LicensePlate == motorcycle.LicensePlate);
                if (existingMotorcycle != null)
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

        public async Task<ServiceResponse<Motorcycle>> Update(Motorcycle motorcycle)
        {

            var response = new ServiceResponse<Motorcycle>();

            try
            {
                _context.Motorcycles.Update(motorcycle);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = motorcycle;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while update motorcycle in database");

            }

            return response;

          
        }

       
    }
}
