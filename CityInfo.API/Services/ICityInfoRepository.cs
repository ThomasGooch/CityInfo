using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(string? name);
        Task<City?> GetCityAsync(int cityId, bool includedPointsOfInterest);
        Task<bool> CityExistsAsync(int id);
       

        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        
        Task PatchPointOfInterestAsync(int cityId, int id, JsonPatchDocument<PointOfInterestUpdateDto> document);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();

    }
}
