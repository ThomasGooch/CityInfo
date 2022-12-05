using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {

            var cityFound = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is not null)
            {
                return Ok(cityFound.PointsOfInterest);
            }
            return NotFound();

        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int id)
        {

            var cityFound = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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

            var cityFound = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is null)
            {
                return NotFound();
            }

            try
            {
                // for now - demo only
                var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

                var finalPointOfInterest = new PointOfInterestDto { Id = maxPointOfInterestId + 1, Name = pointOfInterest.Name, Description = pointOfInterest.Description };

                cityFound.PointsOfInterest.Add(finalPointOfInterest);

                return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);

            }
            catch (Exception)
            {
                Console.WriteLine("Error: creation failed");
                return NotFound();
            }
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestUpdateDto pointOfInterest)
        {

            var cityFound = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cityFound is null)
            {
                return NotFound();
            }

            try
            {

                var pointOfInterestForUpdate = cityFound.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

                if (pointOfInterestForUpdate is null) return NotFound();

               

                pointOfInterestForUpdate.Name = pointOfInterest.Name;
                pointOfInterestForUpdate.Description = pointOfInterest.Description;



                return NoContent();

            }
            catch (Exception)
            {
                Console.WriteLine("Error: update failed");
                return NotFound();
            }
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            var cityFound = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
        public ActionResult DeletePointOfInterest(int cityId, int id) {
            var cityFound = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
            return NoContent();
        }
    }
}
