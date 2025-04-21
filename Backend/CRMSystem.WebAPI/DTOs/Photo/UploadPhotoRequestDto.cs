using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.DTOs.Photo
{
    public class UploadPhotoRequestDto
    {
        [FromForm]
        public IFormFile File { get; set; } = null!;
    }
}