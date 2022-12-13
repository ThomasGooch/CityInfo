namespace CityInfo.API.Models
{
    public class CityUpdateWithoutPointsOfInterestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
