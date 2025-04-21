using CRMSystem.WebAPI.DTOs.School.GroupLessonDays;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/group-lesson-days")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GroupLessonDayController(GroupLessonDayService service)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{groupId:guid}/{lessonDayId:guid}")]
        public async Task<IActionResult> GetById(Guid groupId, Guid lessonDayId)
        {
            var item = await service.GetByIdAsync(groupId, lessonDayId);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupLessonDayDto dto)
        {
            var created = await service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpDelete("{groupId:guid}/{lessonDayId:guid}")]
        public async Task<IActionResult> Delete(Guid groupId, Guid lessonDayId)
        {
            await service.DeleteAsync(groupId, lessonDayId);
            return NoContent();
        }
    }
}