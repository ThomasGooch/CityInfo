using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger
            , IMailService localMailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService;
            _citiesDataStore = citiesDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("example exception.");
                
                var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (cityFound is not null)
                {
                    return Ok(cityFound.PointsOfInterest);
                }
                _logger.LogInformation($"city with id: {cityId}, wasn't found when accessing points of interest.");
                return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "a problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int id)
        {

            var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is not null)
            {
                var pointOfInterest = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == id);
                if (pointOfInterest is null)
                {
                    return NotFound();
                }
                return Ok(pointOfInterest);
            }
            return NotFound();

        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestCreationDto pointOfInterest)
        {

            var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is null)
            {
                return NotFound();
            }


            // for now - demo only
            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto { Id = maxPointOfInterestId + 1, Name = pointOfInterest.Name, Description = pointOfInterest.Description };

            cityFound.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);


        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestUpdateDto pointOfInterest)
        {

            var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is null)
            {
                return NotFound();
            }



            var pointOfInterestForUpdate = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestForUpdate is null) return NotFound();



            pointOfInterestForUpdate.Name = pointOfInterest.Name;
            pointOfInterestForUpdate.Description = pointOfInterest.Description;



            return NoContent();


        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is null)
            {
                return NotFound();
            }

            var pointOfInterestForUpdate = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestForUpdate is null) return NotFound();

            var pointOfInterestToPatch = new PointOfInterestUpdateDto
            {
                Name = pointOfInterestForUpdate.Name,
                Description = pointOfInterestForUpdate.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(pointOfInterestToPatch);
            }

            pointOfInterestForUpdate.Name = pointOfInterestToPatch.Name;
            pointOfInterestForUpdate.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePointOfInterest(int cityId, int id)
        {
            var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is null)
            {
                return NotFound();
            }

            var pointOfInterestForDeletion = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestForDeletion is null)
            {
                return NotFound();
            }

            cityFound.PointsOfInterest.Remove(pointOfInterestForDeletion);
            _localMailService.Send("Deletion Requested", $"point of interest with cityId {cityId}, and pointOfInterestId {id} DELETED.");
            return NoContent();
        }
    }
}
