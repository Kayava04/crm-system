using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.DTOs.Email;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController(
        EmailService emailService,
        ILogger<EmailController> logger) : ControllerBase
    {
        [HttpPost("send")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
        {
            var message = await emailService.SendMessageAsync(messageDto);
            
            logger.LogInformation($"Message was sent successfully. Message ID: {message.Id}.");
            return Ok(message);
        }

        [HttpGet("messages")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await emailService.GetAllMessagesAsync();
            
            logger.LogInformation($"Messages were retrieved successfully. Total Count: {messages.Count()}");
            return Ok(messages);
        }
        
        [HttpGet("messages/{receiverId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.UserOrAdmin)]
        public async Task<IActionResult> GetMessagesByReceiverId(Guid receiverId)
        {
            var messages = await emailService.GetMessagesByReceiverIdAsync(receiverId);
            
            logger.LogInformation($"All messages were retrieved for ID: {receiverId}. Total Count: {messages.Count()}");
            return Ok(messages);
        }
    }
}