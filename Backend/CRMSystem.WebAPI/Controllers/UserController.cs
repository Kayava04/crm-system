using AutoMapper;
using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.DTOs.User;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/user/me")]
    public class UserController(
        UserService service,
        IValidatorFactory validatorFactory,
        IMapper mapper,
        ILogger<UserController> logger) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetMe()
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);
            logger.LogInformation($"Fetching profile for user with ID '{userId}'.");
            
            var user = await service.GetUserByIdAsync(userId);
            var userDto = mapper.Map<UserResponseDto>(user);
            
            logger.LogInformation($"Successfully fetched profile for user '{userDto.Username}'.");
            return Ok(userDto);
        }

        [HttpPatch]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            if (!validatorFactory.Validate(updateUserDto, out var errorMessage))
            {
                logger.LogWarning($"Update validation failed for user '{userId}'. Message: {errorMessage}");
                return BadRequest(errorMessage);
            }

            try
            {
                await service.UpdateUserAsync(
                    userId,
                    updateUserDto.FullName,
                    updateUserDto.BirthDate,
                    updateUserDto.Email);

                logger.LogInformation($"User '{userId}' successfully updated profile.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError($"Error while updating profile for user '{userId}'. Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("change-password")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);

            if (!validatorFactory.Validate(updateUserPasswordDto, out var errorMessage))
            {
                logger.LogWarning($"Update password validation failed for user '{userId}'. Message: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            try
            {
                await service.UpdateUserPasswordAsync(
                    userId,
                    updateUserPasswordDto.OldPassword,
                    updateUserPasswordDto.NewPassword,
                    updateUserPasswordDto.ConfirmNewPassword);
                
                logger.LogInformation($"User '{userId}' successfully changed password.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError($"Error while changing password for user '{userId}'. Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}