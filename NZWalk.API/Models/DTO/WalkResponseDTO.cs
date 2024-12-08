namespace NZWalk.API.Models.DTO
{
    public class WalkResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public DifficultyResponseDTO DifficultyResponse { get; set; }
        public RegionResponseDTO RegionResponse { get; set; }

    }
}
