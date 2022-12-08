using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ILogger<CitiesController> logger, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            var results = new List<CityWithoutPointsOfInterestDto>();
            foreach (var city in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = city.Id,
                    Name= city.Name,
                    Description= city.Description
                });
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            //CityDto? result = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            //if (result is null)
            //{
            //    _logger.LogCritical($"city with id {id}, not found.");
            //    return NotFound();
            //}

            return Ok();
            //return Ok(result);

        }
    }
}
