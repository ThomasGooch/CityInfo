using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(ILogger<CitiesController> logger, CitiesDataStore citiesDataStore) {
            _logger = logger;
            _citiesDataStore = citiesDataStore;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            CityDto? result = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            if (result is null)
            {
                _logger.LogCritical($"city with id {id}, not found.");
                return NotFound();
            }

            return Ok(result);

        }
    }
}
