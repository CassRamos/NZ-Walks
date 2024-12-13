using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAcessor;
        private readonly NZWalksDbContext _nZWalksDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAcessor, NZWalksDbContext nZWalksDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAcessor = httpContextAcessor;
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.Filename}{image.FileExtension}");

            // uplaod image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAcessor.HttpContext.Request.Scheme}://{_httpContextAcessor.HttpContext.Request.Host}{_httpContextAcessor.HttpContext.Request.PathBase}/Images/{image.Filename}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // add image to the Images table
            await _nZWalksDbContext.AddAsync(image);
            await _nZWalksDbContext.SaveChangesAsync();

            return image;
        }
    }
}
