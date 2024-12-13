using System.ComponentModel.DataAnnotations;

namespace NZWalk.API.Models.DTO
{
    public class ImageUploadRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Filename { get; set; }
        public string? FileDescription { get; set; }
    }
}
