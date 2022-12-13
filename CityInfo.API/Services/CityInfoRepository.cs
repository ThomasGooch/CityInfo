using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;
        private readonly ILogger<CityInfoRepository> _logger;

        public CityInfoRepository(CityInfoContext context, ILogger<CityInfoRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(searchQuery))
            {
                return await _context.Cities.OrderBy(c => c.Name).ToListAsync(); 
            }
            
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrEmpty(name)) {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }
            if (!string.IsNullOrEmpty(searchQuery)) {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery) || (a.Description != null && a.Description.Contains(searchQuery)));
            }
            return await collection.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int id)
        {
            return await _context.Cities.AnyAsync(c => c.Id == id);
        }
        public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public Task CreateCityAsync(City city)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCityAsync(int id, City city)
        {
            throw new NotImplementedException();
        }

        public Task PatchCityAsync(int id, JsonPatchDocument<CityUpdateWithoutPointsOfInterestDto> document)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCityAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }

        }

        public Task UpdatePointOfInterestAsync(int cityId, int id, PointOfInterest pointOfInterest)
        {
            throw new NotImplementedException();
        }

        public Task PatchPointOfInterestAsync(int cityId, int id, JsonPatchDocument<PointOfInterestUpdateDto> document)
        {
            throw new NotImplementedException();
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {

            _context.PointsOfInterest.Remove(pointOfInterest);

        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
