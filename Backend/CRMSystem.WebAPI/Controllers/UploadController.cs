using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.DTOs.Photo;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UploadController(
        UserService service,
        ILogger<UploadController> logger) : ControllerBase
    {
        [HttpPost("upload-photo")]
        [Authorize(AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoRequestDto requestDto)
        {
            var userIdClaim = User.FindFirst("userId");

            if (userIdClaim == null)
            {
                logger.LogWarning("Upload photo attempt failed: Missing userId claim.");
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                logger.LogWarning($"Upload photo attempt failed: Invalid userId format: '{userIdClaim.Value}'.");
                return BadRequest();
            }
            
            logger.LogInformation($"User '{userId}' is attempting to upload a photo.");
            
            try
            {
                var imageUrl = await service.UploadUserPhoto(userId, requestDto.File, Request);
                
                logger.LogInformation($"User '{userId}' uploaded a photo successfully. Image URL: {imageUrl}.");
                return Ok(new { imageUrl });
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError($"Upload photo failed for user '{userId}'. Message: {ex.Message}.");
                return BadRequest(ex.Message);
            }
        }
    }
}