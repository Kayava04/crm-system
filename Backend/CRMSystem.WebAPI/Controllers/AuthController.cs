using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.DTOs.Auth;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(
        UserService userService,
        IValidatorFactory validatorFactory,
        ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request)
        {
            if (!validatorFactory.Validate(request, out var errorMessage))
            {
                logger.LogWarning($"SignUp validation failed: {errorMessage}");
                return BadRequest(errorMessage);
            }

            try
            {
                await userService.SignUp(request.FullName, request.BirthDate, request.Email, request.Username, request.Password);
            
                logger.LogInformation($"User '{request.Username}' successfully signed up.");
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError($"Sign-up failed for user '{request.Username}': {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request)
        {
            if (!validatorFactory.Validate(request, out var errorMessage))
            {
                logger.LogWarning($"Sign-in validation failed: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            var token = await userService.SignIn(request.Username, request.Password);
            Response.Cookies.Append("jwt", token);
            
            logger.LogInformation($"User '{request.Username}' successfully signed in.");
            return Ok();
        }

        [HttpPost("sign-out")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public new IActionResult SignOut()
        {
            Response.Cookies.Delete("jwt");
            var username = User.FindFirst("username")?.Value;
            
            logger.LogInformation($"User '{username}' signed out.");
            return Ok();
        }
    }
}