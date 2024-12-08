using System.ComponentModel.DataAnnotations;

namespace NZWalk.API.Models.DTO
{
    public class UpdateWalkRequestDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name has to be a maximum of 50 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Description has to be a maximum of 200 characters")]
        public string Description { get; set; }
        [Required]
        [Range(0, 50, ErrorMessage = "Length must be between 0 and 50 KM")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
