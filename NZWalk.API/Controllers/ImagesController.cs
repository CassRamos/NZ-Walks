using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm] ImageUploadRequestDTO uploadRequestDTO)
        {
            ValidateFileUpload(uploadRequestDTO);

            if (ModelState.IsValid)
            {
                var imageDomain = new Image
                {
                    File = uploadRequestDTO.File,
                    FileExtension = Path.GetExtension(uploadRequestDTO.File.FileName),
                    FileSizeInBytes = uploadRequestDTO.File.Length,
                    Filename = uploadRequestDTO.Filename,
                    FileDescription = uploadRequestDTO.FileDescription
                };

                await _imageRepository.Upload(imageDomain);

                return Ok(imageDomain);
            }

            return BadRequest();
        }

        private void ValidateFileUpload(ImageUploadRequestDTO requestDTO)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(requestDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (requestDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file");
            }
        }

    }
}
