using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    /// <summary>
    /// DTO for point of interest creation
    /// </summary>
    public class PointOfInterestCreationDto
    {
        /// <summary>
        /// Name of point of interest
        /// </summary>
        [Required(ErrorMessage ="You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description for point of interest
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
