using AutoMapper;
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
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService localMailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService;
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            try
            {

                var cityExist = await _cityInfoRepository.GetCityAsync(cityId, false);

                if (cityExist is null)
                {
                    return NotFound();
                }

                var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest));

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "a problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int id)
        {

            var cityExist = await _cityInfoRepository.GetCityAsync(cityId, false);

            if (cityExist is null)
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, id);
            if (pointOfInterest is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }





        //[HttpPost]
        //public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestCreationDto pointOfInterest)
        //{

        //var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //if (cityFound is null)
        //{
        //    return NotFound();
        //}


        //// for now - demo only
        //var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

        //var finalPointOfInterest = new PointOfInterestDto { Id = maxPointOfInterestId + 1, Name = pointOfInterest.Name, Description = pointOfInterest.Description };

        //cityFound.PointsOfInterest.Add(finalPointOfInterest);

        //return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);



        //}

        //[HttpPut("{pointOfInterestId}")]
        //public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestUpdateDto pointOfInterest)
        //{

        //var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //if (cityFound is null)
        //{
        //    return NotFound();
        //}



        //var pointOfInterestForUpdate = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

        //if (pointOfInterestForUpdate is null) return NotFound();



        //pointOfInterestForUpdate.Name = pointOfInterest.Name;
        //pointOfInterestForUpdate.Description = pointOfInterest.Description;



        //return NoContent();


        //}

        //[HttpPatch("{pointOfInterestId}")]
        //public ActionResult<PointOfInterestDto> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        //{
        //    var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (cityFound is null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestForUpdate = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

        //    if (pointOfInterestForUpdate is null) return NotFound();

        //    var pointOfInterestToPatch = new PointOfInterestUpdateDto
        //    {
        //        Name = pointOfInterestForUpdate.Name,
        //        Description = pointOfInterestForUpdate.Description
        //    };

        //    patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    if (!TryValidateModel(pointOfInterestToPatch))
        //    {
        //        return BadRequest(pointOfInterestToPatch);
        //    }

        //    pointOfInterestForUpdate.Name = pointOfInterestToPatch.Name;
        //    pointOfInterestForUpdate.Description = pointOfInterestToPatch.Description;

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public ActionResult DeletePointOfInterest(int cityId, int id)
        //{
        //    var cityFound = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (cityFound is null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestForDeletion = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == id);
        //    if (pointOfInterestForDeletion is null)
        //    {
        //        return NotFound();
        //    }

        //    cityFound.PointsOfInterest.Remove(pointOfInterestForDeletion);
        //    _localMailService.Send("Deletion Requested", $"point of interest with cityId {cityId}, and pointOfInterestId {id} DELETED.");
        //    return NoContent();
        //}
    }
}
