namespace CityInfo.API.Models
{
    /// <summary>
    /// DTO for city without points of interest
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        /// <summary>
        /// Id of city
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of city
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description of city
        /// </summary>
        public string? Description { get; set; }

    }
}
