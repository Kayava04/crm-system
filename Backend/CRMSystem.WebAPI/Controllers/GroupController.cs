using CRMSystem.WebAPI.DTOs.School.Groups;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/groups")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GroupController(GroupService groupService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await groupService.GetAllAsync();
            return Ok(groups);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var group = await groupService.GetByIdAsync(id);
            return group is null ? NotFound() : Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupDto dto)
        {
            var created = await groupService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateGroupDto dto)
        {
            var updated = await groupService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await groupService.DeleteAsync(id);
            return NoContent();
        }
    }
}