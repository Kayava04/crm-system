using CRMSystem.WebAPI.DTOs.School.Contacts;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ContactController(ContactService contactService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await contactService.GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contact = await contactService.GetByIdAsync(id);
            return contact is null ? NotFound() : Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactDto dto)
        {
            var created = await contactService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateContactDto dto)
        {
            var updated = await contactService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await contactService.DeleteAsync(id);
            return NoContent();
        }
    }
}